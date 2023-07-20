using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class dashSMB : SceneLinkedSMB<player>
    {

        bool shortDash = false;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);
            m_MonoBehaviour.DashPressDuration = 0;
            m_MonoBehaviour.DisableHurtBox();
        }

        public override void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //m_MonoBehaviour.MovementInput();
            m_MonoBehaviour.Dashinput();

            //m_MonoBehaviour.handledash();
            
        }

        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStatePostEnter(animator, stateInfo, layerIndex);
            m_MonoBehaviour.EnableHurtBox();
            m_MonoBehaviour.DoStamRegain(false);
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if space is let go
            Debug.Log("current dash dur in state update is " + m_MonoBehaviour.DashPressDuration);
            if (m_MonoBehaviour.DashPressDuration <= 1.1 && m_MonoBehaviour.DashPressDuration > 0)
            {
                animator.SetTrigger("ShortDash");
                Debug.Log("ShortDash");
                shortDash = true;
            }
            else
            {
                animator.SetTrigger("dash");
                shortDash = false;

            }

            //need to use animator state machine to switch to attacking or a different type of attack
            //if (m_MonoBehaviour.Attacking)
            //{
            //    Debug.Log("attacking while dashing");
            //    //m_MonoBehaviour.activatetrail();
            //    animator.SetTrigger("ATK");
            //}

            //for after image stuff
            m_MonoBehaviour.Dashinput();

            m_MonoBehaviour.handledash();
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if(shortDash)
            {
                m_MonoBehaviour.ReduceStamina(15);
            }
            else
            {
                m_MonoBehaviour.ReduceStamina(30);
            }
            //m_MonoBehaviour.FinishedAction(1);
        }

        public override void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {


            //if (m_MonoBehaviour.Attacking)
            //{
            //    //Debug.Log("attacking on trasition from state update");
            //    animator.SetTrigger("ATK");
            //}

            base.OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex, controller);
            //Debug.Log("transition from state update");
            m_MonoBehaviour.Dashinput();

            m_MonoBehaviour.handledash();
        }

        
    }

    

}
