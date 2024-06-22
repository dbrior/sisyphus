using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpy : MonoBehaviour
{
    public Sprite hitHarpy;
    public SpriteRenderer harpyRenderer;
    public Animator harpyAnimator;
    private Cloud cloudScript;
    public Rigidbody2D harpySpriteRb;
    public LootBox lootboxPrefab;
    Collider2D collider2D;
    private bool triggered = false;
    public static Vector2 GetRandomForce(float minForce, float maxForce)
    {
        float angle = Random.Range(60f, 130f);

        float angleInRadians = angle * Mathf.Deg2Rad;

        float forceMagnitude = Random.Range(minForce, maxForce);

        float forceX = Mathf.Cos(angleInRadians) * forceMagnitude;
        float forceY = Mathf.Sin(angleInRadians) * forceMagnitude;

        return new Vector2(forceX, forceY);
    }

    void Start()
    {
        cloudScript = GetComponent<Cloud>();
        collider2D = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered) {
            LootBox lootbox = Instantiate(lootboxPrefab);
            lootbox.transform.position = transform.position;
            harpyAnimator.enabled = false;
            harpyRenderer.sprite = hitHarpy;
            harpySpriteRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            harpySpriteRb.AddForce(GetRandomForce(100f, 200f));
            cloudScript.enabled = false;
            collider2D.enabled = false;
        }
    }
}
