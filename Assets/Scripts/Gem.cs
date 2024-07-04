using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float pointValue = 1000f;
    private Rigidbody2D rb;
    private bool hasFrozen = false;
    private float currLifetime = 0f;
    public TappingTutorial tappingTutorial;
    public AudioSource spawnSound;
    private bool didGemTutorial = false;

    void Start()
    {
        rb  = GetComponent<Rigidbody2D>();
        didGemTutorial = WorldController.Instance.didGemTutorial;
        spawnSound.Play();
    }

    void Update()
    {
        currLifetime += Time.deltaTime;
        if (!didGemTutorial && !hasFrozen && currLifetime > 1f)
        {
            hasFrozen = true;
            tappingTutorial.gameObject.SetActive(true);
            tappingTutorial.Activate();
        }
    }
    void OnMouseDown()
    {
        if (WorldController.Instance.frozen && !didGemTutorial) {
            Time.timeScale = 1f;
            tappingTutorial.gameObject.SetActive(false);
            WorldController.Instance.frozen = false;
            WorldController.Instance.didGemTutorial = true;
            WorldController.Instance.boulder_b.swipeDetection.enabled = true;
            WorldController.Instance.StartHarpySpawns();
        } else if (!didGemTutorial && !WorldController.Instance.frozen) {
            return;
        }

        WorldController.Instance.gemCollectSound.Play();
        // WorldController.Instance.points += pointValue;
        WorldController.Instance.gemCount += 1;
        WorldController.Instance.AddPointTextSpawn(transform.position, 1.ToString(), new Color(43f/255, 1f, 0f), 1.75f, 2f);
        Destroy(gameObject);
    }
}
