using UnityEngine;
using topdown;
using UnityEngine.Animations;

public class StunSMB : SceneLinkedSMB<Rusher>
{
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnSLStatePreExit(animator, stateInfo, layerIndex, controller);
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex, controller);
        m_MonoBehaviour.IsStunned = false;
        Debug.Log("enemy recovered from stun");
    }
}
