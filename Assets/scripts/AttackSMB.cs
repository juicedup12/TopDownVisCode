using UnityEngine;

namespace topdown
{
    public class AttackSMB : SceneLinkedSMB<player>
    {
        AnimatorClipInfo[] m_CurrentClipInfo;
        string[] m_ClipName;

        public float ComboInputWindow;
        float timer;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timer = 0;
            base.OnSLStateEnter(animator, stateInfo, layerIndex);
            //m_MonoBehaviour.Attacking = false;
            m_MonoBehaviour.move = Vector3.zero;
            //m_MonoBehaviour.canmove = false;
            AnimatorStateInfo animinfo = animator.GetCurrentAnimatorStateInfo(0);


            //Fetch the current Animation clip information for the base layer
            m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

            //Access the Animation clip name
            m_ClipName = new string[m_CurrentClipInfo.Length];
            for (int i = 0; i < m_CurrentClipInfo.Length; i++)
            {
                m_ClipName[i] = m_CurrentClipInfo[i].clip.name;
                //Debug.Log(i + "clip name is " + m_CurrentClipInfo[i].clip.name);
            }
            m_MonoBehaviour.swoosh.SetActive(true);

            //invoke an event
            m_MonoBehaviour.FinishedAction(0);

        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //timer += .01f;
            //if (m_MonoBehaviour.Attacking)
            //{
            //    if (timer > ComboInputWindow)
            //    {
            //        animator.SetTrigger("Punch2");
            //        Debug.Log("hit during comobo windowtimer is at " + timer);
            //        m_MonoBehaviour.swoosh.SetActive(true);
            //        m_MonoBehaviour.Attacking = false;
            //    }
            //    else if (timer < ComboInputWindow)
            //    {
            //        Debug.Log("combo window missed, timer is at " + timer);
            //        m_MonoBehaviour.Attacking = false;
            //    }
            //}
        }

        public override void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);
        }
    }
}
