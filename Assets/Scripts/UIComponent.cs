using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public void ToggleUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
