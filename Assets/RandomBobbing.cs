using UnityEngine;

public class RandomBobbing : MonoBehaviour
{
    public float radius = 5.0f;
    public float speed = 0.5f;
    public float smoothTime = 0.3f;
    public Vector3 centerPosition;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    private float timeOffset;
    private bool startBobbing = false;
    private float lastUpdateTime = 0;
    private float updateInterval = 0.1f; // Update target position every 0.1 seconds

    void Start()
    {
        timeOffset = Random.Range(0f, 10000f);
    }

    void FixedUpdate()
    {
        if (startBobbing)
        {
            if (Time.time - lastUpdateTime > updateInterval)
            {
                lastUpdateTime = Time.time;
                float newX = Mathf.PerlinNoise(timeOffset + Time.time * speed, 0f) * 2 - 1;
                float newY = Mathf.PerlinNoise(0f, timeOffset + Time.time * speed) * 2 - 1;
                float newZ = Mathf.PerlinNoise(timeOffset + Time.time * speed, timeOffset + Time.time * speed) * 2 - 1;

                targetPosition = new Vector3(newX, newY, newZ) * radius + centerPosition;
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    void OnCentered()
    {
        startBobbing = true;
    }
}
