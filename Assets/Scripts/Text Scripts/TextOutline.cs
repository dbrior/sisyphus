using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextOutline : MonoBehaviour
{
    public float outlineWidth;
    public Color outlineColor;

    void Start()
    {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        text.outlineWidth = outlineWidth;
        text.outlineColor = outlineColor;
    }

    void Update()
    {
        
    }
}
