using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldController : MonoBehaviour
{
    [Header("Sprites:")]
    public GameObject pushingSprite;
    public GameObject idleSprite;
    public Animator boulderAnimator;
    [Header("Animation Speeds:")]
    public float maxAnimationSpeed = 2f;
    public float minAnimationSpeed = 0.25f;
    public float maxClickRateTarget = 30.0f;
    [Header("UI:")]
    public TextMeshProUGUI scoreText;
    [Header("Platforms:")]
    public Transform platformA;
    public Transform platformB;
    [Header("Terrain Angle:")]
    public float maxTerrainScoreTarget = 10000.0f;
    public float maxTerrainAngle = 70.0f;
    public float minTerrainAngle = 10.0f;
    [Header("Click-Rate/Movement:")]
    public float moveSpeed = 0.25f;
    public float TimeWindow = 2f; // 2 seconds
    [Header("Cloud Spawning:")]
    public List<GameObject> cloudPrefabs;
    private List<Cloud> spawnedClouds = new List<Cloud>();
    public float minScale, maxScale;
    public float minHeight, maxHeight;
    public float spawnX;
    public float spawnInterval;
    public float deltaForSpawn = 9.0f;
    public float lifespan = 9.0f;
    public float absoluteRandomDeltaRange = 6.0f;

    private float currAccumulatedDelta = 0.0f;
    private float nextSpawnTime;
    private float currScore = 0.0f;
    private List<float> inputTimestamps = new List<float>();
    private SpriteRenderer pushingSpriteRenderer;
    private Animator pushingSpriteAnimator;
    private SpriteRenderer idleSpriteRenderer;

    // Setting terrain angle
    private float GetTerrainAngle(float score) {
        if (score > maxTerrainScoreTarget) {
            return maxTerrainAngle;
        } else {
            return ((score/maxTerrainScoreTarget)*(maxTerrainAngle - minTerrainAngle)) + minTerrainAngle;
        }
    }
    private void SetTerrainAngle(float currScore) {
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, GetTerrainAngle(currScore));
    }

    // Cloud spawning
    private bool ShouldSpawnCloud()
    {
        if (currAccumulatedDelta >= deltaForSpawn + Random.Range(-absoluteRandomDeltaRange, absoluteRandomDeltaRange)) {
            currAccumulatedDelta = 0.0f;
            return true;
        }
        return false;
    }
    void SpawnCloud()
    {
        // TODO: Track spawned clouds in list for cleanup
        // TODO: Have clouds operate on independent x position, for parallax
        GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];
        float scale = Random.Range(minScale, maxScale);
        float height = Random.Range(minHeight, maxHeight);

        Cloud cloud = Instantiate(cloudPrefab).GetComponent<Cloud>();
        cloud.spawnTimestamp = Time.time;
        cloud.lifespan = lifespan;
        cloud.transform.SetParent(platformA.position.x > platformB.position.x ? platformA : platformB, false);
        cloud.transform.localScale = new Vector3(scale, scale, 1);
        cloud.transform.eulerAngles = new Vector3(0,0,0);
        cloud.transform.position = new Vector3(spawnX, height);
    }

    // Getting click rate
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

    // Setting animation speeds
    private float GetAnimationSpeed(float clickRate) {
        if (clickRate > maxClickRateTarget) {
            return maxAnimationSpeed;
        } else {
            return ((clickRate/maxClickRateTarget)*(maxAnimationSpeed - minAnimationSpeed)) + minAnimationSpeed;
        }
    }
    private void SetSpriteAnimationSpeed(float clickRate) {
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
    }

    // Moving platforms & adjust score accordingly
    private float MovePlatforms(float clickRate) {
        Vector2 currPlatformAPosition = platformA.localPosition;
        Vector2 newPlatformAPosition = new Vector2(platformA.localPosition.x - (clickRate * moveSpeed), platformA.localPosition.y);

        Vector2 currPlatformBPosition = platformB.localPosition;
        Vector2 newPlatformBPosition = new Vector2(platformB.localPosition.x - (clickRate * moveSpeed), platformB.localPosition.y);

        platformA.localPosition = Vector2.Lerp(currPlatformAPosition, newPlatformAPosition, Time.deltaTime);
        platformB.localPosition = Vector2.Lerp(currPlatformBPosition, newPlatformBPosition, Time.deltaTime);

        float delta = Mathf.Abs(currPlatformAPosition.x - newPlatformAPosition.x);

        return delta;
    }

    void UpdateScore(float distanceDelta) {
        currScore += distanceDelta / 100.0f;
        currAccumulatedDelta += distanceDelta / 100.0f;
        scoreText.text = "Score: " + Mathf.Round(currScore).ToString();
    }

    void UpdateDistanceAndScore(float clickRate) {
        float distanceDelta = MovePlatforms(clickRate);
        UpdateScore(distanceDelta);
    }
    
    void Start()
    {
        pushingSpriteRenderer = pushingSprite.GetComponent<SpriteRenderer>();
        pushingSpriteAnimator = pushingSprite.GetComponent<Animator>(); 

        idleSpriteRenderer = idleSprite.GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        // The core function calls set the foundation that controls the game
        float clickRate = CalculateClickRate();                                 // clickRate determines the speed of the game
        Debug.Log("Click rate: " + clickRate + " clicks per second");               
        UpdateDistanceAndScore(clickRate);                                      // current distance from start yields score (1:1)

        // Below rely on either clickRate or score
        SetSpriteAnimationSpeed(clickRate);             // some animations adapt to the speed of the game
        SetTerrainAngle(currScore);                     // Make sure this occurs after and score adjustments

        // TODO: These functions have todo's
        if(ShouldSpawnCloud()) {
            SpawnCloud();
        }
    }
}
