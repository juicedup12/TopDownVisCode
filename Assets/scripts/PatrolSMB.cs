using UnityEngine;

namespace topdown
{
    public class PatrolSMB : SceneLinkedSMB<Enemy>
    {

        

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
            m_MonoBehaviour.offset = 0;
            m_MonoBehaviour.searchforPlayer();

            //after arriving at first node point increment 
           


            m_MonoBehaviour.orientToWaypoint();

            //if there's only one patrol point stay 
            if (m_MonoBehaviour.Patrolpoints.Length < 1)
            {
                //Debug.Log("less than 2 waypoints");
                return;
            }
            m_MonoBehaviour.patrolupdate();
        }
        

    }



}
