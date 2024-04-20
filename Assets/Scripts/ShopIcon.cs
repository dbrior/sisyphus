using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopIcon : MonoBehaviour
{
    public GameObject openIcon;
    public GameObject closedIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleIcon() {
        openIcon.SetActive(!openIcon.activeSelf);
        closedIcon.SetActive(!closedIcon.activeSelf);
    }
}
