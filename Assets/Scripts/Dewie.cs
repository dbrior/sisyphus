using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.Animations;
using UnityEngine.Rendering.Universal;

public class Dewie : MonoBehaviour
{
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController runningController;
    public Color color = new Color(1f, 1f, 1f); // White
    public float baseGlowIntensity = 0.0f;
    public Light2D dewieGlow;
    public bool wander = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public void UpdateColor(Color newColor)
    {
        color = newColor;
        dewieGlow.color = color;
        spriteRenderer.color = color;   
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        dewieGlow.color = color;
        spriteRenderer.color = color;

        if (wander)
        {
            spriteRenderer.sortingOrder = 10;
        }
    }

    void FixedUpdate()
    {
        animator.speed = WorldController.Instance.animationSpeed;
        if (WorldController.Instance.isMoving) {
            animator.runtimeAnimatorController = runningController;
        } else {
            animator.runtimeAnimatorController = idleController;
        }
        dewieGlow.intensity = ((WorldController.Instance.rawClickRate / 25.0f) * 2) + baseGlowIntensity;
    }
}
