using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceUI : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textUI.text = WorldController.Instance.maxScore.ToString("F0");
    }
}
