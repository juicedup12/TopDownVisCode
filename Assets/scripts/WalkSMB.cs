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
            m_MonoBehaviour.DoStamRegain(true);
            if (OnWalk != null)
                OnWalk();
            else Debug.Log("no walk action");
        }
        

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            m_MonoBehaviour.MoveLookAhead();

            animator.SetFloat("DirX", m_MonoBehaviour.InputMove.x);
            animator.SetFloat("DirY", m_MonoBehaviour.InputMove.y);






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