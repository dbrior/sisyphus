using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int type;
    public float value;
    public Sprite sprite;

    public Item(Sprite sprite, int type, float value)
    {
        this.type = type;
        this.value = value;
    }
}


public class LootManager : Singleton<LootManager>
{
    public List<Sprite> sprites;
    public List<float> values;
    public List<int> types;
    public List<Item> lootItems;

    void Start() {
        for (int i=0; i<sprites.Count; i++) {
            lootItems.Add(new Item(sprites[i], types[i], values[i]));
        }
    }

    public Item GetItem(int i) {
        return lootItems[i];
    }
}
