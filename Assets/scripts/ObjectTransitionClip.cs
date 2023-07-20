using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectTransitionClip : PlayableAsset
{
    public GroupTransition transitioner;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ObjectTransitionBehavior>.Create(graph);

        var ObjTransition = playable.GetBehaviour();
        ////pass values to obj transition here
        Debug.Log("owner is : " + owner.name);
        Debug.Log("playable 0 is " + graph.GetOutput(0));

        for (int i = 0; i < graph.GetOutputCount(); i++)
        {
            transitioner = graph.GetOutput(i).GetUserData() as GroupTransition;
        }
        //Debug.Log(transitioner ? "length is " + transitioner.TileLength : "no transtioner");
        return playable;

    }
}
