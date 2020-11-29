using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class WalkSMB : SceneLinkedSMB<player>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
                m_MonoBehaviour.MovementInput();
                //m_MonoBehaviour.UpdateRotation();
                m_MonoBehaviour.SlowmodeInput();


                //check for mouse down
                if (m_MonoBehaviour.Isattacking())
                {
                    animator.SetTrigger("ATK");
                }

                m_MonoBehaviour.IsPressingSpace();


                if (m_MonoBehaviour.ledgecheck())
                {
                    animator.SetTrigger("ledge");
               }
            
           

            //check if on ledge
        }

        
    }
}