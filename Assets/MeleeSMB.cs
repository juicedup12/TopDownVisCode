using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace topdown
{
    public class MeleeSMB : SceneLinkedSMB<player>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if(m_MonoBehaviour.IsAnamationFinished())
            {
                animator.SetTrigger("melee");
            }
        }
    }
}
