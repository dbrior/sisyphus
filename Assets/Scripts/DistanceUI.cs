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
        if (WorldController.Instance.maxScore > 100f) {
            textUI.text = WorldController.Instance.maxScore.ToString("F0");
        } else if (WorldController.Instance.maxScore > 10f) {
            textUI.text = WorldController.Instance.maxScore.ToString("F1");
        } else {
            textUI.text = WorldController.Instance.maxScore.ToString("F2");
        }
    }
}
