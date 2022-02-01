using UnityEngine;



namespace topdown
{
    public class restrainedSMB : SceneLinkedSMB<player>
    {
        

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Breakfree();
        }


    }
}
