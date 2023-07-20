using UnityEngine;
using topdown;
using UnityEngine.Animations;

public class ParrySMB : SceneLinkedSMB<player>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
        //
    }


    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {

        base.OnSLStateExit(animator, stateInfo, layerIndex, controller);
        m_MonoBehaviour.ActivateParry(false);
        m_MonoBehaviour.EnableHurtBox();
        m_MonoBehaviour.FinishedAction(2);
    }
}
