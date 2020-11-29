using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class chase : SceneLinkedSMB<Shooter>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            m_MonoBehaviour.targetplayer();
            m_MonoBehaviour.offset = .8f;
            m_MonoBehaviour.offsetSavedValue = .8f;

        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            m_MonoBehaviour.targetplayer();
            
            if (m_MonoBehaviour.canShoot())
            {

                Debug.Log("firing");
            }
        }
    }
}
