using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using System;

[RequireComponent(typeof(CameraController))]
// Esta weá debería llamarse InputManager
public class PlaceController : MonoBehaviour
{   
    [SerializeField]
    private ARPlacePlant aRPlacePlant;
    [SerializeField]
    private EaglePlacePlant eaglePlacePlant;
    
    private GameObject plant;

    private CameraController cameraController;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();

        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    public void EnablePlaceMode()
    {
        EnhancedTouch.Touch.onFingerDown += PlacePlant;
        EnhancedTouch.Touch.onFingerMove -= onFingerMove;
        
        EnhancedTouch.Touch.onFingerDown -= SelectPlant;
        EnhancedTouch.Touch.onFingerUp -= DeselectOrDeletePlant;
        
        EnhancedTouch.Touch.onFingerMove -= itWasNotATap;
        // EnhancedTouch.Touch -= RotatePlant;
        // EnhancedTouch.Touch -= ScalePlant;
    }

    public void DisablePlaceMode()
    {
        EnhancedTouch.Touch.onFingerDown -= PlacePlant;
        EnhancedTouch.Touch.onFingerMove += onFingerMove;

        EnhancedTouch.Touch.onFingerDown += SelectPlant;
        EnhancedTouch.Touch.onFingerUp += DeselectOrDeletePlant;
        
        EnhancedTouch.Touch.onFingerMove += itWasNotATap;
        // EnhancedTouch.Touch += RotatePlant;
        // EnhancedTouch.Touch += ScalePlant;
    }

    public void SetPlant(GameObject desiredPlant)
    {
        Debug.Log($"Planta cambiada a {desiredPlant}");
        plant = desiredPlant;
    }

    private Transform selectedPlant = null;
    private bool isTap;
    public void SelectPlant(EnhancedTouch.Finger finger)
    {
        isTap = true;

        if (cameraController.aRMode)
            selectedPlant = aRPlacePlant.GetPlant(finger);
        else
            selectedPlant = eaglePlacePlant.GetPlant(finger);
        if (selectedPlant != null)
            Debug.Log($"Selected plant: {selectedPlant}");
    }

    public void itWasNotATap(EnhancedTouch.Finger _) => isTap = false;

    public void DeselectOrDeletePlant(EnhancedTouch.Finger _)
    {
        if (selectedPlant != null)
        {
            Debug.Log($"Deselected plant: {selectedPlant}");
            if (isTap)
                Destroy(selectedPlant.gameObject);
        }

        selectedPlant = null;
    }

    public void PlacePlant(EnhancedTouch.Finger finger)
    {
        Pose? plantPose;
        if (cameraController.aRMode)
            plantPose = aRPlacePlant.PlacePlant(finger);
        else
            plantPose = eaglePlacePlant.PlacePlant(finger);
        
        if (plantPose.HasValue)
            Instantiate(plant, plantPose.Value.position, plantPose.Value.rotation, cameraController.virtualGarden.transform);
    }

    private void onFingerMove(EnhancedTouch.Finger finger)
    {
        if (selectedPlant == null)
        {
            if (!cameraController.aRMode)
                eaglePlacePlant.MoveCamera(finger.screenPosition - finger.touchHistory[1].screenPosition);
        }
        else
            ScaleRotatePlant(finger);
    }

    private void ScaleRotatePlant(EnhancedTouch.Finger finger)
    {
        if (selectedPlant == null)
            return;
        
        Vector2 plantScreenPosition;
        if (cameraController.aRMode)
            plantScreenPosition = aRPlacePlant.GetPlantScreenPosition(selectedPlant);
        else
            plantScreenPosition = eaglePlacePlant.GetPlantScreenPosition(selectedPlant);
        Vector2 relativePosition = finger.screenPosition - plantScreenPosition;
        float angle = Mathf.Abs(Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg);
        if (angle > 90)
            angle = 180 - angle;
        
        // En el futuro, cuando la escala si dependa de la escala original de la planta,
        // hay que reemplazar deltaScreenPosition por relativePosition
        Vector2 deltaScreenPosition = finger.screenPosition - finger.touchHistory[1].screenPosition;
        if (angle <= 10)
            RotatePlant(deltaScreenPosition.x);
        else if (angle >= 80)
            ScalePlant(deltaScreenPosition.y);
    }

    private void ScalePlant(float magnitude)
    {
        //Vector2 deltaScreenPosition = finger.screenPosition - finger.touchHistory[1].screenPosition;
        // Aquí vvvv vamos a tener que dividir deltaScreenPosition por un múltiplo de la pantalla para
        // que en todos los dispositivos funcione igual.
        //Vector3 deltaVector = Vector3.one*Mathf.Clamp(deltaScreenPosition.y/100, -1, 1);
        Vector3 deltaVector = Vector3.one*Mathf.Clamp(magnitude/100, -1, 1);
        
        // Si la selectedPlant no parte con escala (1, 1, 1) esta weá va a explotar a la mierda.
        // Para arreglarlo hay que guardar la escala original de <selectedPlant> y en <deltaVector>
        // en vez de multiplicar por Vector3.one vamos a tener que multiplicar por la escala original
        // y seguramente clampear el resultado por valores más chicos como +-.1f. Y luego hacer la pedazo
        // de condición corte (selectedPlant.localScale + deltaVector).x <= originalScale.x &&
        // (selectedPlant.localScale + deltaVector).y <= originalScale.y && ...
        if ((selectedPlant.localScale + deltaVector).x <= 4 && (selectedPlant.localScale + deltaVector).x >= 1)
            selectedPlant.localScale += deltaVector;
    }


    private void RotatePlant(float magnitude)
    {
        // Aquí vvvv vamos a tener que dividir <magnitude> por un múltiplo de la pantalla para
        // que en todos los dispositivos funcione igual.
        selectedPlant.localRotation = Quaternion.Euler(selectedPlant.localRotation.eulerAngles + new Vector3(0, magnitude/2, 0));
    }
}
