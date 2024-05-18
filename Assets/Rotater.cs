using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotationSpeed;
    public Vector3 baseScale = Vector3.one;
    public Vector3 scaleAmplitude = new Vector3(0.5f, 0.5f, 0.5f);
    public float frequency = 1f; 
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        transform.localScale = baseScale + scaleAmplitude * Mathf.Sin(Time.time * frequency);
    }
}
