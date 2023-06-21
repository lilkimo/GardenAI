using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using System;

[RequireComponent(typeof(CameraController))]
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
        EnhancedTouch.Touch.onFingerMove -= ScaleRotatePlant;
        
        EnhancedTouch.Touch.onFingerDown -= SelectPlant;
        EnhancedTouch.Touch.onFingerUp -= DeselectOrDeletePlant;
        
        EnhancedTouch.Touch.onFingerMove -= itWasntATap;
        // EnhancedTouch.Touch -= RotatePlant;
        // EnhancedTouch.Touch -= ScalePlant;
    }

    public void DisablePlaceMode()
    {
        EnhancedTouch.Touch.onFingerDown -= PlacePlant;
        EnhancedTouch.Touch.onFingerMove += ScaleRotatePlant;

        EnhancedTouch.Touch.onFingerDown += SelectPlant;
        EnhancedTouch.Touch.onFingerUp += DeselectOrDeletePlant;
        
        EnhancedTouch.Touch.onFingerMove += itWasntATap;
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
        Debug.Log($"Selected plant: {selectedPlant}");
    }

    public void itWasntATap(EnhancedTouch.Finger _) => isTap = false;

    public void DeselectOrDeletePlant(EnhancedTouch.Finger _)
    {
        if (isTap && selectedPlant != null)
            Destroy(selectedPlant.gameObject);

        Debug.Log($"Deselected plant: {selectedPlant}");
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

    private void ScaleRotatePlant(EnhancedTouch.Finger finger)
    {
        if (selectedPlant == null)
            return;
        
        Vector2 deltaScreenPosition = finger.screenPosition - finger.touchHistory[1].screenPosition;
        //Debug.Log(finger.screenPosition - Camera.WorldToScreenPoint(selectedPlant.transform.position));
        if (Mathf.Abs(deltaScreenPosition.x) >= Mathf.Abs(deltaScreenPosition.y))
            RotatePlant(deltaScreenPosition.x);
        else
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
        selectedPlant.rotation = Quaternion.Euler(selectedPlant.rotation.eulerAngles + new Vector3(0, magnitude/2, 0));
    }
}
