using UnityEngine;

namespace topdown
{
    public class LevelStart : SceneLinkedSMB<player>
    {
        bool NormalTimeScale;
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);
            m_MonoBehaviour.setVelocity(true);
            //m_MonoBehaviour.canmove = false;
            NormalTimeScale = true;
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if (m_MonoBehaviour.PressedAccept && m_MonoBehaviour.SequenceDone)
            {
                //change to execute a callback
                m_MonoBehaviour.WalkIntoLevel();
            }
        }
        
    }
}
