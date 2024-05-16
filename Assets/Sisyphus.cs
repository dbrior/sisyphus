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
    public float animationSpeed = 0f;
    
    // Controllers
    private bool bootsMode;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

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
    }
    public void SetPushingState()
    {
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

    void Update()
    {
        animator.speed = animationSpeed;   
    }
}
