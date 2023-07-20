using UnityEngine;
using topdown;

public class ThrowSMB : SceneLinkedSMB<player>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);
        Debug.Log("entering throw smb");
        animator.speed = 0;
        //m_MonoBehaviour.ReleaseAttack = false;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        //update look input
        m_MonoBehaviour.MoveLookAhead();
        m_MonoBehaviour.AimThrow();


        //if(m_MonoBehaviour.ReleaseAttack)
        //{
        //    animator.speed = 1;
        //    Debug.Log("released attack");
        //}
    }


    //may have to substitute for animation event;
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);
        Debug.Log("pre exit throw smb");
        m_MonoBehaviour.inventory.ThrowItem();
    }
}
