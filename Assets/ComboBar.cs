using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Image bar;
    public float moveSpeed = 100f;
    public Animator fireStreak;
    public Transform outPosition;
    public Transform inPosition;
    private bool hitStreak = false;
    void SetMultiplier(float progress)
    {
        if (progress >= 1) {
            WorldController.Instance.scoreMultiplier = 3f;
            if (!hitStreak) {
                fireStreak.Play("Entry", 0, 0f);
                fireStreak.gameObject.SetActive(true);
                fireStreak.enabled = true;
                hitStreak = true;
            }
        } else if (progress >= 0.5) {
            if (hitStreak) {
                hitStreak = false;
                fireStreak.gameObject.SetActive(false);
                fireStreak.enabled = false;
            }
            WorldController.Instance.scoreMultiplier = 2f;
        } else {
            WorldController.Instance.scoreMultiplier = 1f;
        }
    }

    void Start()
    {
        fireStreak.enabled = false;
        fireStreak.gameObject.SetActive(false);
    }

    void Update()
    {
        float progress = WorldController.Instance.rawExtendedClickRate / (1000f / 60f);
        bar.fillAmount = progress;

        SetMultiplier(progress);

        if (progress >= 0.1) {
            Debug.Log("Out");
            transform.position = Vector3.MoveTowards(transform.position, outPosition.position, moveSpeed * Time.deltaTime);
        } else {
            Debug.Log("In");
            transform.position = Vector3.MoveTowards(transform.position, inPosition.position, moveSpeed * Time.deltaTime);
        }
    }
}
