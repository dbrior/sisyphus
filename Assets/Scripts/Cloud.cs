using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public bool enabled = true;
    public float height;
    public float spawnTimestamp;
    public float lifespan;
    private WorldController controller;

    // Bobbing
    public float amplitude = 0.25f;
    public float frequency = 0.5f;
    private float phase;

    private float minDecay = 0.45f;
    private float maxDecay = 0.01f;
    public float baseSpeed = 0.5f;

    void OnBecameInvisible () {
        if (spawnTimestamp >= Time.time - lifespan) {
            Destroy(gameObject);
        }
    }

    void Start () {
        controller = transform.parent.gameObject.GetComponent<WorldController>();
        phase = Random.Range(0.0f, 2.0f * Mathf.PI);
    }

    void Update () {
        if(enabled) {
            float newXPosition = (transform.localPosition.x - ((controller.delta + baseSpeed) * (((maxDecay - minDecay) * (1-(transform.localScale.x / controller.maxScale))) + minDecay)));
            Vector2 newPosition = new Vector2(
                newXPosition,
                height + (amplitude * Mathf.Sin(frequency * Time.time + phase))
            );
            transform.localPosition = Vector2.Lerp(transform.localPosition, newPosition, Time.deltaTime);
        }
    }
}
