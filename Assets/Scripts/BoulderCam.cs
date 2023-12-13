using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderCam : MonoBehaviour
{
    public Transform boulder;

    public float zoomSpeed = 1f; // Speed of zooming
    public float minOrthoSize = 1f; // Minimum orthographic size
    public float maxOrthoSize = 10f; // Maximum orthographic size
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); // Get the Camera component
    }

    void Update()
    {
        transform.position = new Vector3(boulder.position.x, boulder.position.y, transform.position.z);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);
    }
}
