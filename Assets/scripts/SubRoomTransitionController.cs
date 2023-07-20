using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using topdown;

/// <summary>
/// responsible for passing time values to SubRoom objects
/// </summary>
public class SubRoomTransitionController : MonoBehaviour , ITimeControl
{
    [SerializeField]
    AnimatorTimeController[] timeControllers;
    public AnimatorTimeController[] SetTimeControllers { set  { timeControllers = value; print("setting time controllers with value of " + value.Length); } }

    private void Awake()
    {
    }

    private void OnEnable()
    {
        BaseRoom.OnRoomActive += AssignTimeControllers;
    }

    private void OnDisable()
    {
        BaseRoom.OnRoomActive -= AssignTimeControllers;
    }


    void AssignTimeControllers(BaseRoom room)
    {
        print("subroom controller callback received from room activate");
        if (room is ISubroomTransitionDataProvider)
        {
            ISubroomTransitionDataProvider subroomTransitionDataProvider = room as ISubroomTransitionDataProvider;
            SetTimeControllers = subroomTransitionDataProvider.GetTimeControllers;
        }
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
        if (timeControllers == null) return;
        //do a foreach loop and pass in time for each animator time controller
        foreach (AnimatorTimeController item in timeControllers)
        {
            item.SetTime((float)time);
        }
        
    }



}
