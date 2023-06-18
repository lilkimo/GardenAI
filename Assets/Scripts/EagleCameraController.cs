using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EagleCameraController : MonoBehaviour
{
    [SerializeField]
    private Camera ARCamera;

    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        SyncCamera();
    }
    void Update() {}

    public void SyncCamera() {
        Vector3 newPosition = ARCamera.transform.position;
        newPosition.y = this.transform.position.y;
        this.transform.position = newPosition;

        Vector3 newRotation = ARCamera.transform.rotation.eulerAngles;
        newRotation.x = 90;
        this.transform.rotation = Quaternion.Euler(newRotation);
    }
}
