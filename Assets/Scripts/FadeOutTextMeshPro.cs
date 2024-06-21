using UnityEngine;
using TMPro; // Namespace for Text Mesh Pro

public class FadeOutTextMeshPro : MonoBehaviour
{
    public float fadeDuration = 0.1f; // Duration in seconds over which the fade out occurs
    private TextMeshPro textMesh; // Use TextMeshProUGUI if it's a UI element
    private float startAlpha;
    private float timer;

    void Start()
    {
        // Get the TextMeshPro component
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro component not found on the object!");
            return;
        }

        // Initialize variables
        startAlpha = textMesh.color.a;
        timer = 0;
    }

    void Update()
    {
        if (textMesh != null && timer < fadeDuration)
        {
            timer += Time.deltaTime; // Increment timer by the elapsed time since last frame
            float alpha = Mathf.Lerp(startAlpha, 0, timer / fadeDuration); // Calculate the new alpha value
            Color newColor = textMesh.color;
            newColor.a = alpha; // Set the new alpha
            textMesh.color = newColor; // Apply the new color to the text

            if (alpha <= 0)
            {
                // Optionally deactivate the GameObject once fully transparent
                gameObject.SetActive(false);
            }
        }
    }
}
