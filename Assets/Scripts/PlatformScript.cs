using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public GameObject targetPlatform;
    public float teleportXOffset = 7.5f;

    private Renderer currentPlatformRenderer;
    private Renderer targetPlatformRenderer;
    private Transform targetPlatformTransform;

    private bool prevIsCurrentPlatformVisible;
    private bool prevIsTargetPlatformVisible;
    // Start is called before the first frame update
    void Start()
    {
        currentPlatformRenderer = GetComponent<Renderer>();
        targetPlatformRenderer = targetPlatform.GetComponent<Renderer>();
        targetPlatformTransform = targetPlatform.GetComponent<Transform>();

        prevIsCurrentPlatformVisible = currentPlatformRenderer.isVisible;
        prevIsTargetPlatformVisible = targetPlatformRenderer.isVisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "Platform A") {
            Debug.Log("Platform A isVisible: " + currentPlatformRenderer.isVisible.ToString());
        }
        // Debug.Log("Current Platform Visible: " + currentPlatformRenderer.isVisible.ToString());
        // Debug.Log("Target Platform Visible: " + targetPlatformRenderer.isVisible.ToString());

        if (!currentPlatformRenderer.isVisible && prevIsCurrentPlatformVisible && targetPlatformRenderer.isVisible) {
            Debug.Log(name + " teleporting");
            transform.localPosition = new Vector2(
                targetPlatformTransform.localPosition.x + teleportXOffset, 
                targetPlatformTransform.localPosition.y
            );
        }
        prevIsCurrentPlatformVisible = currentPlatformRenderer.isVisible;
        prevIsTargetPlatformVisible = targetPlatformRenderer.isVisible;
    }
}
