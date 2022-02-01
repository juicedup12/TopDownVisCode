using UnityEngine;



public class SelectionManager : MonoBehaviour
{
    ISelector selector;
    MonoBehaviour TargetedObj;
    void Start()
    {
        selector = GetComponent<ISelector>();
    }

    void DetectItems(MonoBehaviour ObjToFind)
    {
        TargetedObj = selector.GetSelection(gameObject.transform, ObjToFind);
    }
}
