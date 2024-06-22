using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    public List<Sprite> sprites;
    public float lifetimeSeconds;
    public float cycleDurationSeconds;
    public float minCycleIntervalSeconds;
    public float maxCycleIntervalSeconds;
    public float maxSize;
    public int pulseCycles = 2;
    public GameObject sparklePrefab;
    public AudioSource selectSound;
    public AudioSource blipSound;
    private float cycleIntervalSeconds;
    private float elapsedTime = 0f;
    private bool cycling = true;
    private Vector3 originalScale;

    private SpriteRenderer sr;
    void Start () {
        cycleIntervalSeconds = minCycleIntervalSeconds;
        sr = gameObject.GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        Destroy(gameObject, lifetimeSeconds);
        StartCoroutine(ConinuousCyleSprite());
        cycling = true;
    }

    void CycleSprite() {
        blipSound.Play();

        sr.sprite = sprites[Random.Range(0, sprites.Count-1)];
    }

    IEnumerator Grow(float duration) {
        float time = 0f;

        while (time < duration) {
            transform.localScale = Vector3.Lerp(originalScale, originalScale*maxSize, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale*maxSize;
    }

    IEnumerator Shrink(float duration) {
        float time = 0f;

        while (time < duration) {
            transform.localScale = Vector3.Lerp(originalScale*maxSize, originalScale, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }

    IEnumerator GrowAndShrink(float duration)
    {
        for (int i=0; i<pulseCycles; i++) {
            yield return StartCoroutine(Grow(duration/2f));

            yield return StartCoroutine(Shrink(duration/2f));
        }
    }

    void SelectItem() {
        selectSound.Play();
        for (int i=0; i<15; i++) {
            GameObject sparkle = Instantiate(sparklePrefab);
            sparkle.transform.position = transform.position;
            Destroy(sparkle, 2f);
        }
        StartCoroutine(GrowAndShrink(0.5f));
    }

    void Update() {
        if (cycling) {
            cycleIntervalSeconds = ((elapsedTime / cycleDurationSeconds) * (maxCycleIntervalSeconds - minCycleIntervalSeconds)) + minCycleIntervalSeconds;
        }
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, WorldController.Instance.boulder.transform.position, 0.1f*Time.deltaTime);
    }

    IEnumerator ConinuousCyleSprite()
    {
        while (elapsedTime < cycleDurationSeconds) {
            CycleSprite();
            yield return new WaitForSeconds(cycleIntervalSeconds);
        }
        cycling = false;
        SelectItem();
    }
}