using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float pointValue = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        WorldController.Instance.gemCollectSound.Play();
        WorldController.Instance.points += pointValue;
        WorldController.Instance.AddPointTextSpawn(transform.position, pointValue.ToString(), new Color(43f/255, 1f, 0f), 1.75f, 2f);
        Destroy(gameObject);
    }
}
