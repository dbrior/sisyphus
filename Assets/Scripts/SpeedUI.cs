using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedUI : MonoBehaviour
{
    private float displayedSpeed = 0f;
    private TextMeshProUGUI textUI;
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textUI.text = WorldController.Instance.smoothedSpeed.ToString("F1");
    }
}
