using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class ledgeSMB : SceneLinkedSMB<player>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.GoToLedge();


            //while on ledge switch ledget targets when directions are pressed

            //if on ledge and press attack trigger ledge attack or get cancel ledge
            m_MonoBehaviour.LedgeUpdate();

        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.deactivateIndicator();
        }


    }
}
