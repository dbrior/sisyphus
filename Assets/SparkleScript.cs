using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SparkleScript : MonoBehaviour
{
    public float minLaunchForce = 3f;    // Minimum force with which to launch the prefab
    public float maxLaunchForce = 10f;   // Maximum force with which to launch the prefab
    public float minAngle = 305f;        // Minimum angle for launch direction
    public float maxAngle = 60f;         // Maximum angle for launch direction
    public float launchForce = 5f;
    public float torqueForce = 1f;

    float GetRandomAngle(float min, float max)
    {
        if (min > max) // handle the wrap-around from 360 back to 0
        {
            float random = Random.value;
            return random > 0.5 ? Random.Range(min, 360) : Random.Range(0, max);
        }
        return Random.Range(min, max);
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
        // Create a random color
        Color randomColor = new Color(Random.value, Random.value, Random.value);

        // Retrieve the SpriteRenderer component and set its color
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = randomColor;
        }

        // // Alternatively, retrieve and set color for an Image component if used instead
        // Image image = GetComponent<Image>();
        // if (image != null)
        // {
        //     image.color = randomColor;
        // }

        // Retrieve the Light2D component and set its color
        Light2D light2D = GetComponent<Light2D>();
        if (light2D != null)
        {
            light2D.color = randomColor;
        }
    }

    void Start()
    {
        // Add a Rigidbody2D component dynamically if not already added
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Calculate a random angle and force
        float angle = GetRandomAngle(minAngle, maxAngle);
        float force = Random.Range(minLaunchForce, maxLaunchForce);

        // Convert angle to radians and then to a direction vector
        Vector2 launchDirection = AngleToVector2(angle);

        // Align object rotation with launch direction
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Apply the force in the calculated direction
        rb.AddForce(launchDirection * force, ForceMode2D.Impulse);

        // Random color
        SetRandomColor();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
