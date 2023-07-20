using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown 
{
    public static class DoorUtilities
    {
        //will look through a collection of 4 doors
        //and given a door direction it will retrieve the one facing opposite of that direction
        public static Transform RetrieveNeighborDoor(DoorDirection direction, LevelChangeInteract[] doorCollection)
        {
            DoorDirection DesiredDoorDirection = DoorDirection.none;
            switch (direction)
            {
                case DoorDirection.up:
                    DesiredDoorDirection = DoorDirection.down;
                    break;
                case DoorDirection.down:
                    DesiredDoorDirection = DoorDirection.up;
                    break;
                case DoorDirection.left:
                    DesiredDoorDirection = DoorDirection.right;
                    break;
                case DoorDirection.right:
                    DesiredDoorDirection = DoorDirection.left;
                    break;
                default:
                    break;
            }

            foreach (LevelChangeInteract door in doorCollection)
            {
                if(door.GetDoorDirection == DesiredDoorDirection)
                {
                    Debug.Log("returning " + door + " from retrieve neighboor");
                    return door.transform;
                }
            }
            return null;
        }


        public static Vector3 VectorFromDirection(DoorDirection doorDirection)
        {
            switch (doorDirection)
            {
                case DoorDirection.none:
                    Debug.Log("error, no drection to get");
                    break;
                case DoorDirection.up:
                    return Vector3.up;
                case DoorDirection.down:
                    return Vector3.down;
                case DoorDirection.left:
                    return Vector3.left;
                case DoorDirection.right:
                    return Vector3.right;
                default:
                    break;
            }
            return Vector3.zero;
        }
    }
}
