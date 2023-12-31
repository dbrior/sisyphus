using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldController : MonoBehaviour
{
    [Header("Sprites:")]
    public GameObject background;
    public GameObject pushingSprite;
    public GameObject idleSprite;
    public Animator boulderAnimator;
    [Header("Animation Speeds:")]
    public float maxAnimationSpeed = 2f;
    public float minAnimationSpeed = 0.25f;
    public float maxClickRateTarget = 30.0f;
    [Header("UI:")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI pointsText;
    public GameObject shopUI;
    [Header("Platforms:")]
    public float delta = 0.0f;
    public Transform platformA;
    public Transform platformB;
    [Header("Terrain Angle:")]
    public float maxTerrainScoreTarget = 10000.0f;
    public float maxTerrainAngle = 70.0f;
    public float minTerrainAngle = 10.0f;
    [Header("Click-Rate/Movement:")]
    public float maxClickRate = 20.0f;
    public float deltaScoreRatio = 100.0f;
    public float moveSpeed = 0.25f;
    public float backgroundMoveFactor = 0.01f;
    public float minRollbackSpeed = 0.1f;
    public float maxRollbackSpeed = 3.0f;
    public float TimeWindow = 2f; // 2 seconds
    [Header("Bird Spawning:")]
    public List<GameObject> birdPrefabs;
    public float minX, maxX;
    [Header("Cloud Spawning:")]
    public List<GameObject> cloudPrefabs;
    public float minScale, maxScale;
    public float minHeight, maxHeight;
    public float spawnX;
    public float spawnInterval;
    public float deltaForSpawn = 9.0f;
    public float lifespan = 30.0f;
    public float absoluteRandomDeltaRange = 6.0f;

    private float maxScore = 0.0f;
    private float clickRate = 0.0f;
    private float currAccumulatedDelta = 0.0f;
    private float nextSpawnTime;
    private float currScore = 0.0f;
    private float terrainAngle;
    private List<float> inputTimestamps = new List<float>();
    private SpriteRenderer pushingSpriteRenderer;
    private Animator pushingSpriteAnimator;
    private SpriteRenderer idleSpriteRenderer;

    // Progression
    public float points = 0.0f;
    private float sisMaxTerrainAngle = 8.0f;
    private float baseClickRate = 0.0f;

    // Sisyphus skills
    public void increaseSisMaxTerrainAngle() {
        float cost = 10.0f;
        if (points >= cost) {
            points -= cost;
            sisMaxTerrainAngle += 10.0f;
            Debug.Log("Max Angle Increase!");
        }
    }
    public void increaseBaseClickRate() {
        float cost = 10.0f;
        if (points >= cost) {
            points -= cost;
            baseClickRate += 2.0f;
            Debug.Log("Base Click Rate Increase!");
        }
    }

    // UI Toggle
    public void ToggleShopUI() {
        shopUI.SetActive(!shopUI.activeSelf);
    }

    // Setting terrain angle
    private float GetTerrainAngle(float score) {
        if (score > maxTerrainScoreTarget) {
            return maxTerrainAngle;
        } else {
            return ((score/maxTerrainScoreTarget)*(maxTerrainAngle - minTerrainAngle)) + minTerrainAngle;
        }
    }
    private void SetTerrainAngle(float currScore) {
        terrainAngle = GetTerrainAngle(currScore);
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, terrainAngle);
    }

    // Birds
    private bool ShouldSpawnBird()
    {
        if (currScore > 2 && Mathf.Round(currScore) % 10 == 0) {
            return true;
        }
        return false;
    }
    void SpawnBird()
    {
        // TODO: Track spawned clouds in list for cleanup
        // TODO: Have clouds operate on independent x position, for parallax
        GameObject birdPrefab = birdPrefabs[Random.Range(0, birdPrefabs.Count)];

        Bird bird = Instantiate(birdPrefab).GetComponent<Bird>();
        bird.spawnTimestamp = Time.time;
        bird.lifespan = lifespan;
        bird.transform.position = new Vector2(1, 4);
        bird.transform.SetParent(platformA);
        bird.transform.eulerAngles = new Vector3(0,0,-90);
    }

    // Clouds
    private bool ShouldSpawnCloud()
    {
        if (Mathf.Abs(currAccumulatedDelta) >= deltaForSpawn + Random.Range(-absoluteRandomDeltaRange, absoluteRandomDeltaRange)) {
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
        cloud.height = height;
        cloud.spawnTimestamp = Time.time;
        cloud.lifespan = lifespan;
        cloud.transform.SetParent(transform);
        cloud.transform.localScale = new Vector3(scale, scale, 1);
        cloud.transform.eulerAngles = new Vector3(0,0,0);
        cloud.transform.localPosition = new Vector3(spawnX * Mathf.Sign(delta), height);
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
        Vector2 currPlatformBPosition = platformB.localPosition;

        Vector2 newPlatformAPosition;
        Vector2 newPlatformBPosition;

        float currTerrainSkillModifier = (1 - (terrainAngle / sisMaxTerrainAngle));
        float movement = (clickRate * moveSpeed) * currTerrainSkillModifier;
        Debug.Log(currTerrainSkillModifier);

        bool shouldLerp = true;
        if (clickRate > 0) {
            newPlatformAPosition = new Vector2(platformA.localPosition.x - movement, platformA.localPosition.y);
            newPlatformBPosition = new Vector2(platformB.localPosition.x - movement, platformB.localPosition.y);
        } else if (clickRate == 0 && Mathf.Round(currScore) > 0) {
            float rollbackAmount = ((GetTerrainAngle(currScore)/maxTerrainAngle)*(maxRollbackSpeed - minRollbackSpeed)) + minRollbackSpeed;
            newPlatformAPosition = new Vector2(platformA.localPosition.x + rollbackAmount, platformA.localPosition.y);
            newPlatformBPosition = new Vector2(platformB.localPosition.x + rollbackAmount, platformB.localPosition.y);
        } else {
            shouldLerp = false;
            newPlatformAPosition = platformA.localPosition;
            newPlatformBPosition = platformB.localPosition;
        }

        if (shouldLerp) {
            platformA.localPosition = Vector2.Lerp(currPlatformAPosition, newPlatformAPosition, Time.deltaTime);
            platformB.localPosition = Vector2.Lerp(currPlatformBPosition, newPlatformBPosition, Time.deltaTime);
        }

        delta = currPlatformAPosition.x - newPlatformAPosition.x;

        return delta;
    }
    private void MoveBackground(float distanceDelta) {
        Vector2 newBackgroundPosition = new Vector2(background.transform.localPosition.x - (distanceDelta * backgroundMoveFactor), background.transform.localPosition.y);
        background.transform.localPosition = Vector2.Lerp(background.transform.localPosition, newBackgroundPosition, Time.deltaTime);
        background.transform.eulerAngles = new Vector3(0,0,0);
    }

    void UpdateScore(float distanceDelta) {
        currScore += distanceDelta / deltaScoreRatio;
        currAccumulatedDelta += distanceDelta / deltaScoreRatio;
        scoreText.text = Mathf.Floor(currScore).ToString();
        if (currScore > maxScore) {
            PlayerPrefs.SetFloat("Max Score", currScore);
            PlayerPrefs.Save();
            points += currScore - maxScore;
            maxScore = currScore;
        }
        maxScoreText.text = Mathf.Floor(maxScore).ToString();
    }

    float UpdateDistanceAndScore(float clickRate) {
        float distanceDelta = MovePlatforms(clickRate);
        MoveBackground(distanceDelta);
        UpdateScore(distanceDelta);
        return distanceDelta;
    }
    
    void Start()
    {
        maxScore = PlayerPrefs.GetFloat("Max Score", 0.0f);
        maxScore = 0.0f;
        pushingSpriteRenderer = pushingSprite.GetComponent<SpriteRenderer>();
        pushingSpriteAnimator = pushingSprite.GetComponent<Animator>(); 

        idleSpriteRenderer = idleSprite.GetComponent<SpriteRenderer>(); 
    }

    void Update() 
    {
        clickRate = Mathf.Min(maxClickRate, CalculateClickRate()) + baseClickRate;     // clickRate determines the speed of the game
        Debug.Log("Click rate: " + clickRate + " clicks per second");               
    }

    void FixedUpdate()
    {
        float distanceDelta = UpdateDistanceAndScore(clickRate);    // current distance from start yields score (1:1)

        // Below rely on either clickRate or score
        SetSpriteAnimationSpeed(clickRate);             // Some animations adapt to the speed of the game
        SetTerrainAngle(currScore);                     // Steepness changes accoding to score

        // TODO: These functions have todo's
        if(ShouldSpawnCloud()) {
            SpawnCloud();
        }
        // if(ShouldSpawnBird()) {
        //     SpawnBird();
        // }
        pointsText.text = Mathf.Floor(points).ToString();
        pointsText.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
