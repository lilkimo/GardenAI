using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ARPlacePlant : MonoBehaviour
{
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    public void PlacePlant(EnhancedTouch.Finger finger, GameObject plant)
    {
        Debug.Log("AR PlacePlant call");
        if(finger.index != 0)
            return;
        if(aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.Planes))
        {
            foreach (ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(plant, pose.position, pose.rotation);
            }
        }
    }
}
