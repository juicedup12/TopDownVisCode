using UnityEngine;
using topdown;
using UnityEngine.Animations;

public class InventoryCheckSMB : SceneLinkedSMB<PlayerInventoryController>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);

        //if (m_MonoBehaviour.items.Count < 1)
        //{
        //    Debug.Log("no items in inventory");
        //    animator.SetTrigger("Inventory");
        //}
        Debug.Log("opening inventory");
        m_MonoBehaviour.ShowInventory();

        

    }

    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStatePostEnter(animator, stateInfo, layerIndex);
        m_MonoBehaviour.isOpen = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);
        m_MonoBehaviour.InventorySelelct();
        
    }
}
