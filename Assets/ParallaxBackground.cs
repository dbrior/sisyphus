using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float multiplier;
    void Update()
    {
        float delta = WorldController.Instance.delta;
        transform.localPosition = new Vector2(
            transform.localPosition.x - ((delta * multiplier) * 0.01f), 
            transform.localPosition.y
        );   
    }
}
