using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sisyphus : MonoBehaviour
{
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
    public void Jump() {
        WorldController.Instance.boulder_b.Jump();
    }
    public void Kick() {
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
    }
}
