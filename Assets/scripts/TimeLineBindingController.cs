using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;


public class TimeLineBindingController : MonoBehaviour
{
    [SerializeField]
    TileGroupRotate[] tileGroup;
    [SerializeField] 
    int GroupToBind;
    [SerializeField]
    PlayableDirector Timeline;

    // Start is called before the first frame update
    void Start()
    {
        AssignBind();
        Timeline.Play();
    }

    void AssignBind()
    {
        if(GroupToBind > tileGroup.Length)
        {
            return;
        }
        TimelineAsset TileGroupOutput = (TimelineAsset)Timeline.playableAsset;
        var track = (TrackAsset)TileGroupOutput.GetOutputTrack(0);
        Timeline.SetGenericBinding(track, tileGroup[GroupToBind]);
        //TileGroupOutput.SetUserData(tileGroup[GroupToBind]);

        //Timeline.SetGenericBinding(TileGroupOutput, tileGroup[GroupToBind]);

    }

}
