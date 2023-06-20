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
        Pose? plantPose;
        if (cameraController.aRMode)
            plantPose = aRPlacePlant.PlacePlant(finger);
        else
            plantPose = eaglePlacePlant.PlacePlant(finger);
        if (plantPose != null)
        {
            // Esta variable está para que el compilador no webé, ya que como plantPose puede ser nulo, toda
            // esta weá explota (Aunque la condición de arriba evita hacer cualquier weá si plantPose == null)
            Pose _plantPose = plantPose.GetValueOrDefault();
            GameObject obj = Instantiate(plant, _plantPose.position, _plantPose.rotation, cameraController.virtualGarden.transform);
        }
    }
}
