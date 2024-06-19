using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float multiplier;
    public float teleportX;
    public Transform partner;
    void Update()
    {
        if (!WorldController.Instance.frozen) {
            float delta = WorldController.Instance.delta;
            transform.localPosition = new Vector2(
                transform.localPosition.x - ((delta * multiplier) * 0.01f), 
                transform.localPosition.y
            );   
            if (transform.position.x <= teleportX) {
                if (partner) {
                    Debug.Log("Tele");
                    Debug.Log(partner.position);
                    Debug.Log(transform.position);
                }
                transform.position = new Vector2((transform.position.x-teleportX) + (-1*teleportX), transform.position.y);
                Debug.Log(transform.position);
            }
        }
    }
}
