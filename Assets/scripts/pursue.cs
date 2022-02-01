using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class pursue : SceneLinkedSMB<Brute>
    {

        //add a low range float and long range float

        public float m_offset;
        public float longrange;
        public float closerange;
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            m_MonoBehaviour.targetplayer();
            m_MonoBehaviour.offset = m_offset;
            m_MonoBehaviour.offsetSavedValue = m_offset;
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            m_MonoBehaviour.targetplayer();


            if (m_MonoBehaviour.OnlyDistance() < closerange)
            {
                animator.SetTrigger("ShortrangeMelee");
                m_MonoBehaviour.swing();
                m_MonoBehaviour.StopAllCoroutines();
                //CodeMonkey.CMDebug.TextPopup("closerange attack", Vector3.zero);
            }

           
            else if (m_MonoBehaviour.AtDistance(longrange))
            {
                animator.SetTrigger("charge");
                m_MonoBehaviour.charge();
                //Debug.Log("ready to charge");
            }

        }
    }
}
