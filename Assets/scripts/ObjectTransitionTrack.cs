using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackClipType(typeof(ObjectTransitionClip))]
[TrackBindingType(typeof(TileGroupTransition))]
public class ObjectTransitionTrack : TrackAsset
{
    protected override void OnCreateClip(TimelineClip clip)
    {
        base.OnCreateClip(clip);
        foreach(PlayableBinding binding in outputs)
        {
            Debug.Log("binding name " + binding.streamName);
            Debug.Log("type " + binding.outputTargetType);
            TileGroupTransition group = binding.sourceObject as TileGroupTransition;
            if (group)
                Debug.Log("group length is " + group.TileLength);
            else Debug.Log("no group");
        }
        foreach (PlayableBinding binding in parent.outputs)
        {
            Debug.Log("parent binding name is " + binding.streamName);
            TileGroupTransition group = binding.sourceObject as TileGroupTransition;
            if (group)
                Debug.Log("group length is " + group.TileLength);
            else Debug.Log("no group in parent");
        }
        
    }
}
