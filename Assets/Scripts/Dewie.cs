using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.Rendering.Universal;

public class Dewie : MonoBehaviour
{
    public AnimatorController idleController;
    public AnimatorController runningController;
    public Light2D dewieGlow;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.speed = WorldController.Instance.animationSpeed;
        if (WorldController.Instance.isMoving) {
            animator.runtimeAnimatorController = runningController;
        } else {
            animator.runtimeAnimatorController = idleController;
        }
        dewieGlow.intensity = (WorldController.Instance.rawClickRate / 25.0f) * 2;
    }
}
