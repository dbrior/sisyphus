using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStreak : MonoBehaviour
{
    public Transform comboBarTransform;

    void Update()
    {
        Vector3 newLocation = Camera.main.ScreenToWorldPoint(comboBarTransform.position + Vector3.up * 2);
        newLocation.z = 0;
        transform.position = newLocation;
    }
}
