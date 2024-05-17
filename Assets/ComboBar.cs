using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Image bar;
    public float moveSpeed = 100f;
    public Transform outPosition;
    public Transform inPosition;
    void Start()
    {
        
    }
    void SetMultiplier(float progress)
    {
        if (progress >= 1) {
            WorldController.Instance.scoreMultiplier = 3f;
        } else if (progress >= 0.5) {
            WorldController.Instance.scoreMultiplier = 2f;
        } else {
            WorldController.Instance.scoreMultiplier = 1f;
        }
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
