using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SparkleScript : MonoBehaviour
{
    public float minLaunchForce = 10f;    // Minimum force with which to launch the prefab
    public float maxLaunchForce = 11f;   // Maximum force with which to launch the prefab
    public float minAngle = 0f;        // Minimum angle for launch direction
    public float maxAngle = 5f;         // Maximum angle for launch direction
    public bool randomizeColor = true; // Toggle for randomizing color
    public bool alignRotation = true;  // Toggle for aligning rotation with trajectory
    public float rotationOffset = 0.0f;
    public bool setParent = false;
    private Transform parentObject;
    public AudioSource collisionSound;
    private bool destroying = false;

    float GetRandomAngle(float min, float max)
    {
        if (min > max) // handle the wrap-around from 360 back to 0
        {
            float random = Random.value;
            return random > 0.5 ? Random.Range(min, 360) : Random.Range(0, max);
        }
        return Random.Range(min, max);
    }
    void SwitchToSecondaryAnimation()
    {
        SpriteAnimatorUI animator = GetComponent<SpriteAnimatorUI>();
        animator.useSecondaryAnimation = true;
        animator.singleShot = true;
        animator.currentFrame = 0;
    }
    void OnMouseDown()
    {
        if(!destroying)
        {
            destroying = true;
            SwitchToSecondaryAnimation();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D: {gameObject.name} has collided with {collision.gameObject.name}");
        if (!destroying)
        {
            destroying = true;
            GetComponent<Rigidbody2D>().simulated = false;
            SwitchToSecondaryAnimation();
            transform.position = new Vector3(transform.position.x, transform.position.y-0.5f, transform.position.z);
            collisionSound.Play();
            WorldController.Instance.SpawnLostPoints(100);
            WorldController.Instance.points -= 100.0f;
        }
    }

    Vector2 AngleToVector2(float angle)
    {
        // Angle in degrees to radians
        float radian = angle * Mathf.Deg2Rad;
        // Creating a direction vector from angle (assuming up is 0 degrees, right is 90 degrees)
        return new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
    }

    void SetRandomColor()
    {
        if (!randomizeColor) return; // Check if color randomization is enabled

        // Create a random color
        Color randomColor = new Color(Random.value, Random.value, Random.value);

        // Retrieve the SpriteRenderer component and set its color
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = randomColor;
        }

        // Retrieve the Light2D component and set its color
        Light2D light2D = GetComponent<Light2D>();
        if (light2D != null)
        {
            light2D.color = randomColor;
        }
    }

    void Start()
    {
        if (setParent) {
            gameObject.transform.SetParent(GameObject.Find("Platform A").transform);
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float angle = GetRandomAngle(minAngle, maxAngle);
        float force = Random.Range(minLaunchForce, maxLaunchForce);
        Vector2 launchDirection = AngleToVector2(angle);

        // Apply random color
        SetRandomColor();

        // Align object rotation with the launch direction if enabled
        if (alignRotation)
        {
            transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg) + rotationOffset);
        }

        // Apply the force in the calculated direction
        rb.AddForce(launchDirection * force, ForceMode2D.Impulse);
    }

    void Update()
    {
        
    }
}
