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

    public void PlacePlant(EnhancedTouch.Finger finger, GameObject plant)
    {
        Debug.Log("Eagle PlacePlant call");
        if(finger.index != 0)
            return;
        Ray ray = camera.ScreenPointToRay(finger.currentTouch.screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            GameObject obj = Instantiate(plant, hit.point, Quaternion.Euler(Vector3.zero));
        }
    }
}
