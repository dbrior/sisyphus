using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Rendering.Universal;

public class WorldController : Singleton<WorldController>
{
    public Light2D globalLight;
    public GameObject camera;
    private Vector3 cameraOriginalPosition;
    public TMP_FontAsset globalFont;
    [Header("Sprites:")]
    public GameObject backgroundA;
    public GameObject backgroundB;
    public GameObject pushingSprite;
    public GameObject idleSprite;
    public Animator boulderAnimator;
    [Header("Animation Speeds:")]
    public float maxAnimationSpeed = 2f;
    public float minAnimationSpeed = 0.25f;
    public float maxClickRateTarget = 75.0f;
    [Header("UI:")]
    public TextMeshProUGUI shopButtonText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI maxClickRateText;
    public TextMeshProUGUI pointsText;
    public GameObject shopUI;
    public GameObject sisyphusUpgradesUI;
    public GameObject petsUI;
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
    public float TimeWindow;
    public float ExtendedTimeWindow;
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

    public float maxScore = 0.0f;
    private float clickRate = 0.0f;
    private float currAccumulatedDelta = 0.0f;
    private float nextSpawnTime;
    public float currScore = 0.0f;
    private float terrainAngle;
    private List<float> inputTimestamps = new List<float>();
    private List<float> inputTimestampsExtended = new List<float>();
    private SpriteRenderer pushingSpriteRenderer;
    private Animator pushingSpriteAnimator;
    private SpriteRenderer idleSpriteRenderer;
    private int devilLevel = 1;

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
    public AudioSource purchaseSound;
    public SpriteAnimatorUI autoClickAnim;
    public SpriteAnimatorUI angleAnim;
    public Light2D light;
    public GameObject lightWheel;
    public float timeFactor = 0.005f;
    public GameObject sparklePrefab;
    private float sprocketTimestamp = 0.0f;
    public AudioSource blipAudio;
    public Transform SprocketSpawn;
    public GameObject devilPrefab;
    public bool startedFirstStage = false;
    public bool didGemTutorial = false;
    public bool didJumpTutorial = false;
    private float devilSpawnDistance;
    public float animationSpeed;
    public bool isMoving;
    public float rawClickRate;
    public float rawExtendedClickRate;
    public Transform dewieSpawn;
    public Transform dewieGangSpawn;
    public GameObject dewiePrefab;
    private bool dewieSpawned = false;
    public RectTransform lostPointsTargetTransform;
    public RectTransform lostPointsSpawnTransform;
    public GameObject lostPointsPrefab;
    public GameObject boulder;
    private Rigidbody2D boulder_rb;
    private SpriteRenderer boulder_sr;
    public Boulder boulder_b;
    private List<float> angularVelocities = new List<float>();
    private float timeSum = 0;
    public float averageAngularVelocity;
    private Vector2 velocity = Vector2.zero;
    private float previousAngularVelocity = 0f;
    private float angularAcceleration = 0f;
    public float autoClicksPerSecond = 1;  // Example starting rate
    private float accumulatedTime = 0.0f;
    private GameObject dewie;
    public float speed;
    public float smoothedSpeed;
    public GameObject pointAdditionPrefab;
    public AudioSource rockHit;
    public float gemSpawnDelta;
    private float accumulatedGemDelta = 0f;
    public GameObject gemPrefab;
    public AudioSource gemSpawnSound;
    public AudioSource gemCollectSound;
    public Sisyphus sisyphus;
    public float scoreMultiplier = 1f;
    public bool frozen;
    public GameObject cerberusPrefab;
    public Transform cerberusSpawn;
    private float initialMass;
    public int prestigePointsMultiplier = 3;
    
    private int lastSparkleSpawnScore;
    public GameObject harpyPrefab;
    public float harpySpawnInterval;
    public Dictionary<string, System.Action<float>> abilityImprovementActions = new Dictionary<string, System.Action<float>>();


    // ------------ Ability improvement functions ---------------------
    private float baseClickRate = 0.0f;
    private float originalClickRate;

    public float manualClickMultiplier = 1f;
    private float originalClickMultiplier;

    public float critChance = 5f;
    private float originalCritChance;

    public float critMultiplier = 2f;
    private float originalCritMultiplier;
    public int gemCount = 0;

    public void increaseBaseClickRate(float amount) {   // Auto Click
        baseClickRate += amount;
        accumulatedTime = 0f;
    }
    public void InceaseClickPower(float amount) {       // Click Power
        manualClickMultiplier *= (1+amount);
    }
    public void IncreaseCritChance(float amount) {      // Crit Chance
        critChance += amount;
    }
    public void IncreaseCritPower(float amount) {       // Crit Power
        critMultiplier += amount;
    }

    public void resetBuffs() {
        baseClickRate = originalClickRate;
        manualClickMultiplier = originalClickMultiplier;
        critChance = originalCritChance;
        critMultiplier = originalCritMultiplier;
    }

    private void storeBuffs() {
        originalClickRate = baseClickRate;
        originalClickMultiplier = manualClickMultiplier;
        originalCritChance = critChance;
        originalCritMultiplier = critMultiplier;
    }

    public void InstantiateImprovementActions() {
        System.Action<float> improveAutoClick = increaseBaseClickRate;
        System.Action<float> improveClickPower = InceaseClickPower;
        System.Action<float> improveCritChance = IncreaseCritChance;
        System.Action<float> improveCritPower = IncreaseCritPower;

        abilityImprovementActions.Add("AutoClick", improveAutoClick);
        abilityImprovementActions.Add("ClickPower", improveClickPower);
        abilityImprovementActions.Add("CritChance", improveCritChance);
        abilityImprovementActions.Add("CritPower", improveCritPower);
    }

    public Dictionary<string,string> AbilityNameMappings = new Dictionary<string,string>{
        {"AutoClick", "AUTO    CLICK"},
        {"ClickPower", "CLICK    POWER"},
        {"CritChance", "CRIT    CHANCE"},
        {"CritPower", "CRIT    POWER"}
    };

    public Dictionary<string,Color> AbilityColorMappings = new Dictionary<string,Color>{
        {"AutoClick", new Color(0.4f, 0.8f, 0.4f, 1f)},   // Darker Pastel Green
        {"ClickPower", new Color(1f, 0.36f, 0.34f, 1f)},  // Darker Pastel Red
        {"CritChance", new Color(0.9f, 0.9f, 0.35f, 1f)}, // Darker Pastel Yellow
        {"CritPower", new Color(0.7f, 0.6f, 0.8f, 1f)}    // Darker Pastel Purple
    };

    // -----------------------------------------------------------------

    public void Freeze()
    {
        frozen = true;
        Time.timeScale = 0;
    }

    public void SpawnLostPoints(int amount)
    {
        GameObject lostPoints = Instantiate(lostPointsPrefab);
        lostPoints.GetComponent<TextMeshPro>().text = "-" + amount.ToString();
        lostPoints.transform.position = lostPointsSpawnTransform.position;
    }

    public void SpawnCerberus()
    {
        GameObject cerberus = Instantiate(cerberusPrefab);
        cerberus.transform.position = cerberusSpawn.position;
        cerberus.transform.SetParent(transform);
    }

    public void SpawnDewie()
    {
        if (!dewieSpawned)
        {
            dewie = Instantiate(dewiePrefab);
            dewie.transform.position = dewieSpawn.position;
            // dewie.transform.rotation = dewieSpawn.rotation;
            // dewie.transform.scale = dewieSpawn.scale;
            dewieSpawned = true;
        }
    }

    // Sisyphus skills
    public void increaseSisMaxTerrainAngle() {
        Debug.Log("angle button pressed");
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

    // UI Toggle
    public void ToggleShopUI() {
        shopUI.SetActive(!shopUI.activeSelf);
        sisyphusUpgradesUI.SetActive(false);
        petsUI.SetActive(false);
        shopButtonText.text = shopUI.activeSelf ? "Close" : "Shop";
    }
    public void ToggleUpgradeUI() {
        sisyphusUpgradesUI.SetActive(!sisyphusUpgradesUI.activeSelf);
        shopUI.SetActive(false);
        petsUI.SetActive(false);
    }
    public void TogglePetsUI() {
        petsUI.SetActive(!petsUI.activeSelf);
        shopUI.SetActive(false);
        sisyphusUpgradesUI.SetActive(false);
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
            formattedNumber = (points / 1000).ToString("0.#") + "M";
        }
        else if (points >= 1000)
        {
            formattedNumber = (points / 1000).ToString("0.#") + "K";
        }
        else
        {
            formattedNumber = points.ToString();
        }
        return formattedNumber;
    }

    private Color colorFormatter(float points) {
        points = Mathf.Floor(points);
        Color formattedColor;
        Color lerpedColor;
        float t;
        // Define RGB values (you can adjust these values as needed)
        Color color_d = new Color(192f/255, 192f/255, 192f/255); // White
        Color mid = new Color(0.0f, 0.5f, 0.0f); // Greenish
        Color end = new Color(1.0f, 0.843f, 0.0f); // White
        // Define the range over which the color will change (1 to 1000 to 100000)
        float minValue = 1.0f;
        float midValue = 1000.0f;
        float maxValue = 10000.0f;
        // Calculate the interpolation factor (normalized between 0 and 1)
        if (points >= midValue)
        {            
            t = Mathf.Clamp01((points - midValue) / (maxValue - midValue));
            lerpedColor = Color.Lerp(mid, end, t);
        }
        else
        {
            lerpedColor = color_d;
            // t = Mathf.Clamp01((points - minValue) / (midValue - minValue));
            // lerpedColor = Color.Lerp(color_d, mid, t);
        }
        formattedColor = lerpedColor;
        return formattedColor;
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
        inputTimestamps.RemoveAll(timestamp => Time.time - timestamp > TimeWindow);
        return inputTimestamps.Count / TimeWindow;
    }
    private float CalculateExtendedClickRate()
    {
        inputTimestampsExtended.RemoveAll(timestamp => Time.time - timestamp > ExtendedTimeWindow);
        return inputTimestampsExtended.Count / ExtendedTimeWindow;
    }

    // Setting animation speeds
    private float GetAnimationSpeed(float speed) {
        if (speed > maxClickRateTarget) {
            return maxAnimationSpeed;
        } else {
            return ((speed/maxClickRateTarget)*(maxAnimationSpeed - minAnimationSpeed)) + minAnimationSpeed;
        }
    }
    private void SetSpriteAnimationSpeed(float clickRate) {
        if (!sisyphus.kicking) {
            sisyphus.SetAnimationSpeed(animationSpeed);
            if (clickRate > 0) {
                sisyphus.SetPushingState();
                boulderAnimator.speed = animationSpeed;
            } else {
                sisyphus.SetIdleState();
                boulderAnimator.speed = 0;
                boulderAnimator.playbackTime = 0;
            }
        }
    }

    private void RotateLight(float distanceDelta) {
        lightWheel.transform.eulerAngles = new Vector3(lightWheel.transform.eulerAngles.x, lightWheel.transform.eulerAngles.y, lightWheel.transform.eulerAngles.z - (distanceDelta * timeFactor));   
    }
    float GetBoulderSpeed()
    {
        float width = boulder_rb.transform.localScale.x;
        float angularVelocityRadians = boulder_rb.angularVelocity * Mathf.Deg2Rad;

        return angularVelocityRadians * (width) * -1;;
    }

    // Moving platforms & adjust score accordingly
    private float MovePlatforms(float clickRate) {
        Vector2 currPlatformAPosition = platformA.localPosition;
        Vector2 currPlatformBPosition = platformB.localPosition;

        Vector2 newPlatformAPosition;
        Vector2 newPlatformBPosition;

        speed = GetBoulderSpeed();
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, speed, Time.deltaTime / 0.5f);
        // Debug.Log(angularVelocityRadians.ToString());

        float movement = speed * Time.deltaTime * 10;

        newPlatformAPosition = new Vector2(platformA.localPosition.x - movement, platformA.localPosition.y);
        newPlatformBPosition = new Vector2(platformB.localPosition.x - movement, platformB.localPosition.y);

        // float currTerrainSkillModifier = (1 - (terrainAngle / sisMaxTerrainAngle));
        // float movement = (clickRate * moveSpeed) * currTerrainSkillModifier;

        // Use this one \/
        // float movement = (clickRate * moveSpeed);

        // Debug.Log(currTerrainSkillModifier);

        bool shouldLerp = true;
        // if (clickRate > 0) {
        //     newPlatformAPosition = new Vector2(platformA.localPosition.x - movement, platformA.localPosition.y);
        //     newPlatformBPosition = new Vector2(platformB.localPosition.x - movement, platformB.localPosition.y);
        // } else if (clickRate == 0 && Mathf.Round(currScore) > 0) {
        //     // float rollbackAmount = ((GetTerrainAngle(currScore)/maxTerrainAngle)*(maxRollbackSpeed - minRollbackSpeed)) + minRollbackSpeed;
        //     float rollbackAmount = 0.0f;
        //     newPlatformAPosition = new Vector2(platformA.localPosition.x + rollbackAmount, platformA.localPosition.y);
        //     newPlatformBPosition = new Vector2(platformB.localPosition.x + rollbackAmount, platformB.localPosition.y);
        // } else {
        //     shouldLerp = false;
        //     newPlatformAPosition = platformA.localPosition;
        //     newPlatformBPosition = platformB.localPosition;
        // }

        if (shouldLerp) {
            platformA.localPosition = Vector2.Lerp(currPlatformAPosition, newPlatformAPosition, Time.deltaTime);
            platformB.localPosition = Vector2.Lerp(currPlatformBPosition, newPlatformBPosition, Time.deltaTime);
        }

        delta = currPlatformAPosition.x - newPlatformAPosition.x;

        return delta;
    }
    private void MoveBackground(float distanceDelta) {
        Vector2 newBackgroundAPosition = new Vector2(backgroundA.transform.localPosition.x - (distanceDelta * backgroundMoveFactor), backgroundA.transform.localPosition.y);
        backgroundA.transform.localPosition = Vector2.Lerp(backgroundA.transform.localPosition, newBackgroundAPosition, Time.deltaTime);
        backgroundA.transform.eulerAngles = new Vector3(0,0,0);

        Vector2 newBackgroundBPosition = new Vector2(backgroundB.transform.localPosition.x - (distanceDelta * backgroundMoveFactor), backgroundB.transform.localPosition.y);
        backgroundB.transform.localPosition = Vector2.Lerp(backgroundB.transform.localPosition, newBackgroundBPosition, Time.deltaTime);
        backgroundB.transform.eulerAngles = new Vector3(0,0,0);
    }

    void UpdateScore(float distanceDelta) {
        // Score is really distance
        // This converts units to reasonable distance values
        currScore += distanceDelta / deltaScoreRatio;

        float pointsPerMeter = 10f * (boulder_rb.mass / initialMass);
        float multiplier = Mathf.Max((boulder_b.currPrestige * prestigePointsMultiplier), 1f);

        pointsPerMeter *= multiplier;

        // currAccumulatedDelta += distanceDelta / deltaScoreRatio;
        // scoreText.text = Mathf.Floor(currScore).ToString();
        if (currScore > maxScore) {
            PlayerPrefs.SetFloat("Max Score", currScore);
            PlayerPrefs.Save();
            points += (currScore - maxScore) * pointsPerMeter;
            maxScore = currScore;
        }
        // maxScoreText.text = Mathf.Floor(maxScore).ToString();
    }

    float UpdateDistanceAndScore(float clickRate) {
        float distanceDelta = MovePlatforms(clickRate);
        MoveBackground(distanceDelta);
        UpdateScore(distanceDelta);
        return distanceDelta;
    }
    
    void Start()
    {
        InstantiateImprovementActions();
        storeBuffs();

        cameraOriginalPosition = camera.transform.position;
        // Set Font
        var tmpTexts = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text tmpText in tmpTexts)
        {
            tmpText.font = globalFont;
        }

        boulder_rb = boulder.GetComponent<Rigidbody2D>();
        boulder_sr = boulder.GetComponent<SpriteRenderer>();
        boulder_b = boulder.GetComponent<Boulder>();

        initialMass = boulder_rb.mass;

        Application.targetFrameRate = 60;
        // currScore = 2000.0f;
        maxScore = PlayerPrefs.GetFloat("Max Score", 0.0f);
        maxScore = 0.0f;
        // pushingSpriteRenderer = pushingSprite.GetComponent<SpriteRenderer>();
        // pushingSpriteAnimator = pushingSprite.GetComponent<Animator>(); 

        // idleSpriteRenderer = idleSprite.GetComponent<SpriteRenderer>();

        animationSpeed = GetAnimationSpeed(0.0f);

        // Initialization
        SetSpriteAnimationSpeed(clickRate);             // Some animations adapt to the speed of the game
        SetTerrainAngle(currScore);                     // Steepness changes accoding to scoregit
        for (float i=-5.0f; i<5.0f; i+=1.0f) {
            // SpawnCloud(i);
        }

        devilSpawnDistance = 10000.0f + Random.Range(0.0f, 10.0f);
        Debug.Log(devilSpawnDistance.ToString());
        // UpdateStrengthUpgrade(10);
        // UpdateClickUpgrade(10);

        lastSparkleSpawnScore = 0;
    }
    private float CalculateAverageAngularVelocity()
    {
        if (angularVelocities.Count == 0)
            return 0;

        float sum = 0;
        foreach (float av in angularVelocities)
        {
            sum += av;
        }
        return sum / angularVelocities.Count;
    }
    void SpawnSparkle(Vector3 spawnPosition)
    {
        GameObject sparkle = Instantiate(sparklePrefab, spawnPosition, Quaternion.identity);
        Destroy(sparkle, 0.925f);  // Auto destroy the sparkle after 1.5 seconds
    }
    void PlayClickSound()
    {
        float pitchModifier = Random.Range(-0.75f, -0.2f);
        blipAudio.pitch = 1 + pitchModifier;
        blipAudio.Play();
    }
    public void UseItem(Item item) {
        System.Action<float> action = abilityImprovementActions[item.type];
        action(item.value);
    }
    public void AddPointTextSpawn(Vector3 clickLocation, string amount, Color color, float fontSize, float fadeDuration)
    {
        PointAddition addition = Instantiate(pointAdditionPrefab).GetComponent<PointAddition>();
        addition.initialForce = Vector2.left * Random.Range(-50f,50f);
        addition.text = "+" + amount;
        addition.textColor = color;
        addition.fontSize = fontSize;
        addition.fadeDuration = fadeDuration;
        addition.transform.position = clickLocation;
        Destroy(addition, 0.5f);
    }
    public void ManualClick(Vector3 clickLocation, bool wasAuto, int multiplicity)
    {
        bool critical = Random.Range(0, 100) <= critChance;
        float clickForce = 1000.0f * scoreMultiplier * manualClickMultiplier * multiplicity;
        if (critical)
        {
            clickForce *= critMultiplier;
        }

        if (!wasAuto) {
            inputTimestamps.Add(Time.time);
            inputTimestampsExtended.Add(Time.time);
        }
        
        boulder_rb.AddTorque(-clickForce);
        
        string pointString = ((Mathf.Ceil((clickForce / boulder_rb.mass) * Time.deltaTime * 3f * 10f) / 10f) * (boulder_rb.mass / initialMass) * Mathf.Max((boulder_b.currPrestige * prestigePointsMultiplier), 1f) * 10f).ToString("F0");
        if (critical)
        {
            AddPointTextSpawn(clickLocation, pointString, new Color(255f/255, 110f/255, 0f/255, 1f), 1.5f, 1.5f);
            rockHit.Play();
        } else {
            AddPointTextSpawn(clickLocation, pointString, new Color(1f, 1f, 1f, 1f), 1f, 0.75f);
            PlayClickSound();
        }
        // SpawnSparkle(SprocketSpawn.position); spawn every 1m instead @ Update()
    }
    void AutoClick(int num_clicks)
    {
        float offset = 0.35f;
        float yShift = 0.2f;
        boulder_b.Touched(boulder.transform.position + new Vector3(Random.Range(-offset,offset), Random.Range(-offset+yShift,offset+yShift), 0), true, num_clicks);
        // boulder_rb.AddTorque(-1000.0f * num_clicks * scoreMultiplier * manualClickMultiplier);
    }

    public void StartHarpySpawns() {
        StartCoroutine(SpawnHarpy());
    }

    IEnumerator SpawnHarpy()
    {
        while (true) {
            GameObject harpy = Instantiate(harpyPrefab);
            Destroy(harpy, 10f);
            yield return new WaitForSeconds(harpySpawnInterval);
        }
    }

    void Update() 
    {
        // Debug.Log(critChance);
        // CLick function
        // Calculate the number of clicks that should have occurred since the last frame
        accumulatedTime += Time.deltaTime;
        int clicksThisFrame = (int)(baseClickRate * accumulatedTime);

        // Reset the accumulated time
        if (clicksThisFrame > 0)
        {
            accumulatedTime -= clicksThisFrame / baseClickRate;
            
            // Perform the click action however many times is necessary
            AutoClick(clicksThisFrame);
        }

        rawClickRate = CalculateClickRate();
        rawExtendedClickRate = CalculateExtendedClickRate();

        light.intensity = (rawClickRate / 25.0f) * 32;
        // sisGlow.intensity = (rawClickRate / 25) * 16;
        clickRate = rawClickRate + baseClickRate;     // clickRate determines the speed of the game
        // Debug.Log(clickRate);
        // maxClickRateText.text = Mathf.Floor(clickRate).ToString();

        // Spawn sparkle every 1m
        // if (currScore >= lastSparkleSpawnScore + 1f) {
        //     int roundedScore = (int) Mathf.Floor(currScore);
        //     int sparkelsToSpawn = roundedScore - lastSparkleSpawnScore;

        //     for (int i=0; i<sparkelsToSpawn; i++) {
        //         SpawnSparkle(SprocketSpawn.position);
        //     }

        //     lastSparkleSpawnScore = roundedScore;
        // }

        // Remove the oldest data if the total time exceeds 10 seconds
        while (timeSum > TimeWindow)
        {
            if (angularVelocities.Count > 0)
            {
                timeSum -= Time.deltaTime;  // Remove the time delta of the first recorded velocity
                angularVelocities.RemoveAt(0);  // Remove the oldest angular velocity
            }
            else
            {
                timeSum = 0;  // Reset time sum if no velocities are stored
                break;
            }
        }

        // Spawn gems
        if (accumulatedGemDelta >= gemSpawnDelta)
        {
            gemSpawnSound.Play();
            GameObject gem = Instantiate(gemPrefab);
            gem.transform.position = boulder.transform.position;
            float randomForceModifier = Random.Range(-75f,75f);
            Vector2 sideForce = Vector2.left * randomForceModifier;
            gem.GetComponent<Rigidbody2D>().AddForce((Vector2.up*400f) + sideForce);
            gem.GetComponent<Rigidbody2D>().AddTorque(randomForceModifier/20f);
            Color color = new Color(Random.value, Random.value, Random.value);
            gem.GetComponent<SpriteRenderer>().color = color;
            gem.GetComponent<Light2D>().color = color;
            accumulatedGemDelta = 0f;
            Destroy(gem, 5f);
        } else {
            accumulatedGemDelta += Time.deltaTime;
        }

        // Check for kick
        if (Input.GetKeyDown(KeyCode.Space)) {
            sisyphus.Kick();
        }
    }

    void FixedUpdate()
    {
        // // New boulder speed
        // // Store the angular velocity along with the timestamp
        // angularVelocities.Add(boulder_rb.angularVelocity);
        // timeSum += Time.deltaTime;

        // // Calculate the average angular velocity
        // averageAngularVelocity = CalculateAverageAngularVelocity();
        // float boulderDeltaV = (boulder_rb.angularVelocity - averageAngularVelocity) / 1000.0f;

        // // Update camera position accordingly
        // // float cameraOffset = Mathf.Round(Mathf.Clamp(boulderDeltaV, -1f, 1f) * 10f) / 10f;
        // float cameraOffset = Mathf.Clamp(boulderDeltaV, -1f, 1f);
        // if (Mathf.Abs(cameraOffset) < 0.3)
        // {
        //     cameraOffset = 0f;
        // }

        float angularAcceleration = (boulder_rb.angularVelocity - previousAngularVelocity) / Time.fixedDeltaTime;
        previousAngularVelocity = boulder_rb.angularVelocity;

        float cameraOffset = angularAcceleration / 1000f;

        Vector2 cameraOffsetPosition = new Vector2(cameraOriginalPosition.x + cameraOffset, camera.transform.position.y);
        
        // Debug.Log(cameraOffset.ToString());

        Vector2 dampedPosition = Vector2.SmoothDamp(camera.transform.position, cameraOffsetPosition, ref velocity, 2f);

        camera.transform.position = new Vector3(dampedPosition.x, dampedPosition.y, -10f);





        // Debug.Log(currScore);
        points = Mathf.Max(0.0f, points);

        float distanceDelta = UpdateDistanceAndScore(clickRate);    // current distance from start yields score (1:1)

        // DAY AND NIGHt
        float frequency = 0.0001f;
        float lightLevel = (0.35f * Mathf.Cos(currScore * frequency)) + 0.65f; // Starting at 1, dips to 0.3
        // float lightLevel = (Mathf.Sin((currScore+10000) * frequency) + 0) / 2 * 0.75f;
        globalLight.intensity = lightLevel;

        // RotateLight(distanceDelta);

        isMoving = clickRate > 0.0f;

        animationSpeed = GetAnimationSpeed(speed);

        // Spawn sparkles for auto click
        // if (baseClickRate != 0 && (Time.time - sprocketTimestamp >= (1 / baseClickRate))) {
        //     GameObject sparkle = Instantiate(sparklePrefab, SprocketSpawn.position, Quaternion.identity);
        //     Destroy(sparkle, 0.925f);  // Auto destroy the sparkle after 1.5 seconds
        //     sprocketTimestamp = Time.time;
        //     // float pitchModifier = Random.Range(-0.75f, -0.2f);
        //     // blipAudio.pitch = 1 + pitchModifier;
        //     // blipAudio.Play();
        // }

        // boulder.transform.eulerAngles = new Vector3(boulder.transform.eulerAngles.x, boulder.transform.eulerAngles.y, boulder.transform.eulerAngles.z - (clickRate/ 10.0f));

        // Below rely on either clickRate or score
        SetSpriteAnimationSpeed(clickRate);             // Some animations adapt to the speed of the game
        SetTerrainAngle(currScore);                     // Steepness changes accoding to score

        // TODO: These functions have todo's
        if(ShouldSpawnCloud()) {
            // SpawnCloud();
        }
        // if(ShouldSpawnBird()) {
        //     SpawnBird();
        // }

        if (!startedFirstStage && currScore >= devilSpawnDistance)
        {
            GameObject devilObject = Instantiate(devilPrefab);
            devilObject.transform.position = new Vector2(0.0f, -4.0f);
            startedFirstStage = true;
            if (dewieSpawned)
            {
                Destroy(dewie);
                for (int i = 0; i<=5; i++)
                {
                    Dewie newDewie = Instantiate(dewiePrefab).GetComponent<Dewie>();
                    newDewie.wander = true;
                    newDewie.baseGlowIntensity = 2;
                    newDewie.color = new Color(Random.value, Random.value, Random.value);
                    newDewie.transform.SetParent(dewieGangSpawn.parent);
                    // newDewie.UpdateColor(new Color(Random.value, Random.value, Random.value));
                    newDewie.transform.localPosition = new Vector2(dewieGangSpawn.localPosition.x + Random.Range(-1f,1f), dewieGangSpawn.localPosition.y);
                    RandomBobbing dewieBobber = newDewie.GetComponent<RandomBobbing>();
                    dewieBobber.centerPosition = newDewie.transform.localPosition;
                    dewieBobber.startBobbing = true;
                    Debug.Log(newDewie.transform.position);
                }
            }
        }

        // pointsText.text = numberFormatter(points);
        // pointsText.color = colorFormatter(points);
        // pointsText.outlineWidth = 0.4f;
        // pointsText.outlineColor = new Color(27/255,22/255,34/255);
        // pointsText.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
