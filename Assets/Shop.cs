using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ShopType {
    AutoClick,
    ManualClick,
    KeyItems
}
[System.Serializable] 
public class ShopInventoryEntry {
    public ShopType Key;
    public GameObject Value;
}

public class Shop : MonoBehaviour
{

    [SerializeField]
    private List<ShopInventoryEntry> shopInventoriesList = new List<ShopInventoryEntry>();

    private Dictionary<ShopType, GameObject> shopInventories;

    private ShopType currShopType;
    private GameObject currShopInventory;

    private void LoadInventory(ShopType shopType) {
        currShopInventory = shopInventories[shopType];
        RefreshInventories();
    }

    private void RefreshInventories() {
        foreach (GameObject shopIntentory in shopInventories.Values) {
            shopIntentory.SetActive(false);
        }
        currShopInventory.SetActive(true);
    }

    public void SetShopType(ShopType newShopType) {
        currShopType = newShopType;
        LoadInventory(currShopType);
    }

    void Awake()
    {
        shopInventories = new Dictionary<ShopType, GameObject>();
        foreach (var entry in shopInventoriesList)
        {
            shopInventories[entry.Key] = entry.Value;
        }
    }
}
