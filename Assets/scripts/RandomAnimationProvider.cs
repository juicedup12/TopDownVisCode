using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomAnimationProvider : MonoBehaviour
{
    [SerializeField] AnimationContainer SubroomAnimations;

    public static RandomAnimationProvider Instance;

    private void Awake()
    {

        Instance = this;
    }

    public void AssignRandomSubRoomAnimation(GameObject[] RoomObjects)
    {
        if (RoomObjects.Length < 1) return;
        AnimationClip RandomDoorClip = SubroomAnimations.RetrieveRandomClip();
        print("assigning animation to " + RoomObjects.Length + " subrooms");
        foreach(GameObject roomobj in RoomObjects)
        {
            roomobj.GetComponent<AnimatorTimeController>().clip = RandomDoorClip;
        }    
    }
}
