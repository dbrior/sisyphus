using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointAddition : MonoBehaviour
{
    public Vector2 initialForce;
    public Color textColor = new Color(1f, 1f, 1f);
    private TextMeshPro tmp;
    private Rigidbody2D rb;
    public string text;
    public float fontSize = 1f;
    public float fadeDuration = 1f;
    private FadeOutTextMeshPro fade;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tmp = GetComponent<TextMeshPro>();
        fade = GetComponent<FadeOutTextMeshPro>();

        fade.fadeDuration = fadeDuration;

        Debug.Log(textColor);

        rb.AddForce(initialForce);
        tmp.text = text;
        tmp.color = textColor;
        tmp.fontSize = fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
