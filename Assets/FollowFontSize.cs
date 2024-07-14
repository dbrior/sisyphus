using UnityEngine;
using TMPro;

public class FollowFontSize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetTmp;
    private TextMeshProUGUI tmp;
    private float currFontSize;

    void UpdateFontSize() {
        tmp.fontSize = targetTmp.fontSize;
    }

    void Awake() {
        tmp = GetComponent<TextMeshProUGUI>();
        currFontSize = tmp.fontSize;
    }

    void Update() {
        if (currFontSize != targetTmp.fontSize) UpdateFontSize();
    }
}
