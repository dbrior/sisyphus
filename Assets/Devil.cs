using UnityEngine;

public class Devil : MonoBehaviour
{
    public float entrySpeed = 3.0f;
    private Transform targetTransform;
    private AudioSource spawnSound;
    private AudioSource bossMusic;
    private AudioSource scoreBlip;
    private bool hasCentered = false;

    void Awake()
    {
        spawnSound = GameObject.Find("DevilSpawn").GetComponent<AudioSource>();
        bossMusic = GameObject.Find("BossMusic").GetComponent<AudioSource>();
        scoreBlip = GameObject.Find("ScoreBlip").GetComponent<AudioSource>();

        // Stop the background music
        GameObject.Find("Music").GetComponent<AudioSource>().Stop();
        
        targetTransform = GameObject.Find("devilFloatLocation").transform;
    }

    void Start()
    {
        scoreBlip.mute = true;
        // spawnSound.Play();
        bossMusic.Play();
        transform.position = new Vector3(0, -5, 0);
    }

    void FixedUpdate()
    {
        if (!hasCentered)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, entrySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetTransform.position) < 0.01f)
            {
                hasCentered = true;
                gameObject.GetComponent<RandomBobbing>().centerPosition = targetTransform.position;
                SendMessage("OnCentered");
            }
        }
    }
}
