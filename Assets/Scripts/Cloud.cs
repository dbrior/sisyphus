using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float spawnTimestamp;
    public float lifespan;

    void OnBecameInvisible () {
        if (spawnTimestamp >= Time.time - lifespan) {
            Destroy(gameObject);
        }
    }
}
