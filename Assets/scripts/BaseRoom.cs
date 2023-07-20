using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


//the base class that room objects derive from
//rooms that inherit this class will be compatible with transitioning behavior

//will need to make it so room gen can inherit this class with ease
namespace topdown{
    public abstract class BaseRoom : MonoBehaviour
    {
        [SerializeField]
        private TimelineAsset RoomStartTransitionTimeline;
        public TimelineAsset GetTransitionTimeline { get => RoomStartTransitionTimeline; }
        public static event Action<BaseRoom> OnRoomActive;
        Vector3 OriginalPos;
        [SerializeField] protected LevelChangeInteract[] Doors;


        //each room will do somethine different when transition finishes
        public abstract void OnTransitionFinish();

        public abstract Transform GetDoorFromDirection(DoorDirection doorDirection);

        public virtual void SetRoomActive()
        {
            gameObject.SetActive(true);
            OnRoomActive?.Invoke(this);
        }

        public virtual void MoveRoomToPosition(Vector3 MovePos)
        {
            OriginalPos = transform.position;
            transform.position = MovePos;
        }

        public virtual void SetRoomInactive()
        {
            print("deactivating " + gameObject);
            transform.position = OriginalPos;
            gameObject.SetActive(false);
        }
    }
}
