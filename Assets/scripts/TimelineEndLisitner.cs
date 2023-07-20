using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//a class for ensuring the assigned room will run 
//specified code once a provided timeline finishes playing
namespace topdown
{
    public class TimelineEndLisitner : MonoBehaviour
    {
        //target timeline will be retrieved from room gen through a property during settargetroom
        PlayableDirector TargetTimeline;
        RoomGen TargetRoom;


        private void OnEnable()
        {
            //RoomGen.OnRoomActive += SetTargetRoom;
            TargetTimeline.stopped += RoomControlTimeline_stopped;
        }
        private void OnDisable()
        {
            //RoomGen.OnRoomActive -= SetTargetRoom;
            TargetTimeline.stopped -= RoomControlTimeline_stopped;
        }

        private void RoomControlTimeline_stopped(PlayableDirector obj)
        {
            print("room " + TargetRoom + " is finished setting up");
        }


        private void SetTargetRoom(RoomGen room)
        {
            print("time line end listener setting room " + room + " as target");
           TargetRoom = room;
        }
    }
}
