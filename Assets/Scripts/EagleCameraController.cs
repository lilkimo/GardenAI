using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EagleCameraController : MonoBehaviour
{
    [HideInInspector]
    public new Camera camera;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }
    void Update() {}
}
