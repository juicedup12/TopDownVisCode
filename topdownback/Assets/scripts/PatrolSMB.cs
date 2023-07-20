using System.Collections;
using System.Collections.Generic;
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
            m_MonoBehaviour.patrolupdate();
                        
            //m_MonoBehaviour.orientToWaypoint();
            
            //if (m_MonoBehaviour.path.Length > 0)
            //{
            //    m_MonoBehaviour.facetarget(m_MonoBehaviour.path[m_MonoBehaviour.path.Length - 1]);
            //}
            //else
            //{
            //   m_MonoBehaviour.facetarget(m_MonoBehaviour.path[0]);
            //}



            //if (m_MonoBehaviour.path[0] != null)
            //{

            //    m_MonoBehaviour.facetarget(m_MonoBehaviour.path[0]);
            //}

            ////if the enemy is going to sound check if it's near target from pathfinding
            ////after arriving at target wait for a certain amount of time and turn going to sound false
            //if (isgoingtosound && nearobj(pathfinding.target) && timeron == false)
            //{
            //    Debug.Log("is near sound");
            //    //wait a few seconds
            //    //go back to patrol
            //    timer = waittime + Time.time;
            //    enemypathfind.isidle = true;
            //    timeron = true;
            //}

            //if (timer <= Time.time && timer != 0 && timeron)
            //{
            //    Debug.Log("stopped looking for sound");
            //    enemypathfind.isidle = false;
            //    ischasingplayer = false;
            //    isgoingtosound = false;
            //    pathfinding.target = Patrolpoints[AtTarget - 1];
            //    timeron = false;
            //}
        }


    }



}
