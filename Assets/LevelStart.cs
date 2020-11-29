using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class LevelStart : SceneLinkedSMB<player>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool NormalTimeScale = true;

            //if level is being built speed it up on shift press
            //if(Input.GetKeyDown(KeyCode.LeftShift) && !m_MonoBehaviour.SequenceDone)
            //{
            //    Time.timeScale = 2;
            //    NormalTimeScale = false;
            //}
            //if(Input.GetKeyUp(KeyCode.LeftShift) || m_MonoBehaviour.SequenceDone && !NormalTimeScale)
            //{
            //    Time.timeScale = 1;
            //    Debug.Log("timescale is normal");
            //    NormalTimeScale = true;
            //}

            //if player is waiting to go into level
            if(m_MonoBehaviour.PressedAccept && m_MonoBehaviour.SequenceDone)
            {
                m_MonoBehaviour.WalkIntoLevel();
            }
        }
    }
}
