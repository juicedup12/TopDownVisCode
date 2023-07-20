using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for activating room the player is going to
//and deactivating room the player was in
//called by timeline and door classes
namespace topdown
{
    public class RoomActivator : MonoBehaviour
    {
        private BaseRoom CurrentRoom;
        private BaseRoom DestinationRoom;
        [SerializeField] Vector2 RoomSpawnPos;
        [SerializeField] player PlayerCharacter;
        DoorDirection doorDirection;
        public DoorDirection SetDoorDir { set => doorDirection = value; }


        private void OnEnable()
        {
            LevelChangeInteract.OnLevelChange += LevelChangeInteract_OnLevelChange;
        }

        private void OnDisable()
        {
            LevelChangeInteract.OnLevelChange -= LevelChangeInteract_OnLevelChange;
        }

        //can probably seperate this logic to a seperate script that tells room activator what to do
        private void LevelChangeInteract_OnLevelChange(LevelChangeInteract obj)
        {
            SetDestination(obj.GetDestinationRoom);
            doorDirection = obj.GetDoorDirection;
        }

        //will be called by timeline or other scripts
        //rework needed later line27
        public void ActivateCurrentRoom()
        {
            if(CurrentRoom)
            {
                CurrentRoom.SetRoomInactive();
            }

            DestinationRoom.MoveRoomToPosition(RoomSpawnPos);
            DestinationRoom.SetRoomActive();
            //will need to rework to place player at door
            print("door direction is " + doorDirection);
            //may have to change this later in case we want to transport player to a room  without spawning them on a door
            //change how playersetpos is set, with it setting on onlevelchange when door interacted
            Vector3 PlayerSetPos = DestinationRoom.GetDoorFromDirection(doorDirection).position;
            print("player set pos is " + PlayerSetPos);
            PlayerCharacter.SetPosAndWalkDir(PlayerSetPos, DoorUtilities.VectorFromDirection(doorDirection));
            PlayerCharacter.SetUIControls(true);
            CurrentRoom = DestinationRoom;
        }

        //called by door interact or starter script to set starting level
        //later the destination needs to be the door class itself
        //and activate the room through the door class
        public void SetDestination(BaseRoom room)
        {
            print("setting room activator destination to " + room);
            DestinationRoom = room;
        }

    }
}
