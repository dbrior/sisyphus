using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FitRectTransform : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;
    private int prevLength = 1;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start() {}
    void Update()
    {
        if (textMeshPro.text.Length != prevLength)
        {
            Debug.Log("Updaingasa");
            UpdateRectTransform();
            prevLength = textMeshPro.text.Length;
        }
        Debug.Log(rectTransform.sizeDelta.ToString());
    }

    // Call this method whenever the text changes
    public void UpdateRectTransform()
    {
        // Update the TextMeshPro object to ensure accurate size measurements
        textMeshPro.ForceMeshUpdate();

        // Calculate the size needed to fit the text
        Vector2 textSize = new Vector2(textMeshPro.preferredWidth + 10f, textMeshPro.preferredHeight + 10f);

        // Set the size of the RectTransform
        rectTransform.sizeDelta = textSize;
    }
}
