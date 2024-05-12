using UnityEngine;
using System.Collections;
using TMPro;

public class PointSubtraction : MonoBehaviour
{
    public float moveSpeed;
    public float lifespan;
    public int numFlashes;
    public float flashDuration;
    public float flashDelta;
    public Color flashColor;
    private Color baseColor;
    private TextMeshPro textMesh;
    private float currDuration = 0;
    private bool flashing = false;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            baseColor = textMesh.color;  // Store the original color
        }
    }

    void Update() // Changed from FixedUpdate to Update
    {
        transform.position = Vector3.MoveTowards(transform.position, WorldController.Instance.lostPointsTargetTransform.position, moveSpeed * Time.deltaTime);
        currDuration += Time.deltaTime;

        if (!flashing && currDuration >= lifespan - ((flashDuration + flashDelta) * numFlashes))
        {
            Debug.Log("Flashing");
            flashing = true;
            StartCoroutine(FlashRoutine());
        }

        if (currDuration > lifespan) Destroy(gameObject);
    }

    IEnumerator FlashRoutine()
    {
        int flashesLeft = numFlashes;
        while (flashesLeft > 0)
        {
            textMesh.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            textMesh.color = baseColor;
            yield return new WaitForSeconds(flashDelta);
            flashesLeft--;
        }
    }
}
