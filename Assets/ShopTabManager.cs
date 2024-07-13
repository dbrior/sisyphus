using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTabManager : MonoBehaviour
{
    [SerializeField]
    private Color activeTextColor;
    [SerializeField]
    private Color disabledTextColor;
    [SerializeField]
    private Color activeBackgroundColor;
    [SerializeField]
    private Color disabledBackgroundColor;
    [SerializeField]
    private List<ShopTab> tabs;
    [SerializeField]
    private Shop shopRoot;

    private void SetActiveTab(ShopTab shopTab) {
        shopRoot.SetShopType(shopTab.GetShopType());
        shopTab.SetColors(activeTextColor, activeBackgroundColor);
        foreach (ShopTab tab in tabs) {
            if (tab == shopTab) {
                continue;
            }
            tab.SetColors(disabledTextColor, disabledBackgroundColor);
        }
    }

    public ShopTab GetTabForShopType(ShopType shopType) {
        foreach (ShopTab tab in tabs) {
            if (tab.GetShopType() == shopType) {
                return tab;
            }
        }
        return null;
    }

    public void ChangeTabRequest(ShopTab incomingTab) {
        SetActiveTab(incomingTab);
    }

    void Start() {
        SetActiveTab(GetTabForShopType(ShopType.AutoClick));
    }

}
