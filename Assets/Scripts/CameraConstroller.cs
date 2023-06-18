using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstroller : MonoBehaviour
{
    [SerializeField]
    private Camera ARCamera;
    [SerializeField]
    private Camera EagleCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchCamera()
    {
        bool isAREnabled = ARCamera.enabled;
        ARCamera.enabled = !isAREnabled;
        EagleCamera.enabled = isAREnabled;
    }
}
