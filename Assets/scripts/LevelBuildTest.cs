using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the script that is incharge of telling level change manager the room to load first
//script will be 
namespace topdown
{
    public class LevelBuildTest : MonoBehaviour
    {
        [SerializeField] RoomActivator roomActivator;
        [SerializeField] BaseRoom RoomToActivate;

        private void Start()
        {
            roomActivator.SetDoorDir = DoorDirection.up;
            roomActivator.SetDestination(RoomToActivate);
            roomActivator.ActivateCurrentRoom();
            //test comment for git
        }
    }
}
