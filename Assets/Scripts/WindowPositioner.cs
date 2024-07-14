using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowPositioner : MonoBehaviour
{
    public Transform openAnchor;
    public Transform closedAnchor;
    public float moveSpeed = 100f;
    private bool open;

    public void ToggleWindow() {
        open = !open;

        if (!open) {
            Time.timeScale = 1f;
        } else {
            Time.timeScale = 0f;
        }
    }

    void Start()
    {
        // why as as as das as dasd asd as
        open = false;
        transform.position = closedAnchor.position;   
    }

    void Update()
    {
        if (open) {
            transform.position = Vector3.MoveTowards(transform.position, openAnchor.position, moveSpeed * Time.unscaledDeltaTime);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, closedAnchor.position, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}
