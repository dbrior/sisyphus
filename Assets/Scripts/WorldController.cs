using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Rendering.Universal;

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
    public TextMeshProUGUI shopButtonText;
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
    // Public
    public List<GameObject> cloudPrefabs;
    public float minScale, maxScale;
    public float minHeight, maxHeight;
    public const float spawnX = 3.5f;
    public float spawnInterval = 6.0f;
    public float deltaForSpawn = 9.0f;
    public float lifespan = 30.0f;
    public float absoluteRandomDeltaRange = 6.0f;
    // Private
    private float lastSpawnTime = 0.0f;

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
    private int angleUpgradeCount = 0;
    public GameObject angleUpgradeContainer;
    public TextMeshProUGUI angleUpgradeText;
    private int autoUpgradeCount = 0;
    public GameObject autoUpgradeContainer;
    public TextMeshProUGUI autoUpgradeText;
    public TextMeshProUGUI autoStrengthUpgradeText;
    public TextMeshProUGUI autoClickUpgradeText;
    public float points = 0.0f;
    private float sisMaxTerrainAngle = 8.0f;
    private float baseClickRate = 0.0f;
    public AudioSource purchaseSound;
    public SpriteAnimatorUI autoClickAnim;
    public SpriteAnimatorUI angleAnim;
    public Light2D light;
    public GameObject lightWheel;
    public float timeFactor = 0.005f;
    

    // Sisyphus skills
    public void increaseSisMaxTerrainAngle() {
        float cost = angleUpgradeCount * 10 + 10;
        if (points >= cost) {
            points -= cost;
            sisMaxTerrainAngle += 10.0f;
            angleUpgradeCount += 1;
            Debug.Log("Max Angle Increase!");
            purchaseSound.Play();
            angleAnim.frameRate = angleAnim.frameRate - ((angleUpgradeCount * 0.01f * angleAnim.frameRate) * angleAnim.frameRate);
            UpdateStrengthUpgrade(cost + 10);
        }
        angleUpgradeText.text = angleUpgradeCount.ToString();
        if (angleUpgradeCount > 0) {
            angleUpgradeContainer.SetActive(true);
        }
    }
    public void increaseBaseClickRate() {
        float cost = autoUpgradeCount * 10 + 10;
        if (points >= cost) {
            points -= cost;
            baseClickRate += 2.0f;
            autoUpgradeCount += 1;
            Debug.Log("Base Click Rate Increase!");
            purchaseSound.Play();
            autoClickAnim.frameRate = autoClickAnim.frameRate - ((autoUpgradeCount * 0.01f * autoClickAnim.frameRate) * autoClickAnim.frameRate);
            UpdateClickUpgrade(cost + 10);
        }
        autoUpgradeText.text = autoUpgradeCount.ToString();
        if (autoUpgradeCount > 0) {
            autoUpgradeContainer.SetActive(true);
        }
    }

    // UI Toggle
    public void ToggleShopUI() {
        shopUI.SetActive(!shopUI.activeSelf);
        shopButtonText.text = shopUI.activeSelf ? "Close" : "Shop";
    }

    public void UpdateStrengthUpgrade(float cost){
        string _cost = "COST: " + cost.ToString();
        autoStrengthUpgradeText.text = _cost;
    }
    public void UpdateClickUpgrade(float cost){
        string _cost = "COST: " + cost.ToString();
        autoClickUpgradeText.text = _cost;
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

    private string numberFormatter(float points) {
        string formattedNumber;
        points = Mathf.Floor(points);
        if (points >= 1000000)
        {
            formattedNumber = (points / 1000).ToString("0.#") + "m";
        }
        else if (points >= 1000)
        {
            formattedNumber = (points / 1000).ToString("0.#") + "k";
        }
        else
        {
            formattedNumber = points.ToString();
        }
        return formattedNumber;
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
        bool timeCondition = Time.time - lastSpawnTime >= (spawnInterval + Random.Range(-1.0f,1.0f));
        bool distanceCondition = Mathf.Abs(currAccumulatedDelta) >= deltaForSpawn + Random.Range(-absoluteRandomDeltaRange, absoluteRandomDeltaRange);
        if (timeCondition) {
            lastSpawnTime = Time.time;
            return true;
        }
        if (distanceCondition) {
            currAccumulatedDelta = 0.0f;
            return true;
        }
        return false;
    }
    void SpawnCloud(float spawnLocation = spawnX)
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
        cloud.transform.localPosition = new Vector3(spawnLocation * Mathf.Sign(delta), height);
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

    private void RotateLight(float distanceDelta) {
        lightWheel.transform.eulerAngles = new Vector3(lightWheel.transform.eulerAngles.x, lightWheel.transform.eulerAngles.y, lightWheel.transform.eulerAngles.z - (distanceDelta * timeFactor));   
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
        // currScore = 1000.0f;
        maxScore = PlayerPrefs.GetFloat("Max Score", 0.0f);
        maxScore = 0.0f;
        pushingSpriteRenderer = pushingSprite.GetComponent<SpriteRenderer>();
        pushingSpriteAnimator = pushingSprite.GetComponent<Animator>(); 

        idleSpriteRenderer = idleSprite.GetComponent<SpriteRenderer>(); 

        // Initialization
        SetSpriteAnimationSpeed(clickRate);             // Some animations adapt to the speed of the game
        SetTerrainAngle(currScore);                     // Steepness changes accoding to score
        for (float i=-5.0f; i<5.0f; i+=1.0f) {
            SpawnCloud(i);
        }
        UpdateStrengthUpgrade(10);
        UpdateClickUpgrade(10);
    }

    void Update() 
    {
        float rawClickRate = CalculateClickRate();
        light.intensity = (rawClickRate / 25.0f) * 32;
        // sisGlow.intensity = (rawClickRate / 25) * 16;
        clickRate = rawClickRate + baseClickRate;     // clickRate determines the speed of the game
        Debug.Log(clickRate);
        maxScoreText.text = Mathf.Floor(clickRate).ToString();              
    }

    void FixedUpdate()
    {
        float distanceDelta = UpdateDistanceAndScore(clickRate);    // current distance from start yields score (1:1)
        RotateLight(distanceDelta);

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

        pointsText.text = numberFormatter(points);
        pointsText.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
