using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public float velocityDecay = 0.95f;  // Decay factor per frame, adjust as necessary
    public float lateralForce = 10.0f;   // Amount of lateral force to apply

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Apply velocity decay
        rb.velocity = new Vector2(rb.velocity.x * velocityDecay, rb.velocity.y * velocityDecay);

        // Check for mouse click and apply lateral force
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPosition = new Vector2(worldPoint.x, worldPoint.y);

            // Calculate direction from object to click position
            Vector2 direction = (clickPosition - rb.position).normalized;

            // Apply lateral force in the direction
            rb.AddForce(direction * lateralForce, ForceMode2D.Impulse);
        }
    }
    float GetvV()
    {
        return rb.velocity.x;
    }
}
