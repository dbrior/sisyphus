using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour
{
    public float speed;
    public float rollback_speed;
    public ScoreScript score_tracker;

    private Rigidbody2D rb;
    private Vector2 raycast_direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        raycast_direction = Vector2.down;
    }

    Vector2 GetSurfaceNormal() {
        int layerMask = ~LayerMask.GetMask("IgnoreSurfaceNormal");

        // Debug.DrawRay(transform.position, Vector3.down * 100, Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycast_direction, 100, layerMask);

        // Does the ray intersect any objects excluding the player layer
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            // Debug.DrawRay(transform.position, Vector3.down * 100, Color.red);
            return hit.normal;
        } else {
            return Vector2.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 20, Color.green);

        float push1 = Input.GetAxis("Push1");
        float push2 = Input.GetAxis("Push2");
        float push3 = Input.GetAxis("Push3");
        float push4 = Input.GetAxis("Push4");
        float push5 = Input.GetAxis("Push5");

        float push_amount = Input.touchCount + push1 + push2 + push3 + push4 + push5;
        // Debug.Log("Push amount: " + push_amount.ToString());

        Vector2 surfaceNormal = GetSurfaceNormal();
        raycast_direction = surfaceNormal * -1;
        Debug.DrawRay(transform.position, surfaceNormal*-100, Color.red);
        rb.AddForce(surfaceNormal*-10);

        float surfaceAngle = Vector2.Angle(Vector2.up, surfaceNormal);
        Debug.Log("Surface Angle: " + surfaceAngle.ToString());

        if (push_amount > 0) {
            rb.angularVelocity = speed * push_amount * -1; // Invert to go in correct direction
        } else {
            rb.angularVelocity = rollback_speed * surfaceAngle;
        }

    }
}
