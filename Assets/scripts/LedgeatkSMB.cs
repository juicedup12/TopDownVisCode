using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class LedgeatkSMB : SceneLinkedSMB<player>
    {
        float timer = 0;
        Vector3 LedgePos;
        Transform enemytransform;


        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);
            timer = 0;
            LedgePos = m_MonoBehaviour.ledgeref.transform.position;
            enemytransform = m_MonoBehaviour.ledgeref.currentEnemy;
            Debug.Log("ledge pos is " + LedgePos);

        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {

            m_MonoBehaviour.MoveInSeconds(ref timer, LedgePos, enemytransform.position , stateInfo.length);

        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);
            m_MonoBehaviour.ledgeref.currentEnemy.GetComponent<Enemy>().Die(m_MonoBehaviour.transform.position);
        }
    }
}
