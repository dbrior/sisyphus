using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/********************************************************************************************
UPGRADE PROGRESSION:
Base Cost = 10
Base CPS = 0.3

(Upgrade N Cost / Upgrade N CPS) = (Upgrade N-1 Cost / Upgrade N-1 CPS) * 3
Uprage N Cost = Upgrade N-1 Cost * 10

After purchase, Cost = Cost * 1.3
After purchase, CPS = CPS
********************************************************************************************/

public class Upgrade : MonoBehaviour
{
    public float cost;
    public float value;

    public GameObject upgradeCountObject;
    public TextMeshProUGUI upgradeCountUI;
    public TextMeshProUGUI upgradeCostUI;
    public TextMeshProUGUI upgradeValueUI;
    public Image iconImage;
    public Material grayscaleMaterial;
    public GameObject grayscaleCover;
    public UnityEvent onPurchase;
    public UnityEvent onFirstPurchase;
    public bool isStaticText = false;
    

    private int upgradeCount;
    private WorldController controller;
    private bool hidden = true;

    void Start()
    {
        controller = GameObject.Find("Main Grid").GetComponent<WorldController>();

        upgradeCountUI.text = upgradeCount.ToString();
        if (!isStaticText) {
            upgradeCostUI.text = cost.ToString();
            // upgradeValueUI.text = "CPS: " + value.ToString();
        }

        if (onPurchase == null) onPurchase = new UnityEvent();

        if (onFirstPurchase == null) onFirstPurchase = new UnityEvent();
    }

    void Update()
    {
        if (hidden && controller.points >= cost) {
            iconImage.material = null;
            grayscaleCover.SetActive(false);
            hidden = false;
        } else if (!hidden && controller.points < cost) {
            iconImage.material = grayscaleMaterial;
            grayscaleCover.SetActive(true);
            hidden = true;
        }
    }

    public void Pressed()
    {
        Debug.Log("Upgrade Pressed");
        if (controller.points >= cost)
        {
            controller.points -= cost;
            controller.increaseBaseClickRate(value);
            upgradeCount++;
            Debug.Log("Base Click Rate Increase!");
            controller.purchaseSound.Play();

            // Spawn devil
            // GameObject devil = Instantiate(devilPrefab);
            // devil.transform.position = new Vector2(0, -10);

            cost = Mathf.Round(cost * 1.3f * 10f) / 10f;

            if (!isStaticText) {
                upgradeCostUI.text = cost.ToString();
            }
            upgradeCountUI.text = upgradeCount.ToString();

            if (upgradeCount > 0)
            {
                upgradeCountObject.SetActive(true);
            }

            onPurchase.Invoke();
            if (upgradeCount == 1) {
                onFirstPurchase.Invoke();
            }
        }
    }

    public void increaseBaseClickRate(float amount, float cost)
    {
        
    }
}
