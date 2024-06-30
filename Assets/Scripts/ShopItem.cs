using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public TextMesh buffText;
    private SpriteRenderer spriteRenderer;
    private Item item;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item newItem) {
        spriteRenderer = GetComponent<SpriteRenderer>();
        item = newItem;
        string buffString = WorldController.Instance.AbilityNameMappings[newItem.type];
        Color buffColor = WorldController.Instance.AbilityColorMappings[newItem.type];

        spriteRenderer.sprite = newItem.sprite;
        buffText.text = buffString;
        buffText.color = buffColor;
    }
}