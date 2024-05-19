using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUIElement : MonoBehaviour
{
    public Transform target;
    public Vector3 posOffest;

    void Update()
    {
        Vector3 newLocation = Camera.main.ScreenToWorldPoint(target.position + Vector3.up * 2) + posOffest;
        newLocation.z = 0;
        transform.position = newLocation;
    }
}
