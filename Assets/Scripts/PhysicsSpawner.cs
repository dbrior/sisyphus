using UnityEngine;

public class ObjectSpawner2D : MonoBehaviour
{
    public GameObject prefab; // Assign this in the Unity editor with your prefab
    public float launchForce = 5f;
    public float torqueForce = 1f;

    void Start()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        // Instantiate the prefab
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);

        // Add a Rigidbody2D component dynamically if not already added
        Rigidbody2D rb = instance.AddComponent<Rigidbody2D>();

        // Apply a force in the upward direction multiplied by the launch force
        rb.AddForce(transform.up * launchForce, ForceMode2D.Impulse);

        // Apply a rotational torque
        rb.AddTorque(torqueForce, ForceMode2D.Impulse);
    }
}
