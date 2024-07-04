using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    enum State {Waiting, Latched, Finished}
    private State state;

    public GameObject shop;
    
    public float spawnY;
    public float durationSeconds;
    private float lifetime = 0f;

    private RandomBobbing bobbing;
    private Animator animator;

    // For moving with platform
    private float lastDistance = 0f;

    // Items
    public List<ShopItem> itemSlots = new List<ShopItem>();
    
    void SwitchState(State newState) {
        switch (newState) {
            case State.Latched:
                transform.SetParent(WorldController.Instance.transform);
                shop.SetActive(true);
                bobbing.startBobbing = true;
                animator.enabled = true;
                break;
            case State.Finished:
                bobbing.startBobbing = false;
                shop.SetActive(false);
                transform.SetParent(WorldController.Instance.platformA);
                animator.enabled = false;
                break;
        }
        state = newState;
    }
    void Start() {
        state = State.Waiting;
        bobbing = GetComponent<RandomBobbing>();
        animator = GetComponent<Animator>();

        animator.enabled = false;
        // bobbing.centerPosition = new Vector3(WorldController.Instance.boulder.transform.position.x+0.6f, transform.position.y, 0);
        for (int i=0; i<3; i++) {
            itemSlots[i].SetItem(LootManager.Instance.GetRandomItem());
        }
    }
    void Update()
    {
        switch (state) {
            case State.Waiting:
                if (transform.position.x < 0) {
                    SwitchState(State.Latched);
                }
                break;
            case State.Latched:
                lifetime += Time.deltaTime;
                if (lifetime > durationSeconds) {
                    SwitchState(State.Finished);
                }
                break;
            case State.Finished: break;
        }
    }
}
