using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsUI : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textUI.text = WorldController.Instance.points.ToString("F0");
    }
}
