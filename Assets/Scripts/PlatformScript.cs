using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public GameObject targetPlatform;
    public float teleportXOffset = 7.5f;

    private WorldController controller;
    private Renderer currentPlatformRenderer;
    private Renderer targetPlatformRenderer;
    private Transform targetPlatformTransform;
    private float prevDelta;

    private bool prevIsCurrentPlatformVisible;
    private bool prevIsTargetPlatformVisible;
    // Start is called before the first frame update
    void Start()
    {
        controller = transform.parent.GetComponent<WorldController>();
        prevDelta = controller.delta;

        currentPlatformRenderer = GetComponent<Renderer>();
        targetPlatformRenderer = targetPlatform.GetComponent<Renderer>();
        targetPlatformTransform = targetPlatform.GetComponent<Transform>();

        prevIsCurrentPlatformVisible = currentPlatformRenderer.isVisible;
        prevIsTargetPlatformVisible = targetPlatformRenderer.isVisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Sign(controller.delta) != Mathf.Sign(prevDelta) && !currentPlatformRenderer.isVisible) {
            transform.localPosition = new Vector2(
                targetPlatformTransform.localPosition.x + (teleportXOffset * Mathf.Sign(controller.delta)), 
                targetPlatformTransform.localPosition.y
            );
        } else if (!currentPlatformRenderer.isVisible && prevIsCurrentPlatformVisible && targetPlatformRenderer.isVisible) {
            transform.localPosition = new Vector2(
                targetPlatformTransform.localPosition.x + (teleportXOffset * Mathf.Sign(controller.delta)), 
                targetPlatformTransform.localPosition.y
            );
        }
        prevIsCurrentPlatformVisible = currentPlatformRenderer.isVisible;
        prevIsTargetPlatformVisible = targetPlatformRenderer.isVisible;

        prevDelta = controller.delta;
    }
}
