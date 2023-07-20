using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class HoldDash : SceneLinkedSMB<player>
    {

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);
            //show the dash indicator over the attack indicator
            m_MonoBehaviour.EnableHoldDashIndicator(true);
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);
            m_MonoBehaviour.MoveLookAhead();


            //if button is let go then go to dashatk
            //if(!m_MonoBehaviour.Dashing)
            //{
            //    m_MonoBehaviour.activatetrail();
            //    animator.SetTrigger("dashatk");
            //}
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex, controller);
            m_MonoBehaviour.EnableHoldDashIndicator(false);

        }

        
    }
}
