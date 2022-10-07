using UnityEngine;



class StageTransition : MonoBehaviour, IRoomTransitioner
{


    public virtual void DoRoomTransition(iStageBuild builder)
    {
        print("base room transition");
    }
}


