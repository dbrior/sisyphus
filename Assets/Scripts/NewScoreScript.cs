using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewScoreScript : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    private WorldController worldController;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        worldController = GameObject.Find("Main Grid").GetComponent<WorldController>();
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = Mathf.Floor(worldController.points).ToString();
    }
}
