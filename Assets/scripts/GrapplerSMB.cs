using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class GrapplerSMB : SceneLinkedSMB<Grappler>
    {
        public float m_offset;
        public float distancetoTrigger;
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            m_MonoBehaviour.targetplayer();
            m_MonoBehaviour.offset = m_offset;
            m_MonoBehaviour.offsetSavedValue = m_offset;

        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            m_MonoBehaviour.targetplayer();
            
            if (m_MonoBehaviour.AtDistance(distancetoTrigger))
            {
                m_MonoBehaviour.ThrowGrapple();
            }

        }
    }
}
