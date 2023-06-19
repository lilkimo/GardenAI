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
    [SerializeField]
    public GameObject VirtualGarden;
    
    void Start()
    {
        SwitchCamera(ARMode);
    }
    void Update() {}

    void SwitchCamera(bool isAREnabled) {
        ARMode = isAREnabled;

        VirtualGarden.SetActive(!ARMode);

        ARCamera.enabled = ARMode;
        EagleCamera.camera.enabled = !ARMode;
    }

    public void SwitchCamera()
    {
        SwitchCamera(!ARMode);
    }
}
