using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedUI : MonoBehaviour
{
    public float smoothTime = 0.5f;
    private float displayedSpeed = 0f;
    private TextMeshProUGUI textUI;
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        displayedSpeed = Mathf.Lerp(displayedSpeed, WorldController.Instance.speed, Time.deltaTime / smoothTime);
        textUI.text = displayedSpeed.ToString("F1");
    }
}
