using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float spawnTimestamp;
    public float lifespan;
    private WorldController controller;

    private float minDecay = 0.45f;
    private float maxDecay = 0.01f;
    private float bobbleRange = 2f;

    void OnBecameInvisible () {
        if (spawnTimestamp >= Time.time - lifespan) {
            Destroy(gameObject);
        }
    }

    void Start () {
        controller = transform.parent.gameObject.GetComponent<WorldController>();
    }

    void Update () {
        float newXPosition = (transform.localPosition.x - (controller.delta * Mathf.Min(minDecay, Mathf.Max(maxDecay, (transform.localScale.x / controller.maxScale)))));
        Vector2 newPosition = new Vector2(
            newXPosition,
            transform.localPosition.y + Random.Range(-bobbleRange, bobbleRange)
        );
        transform.localPosition = Vector2.Lerp(transform.localPosition, newPosition, Time.deltaTime/2);
    }
}
