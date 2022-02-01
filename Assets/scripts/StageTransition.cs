using UnityEngine;



public class StageTransition : MonoBehaviour, IRoomTransitioner
{


    public virtual void DoRoomTransition()
    {
        print("base room transition");
    }
}


