using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public bool aRMode;
    
    [SerializeField]
    private Camera aRCamera;
    [SerializeField]
    private Camera eagleCamera;
    [SerializeField]
    private GameObject virtualGarden;
    
    void Start()
    {
        SwitchCamera(aRMode);
    }

    void SwitchCamera(bool isAREnabled)
    {
        aRMode = isAREnabled;

        virtualGarden.SetActive(!aRMode);

        aRCamera.enabled = aRMode;
        eagleCamera.enabled = !aRMode;
    }

    public void SwitchCamera()
    {
        SwitchCamera(!aRMode);
    }
}
