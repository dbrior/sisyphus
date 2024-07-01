using UnityEngine;
using System.Collections;

public class Devil : MonoBehaviour
{
    // Static entries
    private static int level = 1;
    private static float baseHealth = 50f;
    private static float healthScaleFactor = 2f;

    private static float baseMovementSpeed = 0.5f;
    private static float movementSpeedScaleFactor = 2f;

    public GameObject meteorPrefab;
    private static float baseMeteorSpawnInterval = 5f;
    private static float meteorSpawnIntervalScaleFactor = 2f;

    public RuntimeAnimatorController hitAnimation;
    public RuntimeAnimatorController flyingAnimation;
    public RuntimeAnimatorController deathAnimation;

    public AudioSource spawnSound;
    public AudioSource hitSound;
    public AudioSource deathSound;

    // Health
    private float maxHealth;
    private float currHealth;
    // Speed
    private float movementSpeed;
    // Meteors
    public Vector3 meteorSpawnPosition;
    private float meteorSpawnInterval;
    // Animations
    private Animator animator;
    // Location
    private Vector3 targetPosition;
    
    public float targetVolumeIn = 0.25f;
    public float rampDuration = 13.0f;

    
    public float fireballSpawnTimeRandomFactor = 2.0f; // Randomness factor
    private bool spawnFireballs = false;
    public float invincibleTime = 1f;
    private float timeTillVulnerable = 0;
    private bool isHit = false;
    private float hitTime;
    private bool ending = false;

    void Start() {
        // Components
        animator = GetComponent<Animator>();

        // Stats
        maxHealth = baseHealth * level;
        currHealth = maxHealth;

        movementSpeed = baseMovementSpeed * level;

        meteorSpawnInterval = baseMeteorSpawnInterval * (1f / level);

        // Voice line
        Debug.Log("Voice: " + BossIntro.Instance.GetRandomIntroLine());
        
        // Sounds
        SoundManager.Instance.scoreBlipSound.mute = true;
        SoundManager.Instance.bossMusic.Play();
        SoundManager.Instance.RampInto(SoundManager.Instance.bossMusic, SoundManager.Instance.backgroundMusic, targetVolumeIn, 0f, rampDuration);

        // Health bar
        HealthBarManager.Instance.CreateHealthBar(gameObject);

        // Position
        transform.position = new Vector3(0, -5, 0);

        // Start spawns
        spawnFireballs = true;
        StartCoroutine(SpawnPrefabsAtInterval());
        // StartCoroutine(SelectRandomPositions());
    }

    void StartHit() {
        isHit = true;
        hitSound.Play();
        animator.runtimeAnimatorController = hitAnimation;
        currHealth -= 10f;
        HealthBarManager.Instance.UpdateHealthBar(gameObject, currHealth/maxHealth);
        timeTillVulnerable = invincibleTime;
    }

    void EndHit() {
        isHit = false;
        animator.runtimeAnimatorController = flyingAnimation;
    }

    void Update() {
        // Move to target position
        if (transform.position == targetPosition) {
            targetPosition = GetNewTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        // Check if in invincible frames
        if (timeTillVulnerable > 0) {
            timeTillVulnerable -= Time.deltaTime;
        } else if (isHit && timeTillVulnerable <= 0) {
            EndHit();
        }
        
        // Check if dead
        if(currHealth <= 0f && !ending) {
            Death();
        } else if (ending) {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1) {
                Exit();
            }
        }
    }

    void OnMouseDown() {
        if (!isHit) StartHit();
    }

    void Exit() {
        // Set new spawn
        WorldController.Instance.devilSpawnDistance = WorldController.Instance.currScore + 10000f + Random.Range(0f, 1000f);
        WorldController.Instance.startedFirstStage = false;
        WorldController.Instance.distanceMeter.UpdateBothGoalDistances(DistanceGoal.Type.BossBattle, WorldController.Instance.currScore, WorldController.Instance.devilSpawnDistance);

        Destroy(gameObject);
    }

    void Death() {
        level += 1;
        ending = true;
        animator.runtimeAnimatorController = deathAnimation;
        // deathSound.Play();

        SoundManager.Instance.DelaySound(SoundManager.Instance.successSound, 1.75f);
        animator.Play("DeathAnimation"); // Ensure the correct animation state is played
        SoundManager.Instance.scoreBlipSound.mute = false;
        spawnFireballs = false;
        SoundManager.Instance.RampInto(SoundManager.Instance.backgroundMusic, SoundManager.Instance.bossMusic, targetVolumeIn, 0f, rampDuration);
    }

    Vector3 GetNewTarget() {
        float xBounds = 1f;
        float yBounds = 3f;

        float xPos = WorldController.Instance.boulder.transform.position.x + Random.Range(-xBounds, xBounds);
        float yPos = WorldController.Instance.boulder.transform.position.y + Random.Range(0, yBounds);

        return new Vector3(xPos, yPos, 0);
    }

    // IEnumerator SelectRandomPositions() {
    //     while (!ending) {
    //         targetPosition = GetNewTarget();
    //         yield return new WaitForSeconds(1.5f);
    //     }
    // }

    IEnumerator SpawnPrefabsAtInterval() {
        while (spawnFireballs) {
            yield return new WaitForSeconds(meteorSpawnInterval + Random.Range(-fireballSpawnTimeRandomFactor, fireballSpawnTimeRandomFactor));
            GameObject fireball = Instantiate(meteorPrefab, meteorSpawnPosition, Quaternion.identity);
            SparkleScript fireballScript = fireball.GetComponent<SparkleScript>();
            Destroy(fireball, 5.0f);
        }
    }

    IEnumerator RampVolume(AudioSource audioSourceIn, AudioSource audioSourceOut) {
        float currentTime = 0;
        float startVolumeIn = 0;
        float startVolumeOut = audioSourceOut.volume;

        audioSourceIn.volume = startVolumeIn; // Ensure the fade-in source starts at 0
        while (currentTime < rampDuration) {
            currentTime += Time.deltaTime;
            audioSourceIn.volume = Mathf.Lerp(startVolumeIn, targetVolumeIn, currentTime / rampDuration);
            audioSourceOut.volume = Mathf.Lerp(startVolumeOut, 0, currentTime / rampDuration);
            yield return null; // Wait for the next frame
        }

        audioSourceIn.volume = targetVolumeIn; // Ensure the volume is set to the target volume at the end for fade-in
        audioSourceOut.volume = 0; // Ensure the volume is set to 0 at the end for fade-out
    }
}
