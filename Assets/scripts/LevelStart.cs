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
            m_MonoBehaviour.canmove = false;
            NormalTimeScale = true;
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            

            //if level is being built speed it up on shift press
            if (m_MonoBehaviour.Dashing && !m_MonoBehaviour.SequenceDone && NormalTimeScale)
            {
                Time.timeScale = 2;
                NormalTimeScale = false;
            }
            if ( m_MonoBehaviour.SequenceDone && !NormalTimeScale)
            {
                Time.timeScale = 1;
                //Debug.Log("timescale is normal");
                NormalTimeScale = true;
            }

            //if player is waiting to go into level



            if (m_MonoBehaviour.PressedAccept && m_MonoBehaviour.SequenceDone)
            {
                m_MonoBehaviour.WalkIntoLevel();
            }
        }
        
    }
}
