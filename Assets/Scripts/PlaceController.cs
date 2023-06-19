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
    
    [SerializeField]
    private GameObject plant;

    private CameraController cameraController;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();

        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += PlacePlant;
    }

    public void PlacePlant(EnhancedTouch.Finger finger)
    {
        if (cameraController.aRMode)
            aRPlacePlant.PlacePlant(finger, plant);
        else
            eaglePlacePlant.PlacePlant(finger, plant);
    }
}
