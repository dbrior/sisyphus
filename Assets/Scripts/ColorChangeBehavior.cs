using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorChangeBehaviour : StateMachineBehaviour
{
    public Color colorOnEnter = Color.white;
    public Color colorOnExit = Color.white;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SpriteRenderer spriteRenderer = animator.GetComponent<SpriteRenderer>();
        Light2D light = animator.GetComponent<Light2D>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorOnEnter;
        }
        if (light != null)
        {
            light.color = colorOnEnter;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if (spriteRenderer != null)
        // {
        //     spriteRenderer.color = colorOnEnter;
        // }
        // if (light != null)
        // {
        //     light.color = colorOnEnter;
        // }
    }
}
