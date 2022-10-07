using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectTransitionBehavior : PlayableBehaviour
{
    float duration = 0;
    public override void OnGraphStart(Playable playable)
    {
        Debug.Log("duration is " + playable.GetDuration());
        duration = (float)playable.GetDuration();
        //Debug.Log("behavior playable has " + playable.GetOutputCount() + "outputs");
        //for (int i = 0; i < playable.GetOutputCount(); i++)
        //{
        //    PlayableGraph graph = playable.GetOutput(i).GetGraph();
        //    Debug.Log("behavior graph has " + graph.GetOutputCount() + " outputs ");
        //    for (int o = 0; i < graph.GetOutputCount(); o++)
        //    {

        //        var Userdata = graph.GetOutput(o).GetUserData();
        //        if (Userdata is TileGroupRotate)
        //        {
        //            TileGroupRotate group = Userdata as TileGroupRotate;
        //            if (group)
        //            {
        //                Debug.Log("group length is " + group.TileLength);
        //                if (group is TileGroupRotate)
        //                {
        //                    TileGroupRotate tile = (TileGroupRotate)group;
        //                    tile.SetTilesStartRot();

        //                }
        //            }
        //        }
        //        else Debug.Log("behavior has no group");
                
        //    }
        //}
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (playerData == null) return;
        TileGroupTransition group = playerData as TileGroupTransition;
        float time = (float)playable.GetTime();
        group.LerpObjects(duration, time);
    }

}
