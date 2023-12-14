using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderCam : MonoBehaviour
{
    public Transform boulder;

    public float x_offset;
    public float y_offset;
    public float zoomSpeed = 1f; // Speed of zooming
    public float minOrthoSize = 1f; // Minimum orthographic size
    public float maxOrthoSize = 10f; // Maximum orthographic size
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); // Get the Camera component
        // cam.orthographicSize = 3.0f;
        // x_offset = -0.3f;
        // y_offset = 0.55f;
    }

    void Update()
    {
        transform.position = new Vector3(boulder.position.x + x_offset, boulder.position.y + y_offset, transform.position.z);

        // float scroll = Input.GetAxis("Mouse ScrollWheel");
        // cam.orthographicSize -= scroll * zoomSpeed;
        // cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);
    }
}
