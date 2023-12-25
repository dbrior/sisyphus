using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float spawnTimestamp;
    public float lifespan;
    private WorldController controller;

    void OnBecameInvisible () {
        if (spawnTimestamp >= Time.time - lifespan) {
            Destroy(gameObject);
        }
    }

    void Start () {
        controller = transform.parent.gameObject.GetComponent<WorldController>();
    }

    void Update () {
        float newXPosition = (transform.localPosition.x - (controller.delta * Mathf.Max(0.05f, (transform.localScale.x / controller.maxScale))));
        Vector2 newPosition = new Vector2(
            newXPosition,
            transform.localPosition.y
        );
        transform.localPosition = Vector2.Lerp(transform.localPosition, newPosition, Time.deltaTime);
    }
}
