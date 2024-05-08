using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewScoreScript : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = Mathf.Floor(WorldController.points).ToString();
    }
}
