using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(Camera))]
public class EaglePlacePlant : MonoBehaviour
{
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public Pose? PlacePlant(EnhancedTouch.Finger finger)
    {
        Debug.Log("Eagle PlacePlant call");
        if(finger.index == 0) {
            Ray ray = camera.ScreenPointToRay(finger.currentTouch.screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 50f))
                return new Pose(hit.point, Quaternion.Euler(Vector3.zero));
        }
        return null;
    }
}
