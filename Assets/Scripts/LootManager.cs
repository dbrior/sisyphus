using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    public List<Item> lootItems;

    public Item GetItem(int i) {
        return lootItems[i];
    }

    public Item GetRandomItem() {
        return lootItems[Random.Range(0, LootManager.Instance.lootItems.Count)];
    }
}
