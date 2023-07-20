using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//deprecated
//need to find a way to make this compatible with other objects, like outer walls, props, etc
public class RandomAnimationProvider : MonoBehaviour
{
    [SerializeField] RandomAnimationsContainerSO SubroomAnimations;

    public static RandomAnimationProvider Instance;

    private void Awake()
    {

        Instance = this;
    }

    //need to move this code to the scripatable object later after testing to see if this works
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
