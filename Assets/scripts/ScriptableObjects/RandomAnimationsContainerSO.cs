using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimContainerObj", menuName = "ScriptableObjects/AnimContainer", order = 1)]
public class RandomAnimationsContainerSO : ScriptableObject
{
    public AnimationClip[] clips;

    public AnimationClip RetrieveRandomClip ()
    {
        int RandomClipIndex = Random.Range(0, clips.Length);
        Debug.Log("retrieving animation clip #" + RandomClipIndex + 1);
        return clips[RandomClipIndex];
    }


    public void AssignRandomSubRoomAnimation(GameObject[] RoomObjects)
    {
        if (RoomObjects.Length < 1) return;
        AnimationClip RandomDoorClip = RetrieveRandomClip();
        Debug.Log("assigning animation to " + RoomObjects.Length + " subrooms");
        foreach (GameObject roomobj in RoomObjects)
        {
            roomobj.GetComponent<AnimatorTimeController>().clip = RandomDoorClip;
        }
    }
}
