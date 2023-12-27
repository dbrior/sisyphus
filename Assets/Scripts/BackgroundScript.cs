using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private WorldController controller;

    void Start()
    {
        controller = transform.parent.GetComponent<WorldController>();
    }

    void FixedUpdate()
    {
        Vector2 newPosition = new Vector2(transform.localPosition.x + controller.delta * 0.01f, transform.localPosition.y);
        transform.localPosition = Vector2.Lerp(transform.localPosition, newPosition, Time.deltaTime);
    }
}
