using System.Collections;
using UnityEngine;


public interface ISubRoomClient 
{
    AnimatorTimeController[] GetTimeControllers { get; }
}
