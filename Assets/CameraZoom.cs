using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public float targetZoom = 5f; // The zoomed-in field of view
    public float zoomFactor = 3f; // How much to zoom
    public float zoomSpeed = 10f; // Speed of zoom
    public float duration = 0.5f; // Duration of the zoom effect

    private float originalZoom; // To store the original field of view
    private float currentZoom; // Current target zoom level
    private bool isZooming = false; // Flag to check if currently zooming

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        originalZoom = cam.fieldOfView;
        currentZoom = originalZoom;
    }

    // Call this method from wherever you detect the click
    public void OnObjectClick()
    {
        if (!isZooming)
        {
            StartCoroutine(ZoomCamera());
        }
        else
        {
            // Update the target zoom to keep the zooming smooth
            currentZoom = targetZoom;
        }
    }

    IEnumerator ZoomCamera()
    {
        isZooming = true;
        float timeElapsed = 0;

        // First, zoom in quickly
        while (cam.fieldOfView != targetZoom && timeElapsed < duration)
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetZoom, zoomSpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Reset time and start zooming out
        timeElapsed = 0;
        while (cam.fieldOfView != originalZoom)
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, originalZoom, zoomSpeed * Time.deltaTime);
            yield return null;
        }

        isZooming = false;
    }
}
