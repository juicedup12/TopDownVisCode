using System.Collections;
using System.Collections.Generic;
using topdown;
using UnityEngine;

public class AnimatorControllerOverride : MonoBehaviour, IAnimationInterpolate
{
    [SerializeField] string AnimationParameter;
    private Animator targetAnimator;
    GameObject target;
    public GameObject InterpolateTarget { set => InitializeTarget(value); }

    void InitializeTarget(GameObject target)
    {
        print(gameObject + " initializing " + target);
        this.target = target;
        targetAnimator = target.GetComponent<Animator>();
    }


    public void Interpolate(float value)
    {
        print("interpolating animator with value of " + value);
        targetAnimator.SetFloat(AnimationParameter, value);
    }
}
