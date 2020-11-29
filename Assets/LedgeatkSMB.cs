using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class LedgeatkSMB : SceneLinkedSMB<player>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
           

            //if player reached enemy, then go back to walking
            if (m_MonoBehaviour.ledgeATK())
            {
                animator.SetTrigger("movement");
            }
        }
    }
}
