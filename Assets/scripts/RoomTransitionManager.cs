using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace topdown 
{

    //handles timeline management
    public class RoomTransitionManager : MonoBehaviour
    {
        [SerializeField] PlayableDirector RoomExitTimeline;
        [SerializeField] PlayableDirector RoomControlTimeline;
        private BaseRoom CurrentRoom;


        private void OnEnable()
        {
            BaseRoom.OnRoomActive += BaseRoom_OnRoomActive;
            LevelChangeInteract.OnLevelChange += LevelChangeInteract_OnLevelChange;
            RoomControlTimeline.stopped += RoomControlTimeline_stopped;
        }

        private void OnDisable()
        {
            RoomControlTimeline.stopped -= RoomControlTimeline_stopped;
            LevelChangeInteract.OnLevelChange -= LevelChangeInteract_OnLevelChange;
            BaseRoom.OnRoomActive -= BaseRoom_OnRoomActive;
        }

        private void BaseRoom_OnRoomActive(BaseRoom obj)
        {
            print("getting timeline asset from " + obj);
            RoomControlTimeline.Play(obj.GetTransitionTimeline);
            CurrentRoom = obj;
        }

        private void LevelChangeInteract_OnLevelChange(LevelChangeInteract levelChangeInteract)
        {
            RoomExitTimeline.Play();
        }

        //used for executing 
        private void RoomControlTimeline_stopped(PlayableDirector obj)
        {
             print("room " + CurrentRoom + " is finished setting up");
            CurrentRoom.OnTransitionFinish();
        }




        //called by door interact
        public void ExitTransition(RoomGen Destination, RoomGen FromRoom)
        {
            print("destination is " + Destination);
            RoomExitTimeline.Play();
        }

        //called by room gen when room is activated
        //necessary data will be retrieved by interface properties
        //need to move this all to room class
        //public void PlayRoomEnterTransition(ITileTransitionClient room)
        //{
        //    TileContainer.TileTransform[] tileTransforms = room.GetTilesToTransform;
        //    TileGroupTransition transition = null;
        //    if(tileTransforms != null)
        //    {
        //        //designate tile by type 
        //        TileGroupTransitionType transitionType = room.GetTileTransitionType;
        //        transition = tileTransitioner.DesignateTileGroupByType(transitionType);
        //    }
        //    if (!transition) return;
        //    transition.NewTiles(tileTransforms);
        //    print("room enter tile transition is " + transition);
        //    timeLineBinder.ChangeTransitionTrackBinding(transition);
        //    //access a property and assign instead
        //    SubRoomController.SetTimeControllers = room.GetTimeControllers;
        //    RoomControlTimeline.Play();
        //}

        //moved logic to room activator
        //will be called by the room exit timeline with signal
        public void ShowNextLevel()
        {
            print("changing level");
            //play the room change timeline
            //subscribe a method to the director when it's finished
            //when the the director is finished play the next room's timeline
            //CurrentRoom.DeactivateRoom();
            //ActivateRoom(Destination);
            //playerCharacter.WalkInDir(Destination.transform.position, Vector2.right);
        }

        //moved logic to roomactivator class
        //called by show next level and levelbuild test
        //public void ActivateRoom(RoomGen room)
        //{
        //    if (!room) { print("no room to set active"); return; }
        //    CurrentRoom = room;
        //    print("transition manager setting " + room.gameObject + " to active");
        //    room.SetRoomActive(LevelPlacement);
        //    //initiate tile transition manager
        //    //pass group transition to room control timeline
        //    RoomControlTimeline.Play();
        //}

        // Update is called once per frame
        void Update()
        {

        }



    }
}
