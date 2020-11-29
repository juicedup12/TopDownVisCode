using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class checksoundSMB : SceneLinkedSMB<CopyUnit>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            //if arrived at sound
            if (m_MonoBehaviour.nearTarget())
            {
                m_MonoBehaviour.anim.SetTrigger("lookaround");
            }


            m_MonoBehaviour.searchforPlayer();

        }
    }
}