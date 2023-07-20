using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace topdown
{
    public class LevelChangeInteract : MonoBehaviour, Iinteractable
    {
        //[SerializeField] RoomChangeEvent changeEvent;

        [SerializeField] BaseRoom Destination;
        public BaseRoom GetDestinationRoom { get => Destination; }
        public static event Action<LevelChangeInteract> OnLevelChange;
        
        [SerializeField] DoorDirection doorDirection;
        public DoorDirection GetDoorDirection { get => doorDirection; }

        public void interact()
        {
            print("Unity event called");
            //changeEvent?.Invoke(Destination, GetComponentInParent<RoomGen>());
            //RoomTransitionManager.Instance.ExitTransition(Destination, GetComponentInParent<RoomGen>());
            OnLevelChange?.Invoke(this);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public enum DoorDirection {none, up, down, left, right }

}