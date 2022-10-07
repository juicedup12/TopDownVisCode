using UnityEngine;
using UnityEngine.Animations;
using System;

namespace topdown
{
    public class WalkSMB : SceneLinkedSMB<player>
    {
        public static event Action OnWalk;
        public static event Action OnBusy;

        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
            //m_MonoBehaviour.EnableHurtBox();
            m_MonoBehaviour.canmove = true;
            m_MonoBehaviour.DoStamRegain(true);
            if (OnWalk != null)
                OnWalk();
            else Debug.Log("no walk action");
        }
        

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!m_MonoBehaviour.InDialogue )
            {
                m_MonoBehaviour.MovementInput();
                m_MonoBehaviour.MoveLookAhead();


                //check for mouse down
                if (m_MonoBehaviour.Attacking)
                {
                    animator.SetTrigger("ATK");
                }

                if (m_MonoBehaviour.CanDash())
                {
                    animator.SetTrigger("dash");
                }

                if (m_MonoBehaviour.CanHoldDash())
                {
                    animator.SetTrigger("holdDash");
                }

                if(m_MonoBehaviour.Parry)
                {
                    animator.SetTrigger("parry");
                }

                if(m_MonoBehaviour.OpenInventory)
                {
                    animator.SetTrigger("Inventory");
                    m_MonoBehaviour.OpenInventory = false;
                }

                if (m_MonoBehaviour.ledgecheck())
                {
                    animator.SetTrigger("ledge");
                }


            }
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);
            if(OnBusy != null)
            {
                OnBusy();
            }
        }


    }
}