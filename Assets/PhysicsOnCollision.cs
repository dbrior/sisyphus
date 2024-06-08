using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsOnCollision : MonoBehaviour
{
    private bool triggered = false;
    public List<Rigidbody2D> rbs;
    public List<Rigidbody2D> disableList;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered) {
            triggered = true;
            for (int i = 0; i<rbs.Count; i++) {
                rbs[i].constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}
