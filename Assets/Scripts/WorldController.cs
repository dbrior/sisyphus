using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldController : MonoBehaviour
{
    public GameObject pushingSprite;
    public GameObject idleSprite;
    public Animator boulderAnimator;
    public float maxAnimationSpeed = 2f;
    public float minAnimationSpeed = 0.25f;
    public float maxClickRateTarget = 30.0f;
    public TextMeshProUGUI scoreText;
    public Transform platformA;
    public Transform platformB;
    public float maxTerrainScoreTarget = 10000.0f;
    public float maxTerrainAngle = 70.0f;
    public float minTerrainAngle = 10.0f;
    public float moveSpeed = 0.25f;
    public float TimeWindow = 2f; // 2 seconds

    private float currScore = 0.0f;
    private List<float> inputTimestamps = new List<float>();
    private SpriteRenderer pushingSpriteRenderer;
    private Animator pushingSpriteAnimator;
    private SpriteRenderer idleSpriteRenderer;

    private float GetTerrainAngle(float score) {
        if (score > maxTerrainScoreTarget) {
            return maxTerrainAngle;
        } else {
            return ((score/maxTerrainScoreTarget)*(maxTerrainAngle - minTerrainAngle)) + minTerrainAngle;
        }
    }

    private float GetAnimationSpeed(float clickRate) {
        if (clickRate > maxClickRateTarget) {
            return maxAnimationSpeed;
        } else {
            return ((clickRate/maxClickRateTarget)*(maxAnimationSpeed - minAnimationSpeed)) + minAnimationSpeed;
        }
    }

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
        pushingSpriteRenderer = pushingSprite.GetComponent<SpriteRenderer>();
        pushingSpriteAnimator = pushingSprite.GetComponent<Animator>(); 

        idleSpriteRenderer = idleSprite.GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float clickRate = CalculateClickRate();
        Debug.Log("Click rate: " + clickRate + " clicks per second");

        float animationSpeed = GetAnimationSpeed(clickRate);

        if (clickRate > 0) {
            pushingSpriteRenderer.sortingOrder = 5;
            pushingSpriteAnimator.speed = animationSpeed;
            idleSpriteRenderer.sortingOrder = -1;
            boulderAnimator.speed = animationSpeed;
        } else {
            idleSpriteRenderer.sortingOrder = 5;
            pushingSpriteRenderer.sortingOrder = -1;
            boulderAnimator.speed = 0;
            boulderAnimator.playbackTime = 0;
        }

        Vector2 currPlatformAPosition = platformA.localPosition;
        Vector2 newPlatformAPosition = new Vector2(platformA.localPosition.x - (clickRate * moveSpeed), platformA.localPosition.y);

        Vector2 currPlatformBPosition = platformB.localPosition;
        Vector2 newPlatformBPosition = new Vector2(platformB.localPosition.x - (clickRate * moveSpeed), platformB.localPosition.y);

        platformA.localPosition = Vector2.Lerp(currPlatformAPosition, newPlatformAPosition, Time.deltaTime);
        platformB.localPosition = Vector2.Lerp(currPlatformBPosition, newPlatformBPosition, Time.deltaTime);

        currScore += Mathf.Abs(currPlatformAPosition.x - newPlatformAPosition.x) / 100.0f;
        scoreText.text = "Score: " + Mathf.Round(currScore).ToString();

        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, GetTerrainAngle(currScore));
    }
}
