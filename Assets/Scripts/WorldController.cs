using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Transform platformA;
    public Transform platformB;
    public float moveSpeed = 0.25f;
    public const float TimeWindow = 30f; // 30 seconds

    private float currScore = 0.0f;
    private List<float> inputTimestamps = new List<float>();

    private float CalculateClickRate()
    {
        // Check for mouse clicks, keyboard presses, or screen taps
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            inputTimestamps.Add(Time.time);
        }

        // Remove timestamps older than 30 seconds
        inputTimestamps.RemoveAll(timestamp => Time.time - timestamp > TimeWindow);

        // Return the click rate
        return inputTimestamps.Count / TimeWindow;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float clickRate = CalculateClickRate();
        Debug.Log("Click rate: " + clickRate + " clicks per second");

        Vector2 currPlatformAPosition = platformA.localPosition;
        Vector2 newPlatformAPosition = new Vector2(platformA.localPosition.x - (clickRate * moveSpeed), platformA.localPosition.y);

        Vector2 currPlatformBPosition = platformB.localPosition;
        Vector2 newPlatformBPosition = new Vector2(platformB.localPosition.x - (clickRate * moveSpeed), platformB.localPosition.y);

        platformA.localPosition = Vector2.Lerp(currPlatformAPosition, newPlatformAPosition, Time.deltaTime);
        platformB.localPosition = Vector2.Lerp(currPlatformBPosition, newPlatformBPosition, Time.deltaTime);

        currScore += Mathf.Abs(currPlatformAPosition.x - newPlatformAPosition.x) / 100.0f;
        scoreText.text = "Score: " + Mathf.Round(currScore).ToString();
    }
}
