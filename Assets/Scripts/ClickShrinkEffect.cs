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
    private Collider2D collider;
    private bool pressed = false;
    public void UpdateScale(Vector3 newScale) {
        originalScale = newScale;
        targetScale = originalScale * scaleFactor;
    }
    void Start()
    {
        originalScale = transform.localScale;  // Store the original scale
        targetScale = originalScale * scaleFactor;  // Target scale is 80% of the original
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // if(Input.anyKeyDown)
        // {
        //     float offset = 0.2f;
        //     float yShift = 0.2f;
        //     Touched(transform.position + new Vector3(Random.Range(-offset,offset), Random.Range(-offset+yShift,offset+yShift), 0));
        // }
        if (!WorldController.Instance.frozen && Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosWorld.z = 0;
                    
                    if (collider.OverlapPoint(touchPosWorld))
                    {
                        Touched(touchPosWorld);
                    }
                }
            }
        }
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
    private void Touched(Vector3 touchLocation)
    {
        WorldController.Instance.ManualClick(touchLocation);
        transform.localScale = Vector3.Lerp(originalScale, targetScale, 1f);
    }
    private void OnMouseUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, 1f);
    }
}
