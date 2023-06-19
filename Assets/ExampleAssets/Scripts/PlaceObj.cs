using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlaceObj : MonoBehaviour
{
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] private GameObject planta;

    private void Awake(){
        Debug.Log("ARInteractionsManager enabled.");
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable(){
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable(){
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger){
        if(finger.index != 0) return;
        if(aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.Planes)){
            foreach (ARRaycastHit hit in hits){
                Pose pose = hit.pose;
                GameObject obj = Instantiate(planta, pose.position, pose.rotation);
            }
        }
    }
}
