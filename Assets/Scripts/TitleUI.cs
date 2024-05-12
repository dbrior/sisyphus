using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    private WorldController worldController;
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
    private bool fired = false;

    public float radius = 5.0f;
    public float speed = 0.5f;
    public float smoothTime = 0.3f;
    private Vector2 centerPosition;
    private Vector2 targetPosition;
    private Vector2 velocity = Vector2.zero;
    private float timeOffset;
    public bool startBobbing = false;
    private float lastUpdateTime = 0;
    private float updateInterval = 0.1f; // Update target position every 0.1 seconds

    private RectTransform rectTransform;


    void EnableUI()
    {
        for (int i=0; i<objectsToEnable.Length; i++)
        {
            objectsToEnable[i].SetActive(true);
        }
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        centerPosition = rectTransform.anchoredPosition;
        timeOffset = Random.Range(0f, 10000f);
        worldController = GameObject.Find("Main Grid").GetComponent<WorldController>();
        // textUI = GetComponent<TextMeshProUGUI>();
        // textUI.outlineWidth = 0.4f;
        // textUI.outlineColor = new Color(27 / 255, 22 / 255, 34 / 255);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fired && worldController.currScore > 0)
        {
            EnableUI();
            fired = true;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            lastUpdateTime = Time.time;
            float newX = Mathf.PerlinNoise(timeOffset + Time.time * speed, 0f) * 2 - 1;
            float newY = Mathf.PerlinNoise(0f, timeOffset + Time.time * speed) * 2 - 1;

            targetPosition = new Vector2(newX, newY) * radius + centerPosition;
        }

        rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, targetPosition, ref velocity, smoothTime);
    }
}
