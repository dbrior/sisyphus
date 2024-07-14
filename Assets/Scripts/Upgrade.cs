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
    [SerializeField] private float costScaleFactor = 1.3f;
    [SerializeField] private SkillType skillType;
    [SerializeField] private bool isPrestigeUpgrade = false;

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
    private bool hidden = true;

    [SerializeField] private bool useNewSkillSystem = false;

    void Start()
    {
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
        float points = GetPoints();
        if (hidden && points >= cost) {
            iconImage.material = null;
            grayscaleCover.SetActive(false);
            hidden = false;
        } else if (!hidden && points < cost) {
            iconImage.material = grayscaleMaterial;
            grayscaleCover.SetActive(true);
            hidden = true;
        }
    }

    private float GetPoints() {
        if (isPrestigeUpgrade) {
            return WorldController.Instance.GetPrestigePoints();
        } else {
            return WorldController.Instance.points;
        }
    }

    private void SubtractPoints(float amount) {
        if (isPrestigeUpgrade) {
            WorldController.Instance.SubtractPrestigePoints(amount);
        } else {
            WorldController.Instance.points -= amount;
        }
    }

    private void IncreaseCost() {
        cost *= costScaleFactor;
    }

    public void Pressed()
    {
        Debug.Log("Upgrade Pressed");
        if (GetPoints() >= cost)
        {
            SubtractPoints(cost);
            if (isPrestigeUpgrade) {

            } else if  (useNewSkillSystem) {
                WorldController.Instance.UpgradeSkill(skillType, value);
                IncreaseCost();
            } else {
                WorldController.Instance.increaseBaseClickRate(value);
                cost = Mathf.Round(cost * 1.3f * 10f) / 10f;
            }
            upgradeCount++;
            WorldController.Instance.purchaseSound.Play();

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
}
