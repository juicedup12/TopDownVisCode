using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class RusherPursue : SceneLinkedSMB<Rusher>
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
            //m_MonoBehaviour.OrientToPlayer();

            if (m_MonoBehaviour.OnlyDistance() <= distancetoTrigger)
            {
                animator.SetTrigger("charge");
                m_MonoBehaviour.charge();
                Debug.Log("ready to charge");
            }

        }
    }
}
