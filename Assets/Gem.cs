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

    void Start()
    {
        rb  = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currLifetime += Time.deltaTime;
        if (!hasFrozen && currLifetime > 1f)
        {
            hasFrozen = true;
            tappingTutorial.gameObject.SetActive(true);
            tappingTutorial.Activate();
        }
    }
    void OnMouseDown()
    {
        if (WorldController.Instance.frozen) {
            Time.timeScale = 1f;
            tappingTutorial.gameObject.SetActive(false);
            WorldController.Instance.frozen = false;
        }
        WorldController.Instance.gemCollectSound.Play();
        WorldController.Instance.points += pointValue;
        WorldController.Instance.AddPointTextSpawn(transform.position, pointValue.ToString(), new Color(43f/255, 1f, 0f), 1.75f, 2f);
        Destroy(gameObject);
    }
}
