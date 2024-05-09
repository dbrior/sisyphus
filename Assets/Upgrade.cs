using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private int upgradeCount;
    private WorldController controller;
    private bool hidden = true;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Main Grid").GetComponent<WorldController>();

        upgradeCostUI.text = "COST: " + cost.ToString();
        upgradeCountUI.text = upgradeCount.ToString();
        upgradeValueUI.text = "CPS: " + value.ToString();
    }

    // Update is called once per frame
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

            cost += cost;
            upgradeCostUI.text = "COST: " + cost.ToString();
            upgradeCountUI.text = upgradeCount.ToString();

            if (upgradeCount > 0)
            {
                upgradeCountObject.SetActive(true);
            }
        }
    }

    public void increaseBaseClickRate(float amount, float cost)
    {
        
    }
}
