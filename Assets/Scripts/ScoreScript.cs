using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public Transform boulder;
    public TextMeshProUGUI tmp;

    public float score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0.0f;
        // tmp = gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.Round(boulder.position.x);
        tmp.text = "Score: " + score.ToString();
    }
}
