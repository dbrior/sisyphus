using UnityEngine;

public class ClickShrinkEffect : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    public float scaleFactor = 0.8f;
    private float scaleSpeed = 5f;  // Speed of the scale animation
    private bool isShrinking = false;  // Track if we should be shrinking
    public float shrinkDuration = 0.5f;  // Duration to complete one cycle (shrink and grow back)
    private float timeSinceStarted = 0f;  // Track time since the animation started

    void Start()
    {
        originalScale = transform.localScale;  // Store the original scale
        targetScale = originalScale * scaleFactor;  // Target scale is 80% of the original
    }

    void Update()
    {
        // if (isShrinking)
        // {
        //     // Increment time since started
        //     timeSinceStarted += Time.deltaTime;

        //     // Calculate current progress in the animation cycle
        //     float progress = timeSinceStarted / shrinkDuration;

        //     // Check if animation should still be playing
        //     if (progress < 0.5f)  // First half: shrinking
        //     {
        //         transform.localScale = Vector3.Lerp(originalScale, targetScale, progress * 2);  // Multiply progress by 2 to complete the action in half the duration
        //     }
        //     else if (progress < 1.0f)  // Second half: growing back
        //     {
        //         transform.localScale = Vector3.Lerp(targetScale, originalScale, (progress - 0.5f) * 2);  // Normalize progress to range [0,1]
        //     }
        //     else  // Animation completed
        //     {
        //         isShrinking = false;
        //         timeSinceStarted = 0f;
        //         transform.localScale = originalScale;  // Ensure scale is set to original at the end
        //     }
        // }
    }

    private void OnMouseDown()
    {
        Vector3 clickLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickLocation.z = 0;
        WorldController.Instance.ManualClick(clickLocation);

        transform.localScale = Vector3.Lerp(originalScale, targetScale, 1f);

        // isShrinking = true;
        // timeSinceStarted = 0f;

        // if (!isShrinking)  // Only allow new shrink if not already shrinking
        // {
        //     isShrinking = true;
        //     timeSinceStarted = 0f;
        // }
    }
    private void OnMouseUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, 1f);
    }
}
