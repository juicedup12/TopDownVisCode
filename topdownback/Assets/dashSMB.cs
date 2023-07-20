using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class dashSMB : SceneLinkedSMB<player>
    {

        public override void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                animator.SetTrigger("dash");
            }



            if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("dashatk");
            }

                m_MonoBehaviour.MovementInput();
            m_MonoBehaviour.Dashinput();

            m_MonoBehaviour.handledash();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if space is let go
            if(!Input.GetKey(KeyCode.Space))
            {
                animator.SetTrigger("dash");
            }

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                m_MonoBehaviour.activatetrail();
                animator.SetTrigger("dashatk");
            }

            //for after image stuff
            m_MonoBehaviour.Dashinput();

            m_MonoBehaviour.handledash();
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.isdashing = false;
            m_MonoBehaviour.canmove = true;
        }
    }
}
