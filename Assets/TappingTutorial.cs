using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TappingTutorial : MonoBehaviour
{
    public Animator fingerAnimator;
    public void Activate()
    {
        transform.eulerAngles = Vector3.up;
        WorldController.Instance.Freeze();
        fingerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
}
