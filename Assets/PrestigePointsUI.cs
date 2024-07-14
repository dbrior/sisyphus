using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrestigePointsUI : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public void UpdateUI() {
        tmp.text = WorldController.Instance.GetPrestigePoints().ToString("F0");
    }

    void Start() {
        tmp = GetComponent<TextMeshProUGUI>();
        UpdateUI();
    }
}
