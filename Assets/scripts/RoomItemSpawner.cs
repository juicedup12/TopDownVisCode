using UnityEngine;


//roomitemspawner is for items that activate a specific script in the 
//room that the item spawned it
namespace topdown
{
    public class RoomItemSpawner : Item
    {
        public RoomGen roomref;
        public GameObject ObjToInstantiate;
        
        public override void Start()
        {
            base.Start();
            roomref = GetComponentInParent<RoomGen>();

        }

    }
}
