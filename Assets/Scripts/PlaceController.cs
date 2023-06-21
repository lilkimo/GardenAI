using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

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
        EnhancedTouch.Touch.onFingerDown -= DeletePlant;
        EnhancedTouch.Touch.onFingerMove -= MovePlant;
        // EnhancedTouch.Touch -= RotatePlant;
        // EnhancedTouch.Touch -= ScalePlant;
    }

    public void DisablePlaceMode()
    {
        EnhancedTouch.Touch.onFingerDown -= PlacePlant;
        EnhancedTouch.Touch.onFingerDown += DeletePlant;
        EnhancedTouch.Touch.onFingerMove += MovePlant;
        // EnhancedTouch.Touch += RotatePlant;
        // EnhancedTouch.Touch += ScalePlant;
    }

    public void SetPlant(GameObject desiredPlant)
    {
        Debug.Log($"Planta cambiada a {desiredPlant}");
        plant = desiredPlant;
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

    private void DeletePlant(EnhancedTouch.Finger finger)
    {
        Debug.Log("Delete Plant");
    }

    private void MovePlant(EnhancedTouch.Finger finger)
    {
        Debug.Log("Move Plant");
    }


    private void RotatePlant(EnhancedTouch.Finger finger)
    {
        Debug.Log("Rotate Plant");
    }

    private void ScalePlant(EnhancedTouch.Finger finger)
    {
        Debug.Log("Scale Plant");
    }
}
