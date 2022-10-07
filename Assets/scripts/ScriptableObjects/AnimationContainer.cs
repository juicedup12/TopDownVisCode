using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimContainerObj", menuName = "ScriptableObjects/AnimContainer", order = 1)]
public class AnimationContainer : ScriptableObject
{
    public AnimationClip[] clips;

    public AnimationClip RetrieveRandomClip ()
    {
        int RandomClipIndex = Random.Range(0, clips.Length);
        Debug.Log("retrieving animation clip #" + RandomClipIndex + 1);
        return clips[RandomClipIndex];
    }
}
