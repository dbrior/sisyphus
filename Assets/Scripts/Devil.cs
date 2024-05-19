using UnityEngine;
using System.Collections;

public class Devil : MonoBehaviour
{
    public float entrySpeed = 3.0f;
    private Transform targetTransform;
    public AudioSource spawnSound;
    public AudioSource hitSound;
    public AudioSource deathSound;
    private bool hasCentered = false;
    public float targetVolumeIn = 0.25f;
    public float rampDuration = 13.0f;

    public GameObject prefabToSpawn; // The prefab to spawn
    public Vector3 spawnPosition; // The fixed position to spawn at
    public float spawnInterval = 5.0f; // Base interval in seconds
    public float randomFactor = 2.0f; // Randomness factor
    public float duration = 60.0f;
    private float lifetime = 0.0f;
    private bool spawnFireballs = false;
    private bool cycleComplete = false;
    private Vector3 exitLocation;
    public float maxHealth = 200f;
    private float currHealth;
    public float invincibleTime = 1f;
    private float timeTillVulnerable = 0;
    public RuntimeAnimatorController hitAnimation;
    public RuntimeAnimatorController flyingAnimation;
    public RuntimeAnimatorController deathAnimation;
    private Animator animator;
    private bool isHit = false;
    private float hitTime;
    bool ending = false;

    void Awake()
    {
        targetTransform = GameObject.Find("devilFloatLocation").transform;
    }

    void Start()
    {
        Debug.Log("Voice: " + BossIntro.Instance.GetRandomIntroLine());
        currHealth = maxHealth;
        animator = GetComponent<Animator>();
        SoundManager.Instance.scoreBlipSound.mute = true;
        SoundManager.Instance.bossMusic.Play();
        transform.position = new Vector3(0, -5, 0);
        exitLocation = transform.position;
        SoundManager.Instance.RampInto(SoundManager.Instance.bossMusic, SoundManager.Instance.backgroundMusic, targetVolumeIn, 0f, rampDuration);

        HealthBarManager.Instance.CreateHealthBar(gameObject);
    }
    void StartHit()
    {
        isHit = true;
        animator.runtimeAnimatorController = hitAnimation;

        hitSound.Play();
        animator.runtimeAnimatorController = hitAnimation;
        currHealth -= 10f;
        HealthBarManager.Instance.UpdateHealthBar(gameObject, currHealth/maxHealth);
        timeTillVulnerable = invincibleTime;
    }
    void EndHit()
    {
        isHit = false;
        animator.runtimeAnimatorController = flyingAnimation;
    }

    void Update()
    {
        if (timeTillVulnerable > 0)
        {
            timeTillVulnerable -= Time.deltaTime;
        }
        if (isHit && timeTillVulnerable <= 0)
        {
            EndHit();
        }
        
        if(currHealth <= 0f && !ending)
        {
            OnComplete();
        }
        
        if (ending)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1)
            // if (!animator.IsInTransition(0))
            {
                Exiting();
            }
        }
    }

    void FixedUpdate()
    {
        if (!hasCentered)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, entrySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetTransform.position) < 0.01f)
            {
                hasCentered = true;
                spawnFireballs = true;
                StartCoroutine(SpawnPrefabsAtInterval());
                gameObject.GetComponent<RandomBobbing>().centerPosition = targetTransform.position;
                SendMessage("OnCentered");
            }
        } else {
            lifetime += Time.deltaTime;

            if (lifetime >= duration)
            {
                SendMessage("OnComplete");  
            }
        }
    }

    void OnMouseDown()
    {
        if (!isHit)
        {
            StartHit();
        }
    }

    void Exiting()
    {
        Debug.Log("exiting");
        // bossMusic.Stop();
        Destroy(gameObject);
    }

    void OnComplete()
    {
        ending = true;
        animator.runtimeAnimatorController = deathAnimation;
        // deathSound.Play();
        SoundManager.Instance.DelaySound(SoundManager.Instance.successSound, 1.75f);
        animator.Play("DeathAnimation"); // Ensure the correct animation state is played
        SoundManager.Instance.scoreBlipSound.mute = false;
        cycleComplete = true;
        spawnFireballs = false;
        SoundManager.Instance.RampInto(SoundManager.Instance.backgroundMusic, SoundManager.Instance.bossMusic, targetVolumeIn, 0f, rampDuration);
    }

    IEnumerator SpawnPrefabsAtInterval()
    {
        while (spawnFireballs)
        {
            yield return new WaitForSeconds(spawnInterval + Random.Range(-randomFactor, randomFactor));
            GameObject fireball = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            SparkleScript fireballScript = fireball.GetComponent<SparkleScript>();
            Destroy(fireball, 5.0f);
        }
    }

    IEnumerator RampVolume(AudioSource audioSourceIn, AudioSource audioSourceOut)
    {
        float currentTime = 0;
        float startVolumeIn = 0;
        float startVolumeOut = audioSourceOut.volume;

        audioSourceIn.volume = startVolumeIn; // Ensure the fade-in source starts at 0
        while (currentTime < rampDuration)
        {
            currentTime += Time.deltaTime;
            audioSourceIn.volume = Mathf.Lerp(startVolumeIn, targetVolumeIn, currentTime / rampDuration);
            audioSourceOut.volume = Mathf.Lerp(startVolumeOut, 0, currentTime / rampDuration);
            yield return null; // Wait for the next frame
        }

        audioSourceIn.volume = targetVolumeIn; // Ensure the volume is set to the target volume at the end for fade-in
        audioSourceOut.volume = 0; // Ensure the volume is set to 0 at the end for fade-out
    }
}
