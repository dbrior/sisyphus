using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float push1 = Input.GetAxis("Push1");
        float push2 = Input.GetAxis("Push2");
        float push3 = Input.GetAxis("Push3");
        float push4 = Input.GetAxis("Push4");
        float push5 = Input.GetAxis("Push5");

        float push_amount = push1 + push2 + push3 + push4 + push5;
        Debug.Log("Push amount: " + push_amount.ToString());

        rb.angularVelocity = speed * push_amount * -1; // Invert to go in correct direction
    }
}
