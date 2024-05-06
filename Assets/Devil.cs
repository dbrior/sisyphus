using UnityEngine;
using System.Collections;

public class Devil : MonoBehaviour
{
    public float entrySpeed = 3.0f;
    private Transform targetTransform;
    private AudioSource spawnSound;
    private AudioSource bossMusic;
    private AudioSource baseMusic;
    private AudioSource scoreBlip;
    private bool hasCentered = false;
    public float targetVolumeIn = 0.25f;
    public float rampDuration = 13.0f;

    public GameObject prefabToSpawn; // The prefab to spawn
    public Vector3 spawnPosition; // The fixed position to spawn at
    public float spawnInterval = 5.0f; // Base interval in seconds
    public float randomFactor = 2.0f; // Randomness factor
    private WorldController worldController;
    private AudioSource collisionSound;

    void Awake()
    {
        spawnSound = GameObject.Find("DevilSpawn").GetComponent<AudioSource>();
        bossMusic = GameObject.Find("BossMusic").GetComponent<AudioSource>();
        scoreBlip = GameObject.Find("ScoreBlip").GetComponent<AudioSource>();
        baseMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        collisionSound = GameObject.Find("Explosion").GetComponent<AudioSource>();
        
        targetTransform = GameObject.Find("devilFloatLocation").transform;

        worldController = GameObject.Find("Main Grid").GetComponent<WorldController>();
    }

    void Start()
    {
        scoreBlip.mute = true;
        // spawnSound.Play();
        bossMusic.Play();
        transform.position = new Vector3(0, -5, 0);
        StartCoroutine(RampVolume(bossMusic, baseMusic));
    }

    void FixedUpdate()
    {
        if (!hasCentered)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, entrySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetTransform.position) < 0.01f)
            {
                hasCentered = true;
                StartCoroutine(SpawnPrefabsAtInterval());
                gameObject.GetComponent<RandomBobbing>().centerPosition = targetTransform.position;
                SendMessage("OnCentered");
            }
        }
    }

    IEnumerator SpawnPrefabsAtInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval + Random.Range(-randomFactor, randomFactor));
            GameObject fireball = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            fireball.GetComponent<SparkleScript>().collisionSound = collisionSound;
            fireball.GetComponent<SparkleScript>().worldController = worldController;
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
