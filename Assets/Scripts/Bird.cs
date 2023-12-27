using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float location;
    public float spawnTimestamp;
    public float lifespan;

    // Bobbing
    private float amplitude = 1.5f;
    private float frequency = 0.75f;
    private float phase;

    // Speed
    private float baseSpeed = 5.0f;

    void OnBecameInvisible () {
        if (spawnTimestamp >= Time.time - lifespan) {
            Destroy(gameObject);
        }
    }

    void Start () {
        phase = Random.Range(0.0f, 2.0f * Mathf.PI);
    }

    void Update () {
        Vector2 newPosition = new Vector2(
            transform.localPosition.x + (amplitude * Mathf.Sin(frequency * Time.time + phase)),
            transform.localPosition.y - baseSpeed
        );
        transform.localPosition = Vector2.Lerp(transform.localPosition, newPosition, Time.deltaTime);
    }
}
