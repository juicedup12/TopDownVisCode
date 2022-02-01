using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class chase : SceneLinkedSMB<Shooter>
    {
        public float distToshoot;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            if(m_MonoBehaviour == null)
            {
                Debug.Log("No monobehavior");
            }
            m_MonoBehaviour.targetplayer();
            m_MonoBehaviour.offset = .8f;
            m_MonoBehaviour.offsetSavedValue = .8f;

        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            m_MonoBehaviour.targetplayer();

            m_MonoBehaviour.UpdateAimLaser(distToshoot);
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);
            m_MonoBehaviour.resetAimLine();
        }
    }
}
