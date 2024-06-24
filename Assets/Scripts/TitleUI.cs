using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
    private bool fired = false;


    void EnableUI()
    {
        for (int i=0; i<objectsToEnable.Length; i++)
        {
            objectsToEnable[i].SetActive(true);
        }
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }
    }

    void OnMouseDown() {
        if (!fired)
        {
            EnableUI();
            fired = true;
        }
    }

    void Update()
    {
        if (!fired && Input.touchCount > 0)
        {
            EnableUI();
            fired = true;
        }
    }
}
