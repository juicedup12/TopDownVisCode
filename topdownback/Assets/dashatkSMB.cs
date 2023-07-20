using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class dashatkSMB : SceneLinkedSMB<player>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.MovementInput();
            m_MonoBehaviour.dashATK();
            
            animator.SetTrigger("movement");
        }
    }
}
