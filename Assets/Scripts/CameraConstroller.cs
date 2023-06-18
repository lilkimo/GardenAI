using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstroller : MonoBehaviour
{
    [SerializeField]
    private Camera ARCamera;
    [SerializeField]
    private EagleCameraController EagleCamera;
    [SerializeField]
    public bool ARMode;
    // Start is called before the first frame update
    void Start()
    {
        ARCamera.enabled = ARMode;
        EagleCamera.enabled = !ARMode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchCamera(bool isAREnabled) {
        ARMode = isAREnabled;
        ARCamera.enabled = ARMode;
        EagleCamera.camera.enabled = !ARMode;
    }

    public void SwitchCamera()
    {
        SwitchCamera(!ARMode);
    }
}
