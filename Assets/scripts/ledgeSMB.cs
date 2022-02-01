using UnityEngine;
using UnityEngine.Animations;

namespace topdown
{
    public class ledgeSMB : SceneLinkedSMB<player>
    {
        float timer = 0;
        Vector3 LedgePos;
        Vector3 startpos;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);
            timer = 0;
            m_MonoBehaviour.OnLedgeEnter();
            m_MonoBehaviour.OnLedge();
            LedgePos = m_MonoBehaviour.ledgeref.transform.position;
            startpos = m_MonoBehaviour.transform.position;
        }



        public override void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //move player to the ledge
            base.OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex);
            m_MonoBehaviour.MoveInSeconds(ref timer, startpos, LedgePos, .25f);
           // Debug.Log("transition length is :" + stateInfo.length + "state is " + stateInfo.GetType());
        }

        //reset timer when on ledge
        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
            timer = 0;
            //UITextNotificationController.instance.gameObject.SetActive(true);
            Debug.Log(" text notif current cancel button is " + UITextNotificationController.instance.currentCancelButton);
            UITextNotificationController.instance.UpdateText("press " + UITextNotificationController.instance.currentCancelButton + " to descend");
        }


        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //move player to ledge
            //m_MonoBehaviour.GoToLedge();


            //while on ledge switch ledget targets when directions are pressed

            //if on ledge and press attack trigger ledge attack or get cancel ledge
            m_MonoBehaviour.LedgeInput();

        }
        


            //move player toward desired vector position
        public override void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            //go to desired position within the duration of the animation transition
            
            //Debug.Log("transitin duration is " + stateInfo.length);
            m_MonoBehaviour.MoveInSeconds(ref timer ,LedgePos, m_MonoBehaviour.descenddir , stateInfo.length);
            //Debug.Log("calling transition from state update");
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);
            m_MonoBehaviour.Offledge();
        }


    }
}
