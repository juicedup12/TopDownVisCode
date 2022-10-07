using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using topdown;

/// <summary>
/// responsible for passing time values to room objects
/// </summary>
public class RoomController : MonoBehaviour , ITimeControl
{

    AnimatorTimeController[] timeControllers;
    public static RoomController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void OnControlTimeStart()
    {
        print("started timeline control");
        //may need to do error checking for an array since
        //if this method is running there MUST be an array
    }

    public void OnControlTimeStop()
    {
        print("stopped timeline control");
        //may have to clear the controllers array
    }

    public void SetTime(double time)
    {
        //do a foreach loop and pass in time for each animator time controller
        if (timeControllers != null)
        {
            foreach (AnimatorTimeController item in timeControllers)
            {
                item.time = (float)time;
            }
        }
    }

    public void AssignTimeControllers(RoomGen room)
    {
        timeControllers = room.gameObject.GetComponentsInChildren<AnimatorTimeController>();
        print("assigned time controllers with a length of " + timeControllers.Length);
    }


}
