using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopTab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tabLabel;
    [SerializeField] private Image tabBackground;
    [SerializeField] private ShopType shopType;
    private bool toggled = false;

    public void SetColors(Color textColor, Color backgroundColor) {
        tabLabel.color = textColor;
        tabBackground.color = backgroundColor;
    }

    public void Pressed() {
        transform.parent.gameObject.GetComponent<ShopTabManager>().ChangeTabRequest(this);
    }

    public ShopType GetShopType() {
        return shopType;
    }
 }
