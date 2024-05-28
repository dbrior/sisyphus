using UnityEngine;

public class WorldLockedPatternController : MonoBehaviour
{
    public Material material;  // Assign the material in the Inspector
    private Vector3 worldPosition;  // The world position to lock the pattern to

    void Update()
    {
        // Set the _WorldPosition parameter in the shader to the world position vector
        material.SetVector("_WorldPosition", transform.position / 100f);
    }
}
