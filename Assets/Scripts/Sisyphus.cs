using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sisyphus : MonoBehaviour
{
    // Health
    private float maxHealth = 250f;
    private float currHealth;
    // Animations
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController pushingController;
    public RuntimeAnimatorController pushingBootsController;
    public RuntimeAnimatorController kickingController;

    // Parts
    public Animator glovesController;
    public float animationSpeed = 0f;
    public bool kicking = false;
    
    // Controllers
    private bool bootsMode;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public void Damage(float damage) {
        currHealth -= damage;
        HealthBarManager.Instance.UpdateHealthBar(gameObject, currHealth / maxHealth);

        if (currHealth <= 0) {
            WorldController.Instance.GameOver();
        }
    }

    public void StartFight() {
        currHealth = maxHealth;
        HealthBarManager.Instance.CreateHealthBar(gameObject);
    }

    public void EndFight() {
        HealthBarManager.Instance.DestroyHealthBar(gameObject);
    }

    public void Jump() {
        WorldController.Instance.boulder_b.Jump();
    }
    public void Kick() {
        if (WorldController.Instance.frozen && !WorldController.Instance.didJumpTutorial) {
            Time.timeScale = 1f;
            WorldController.Instance.frozen = false;
            WorldController.Instance.didJumpTutorial = true;
        }
        kicking = true;
        animator.speed = 1f;
        animator.runtimeAnimatorController = kickingController;
    }
    public void StopKick() {
        kicking = false;
    }

    public void SetAnimationSpeed(float newAnimationSpeed)
    {
        animationSpeed = newAnimationSpeed;
        animator.speed = animationSpeed;
        glovesController.speed = animationSpeed;
    }

    // Animation switching
    public void EnabledBootsMode()
    {
        bootsMode = true;
    }
    public void DisabledBootsMode()
    {
        bootsMode = false;
    }
    public void SetIdleState()
    {
        animator.runtimeAnimatorController = idleController;
        glovesController.gameObject.SetActive(false);
    }
    public void SetPushingState()
    {
        // glovesController.gameObject.SetActive(true);
        if (bootsMode) {
            animator.runtimeAnimatorController = pushingBootsController;
        } else {
            animator.runtimeAnimatorController = pushingController;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currHealth = maxHealth;
    }
}
