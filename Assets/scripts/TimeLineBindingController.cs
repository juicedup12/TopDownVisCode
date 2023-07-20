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
    PlayableDirector RoomCotnrolTimeline;

    // Start is called before the first frame update
    void Start()
    {
        //AssignBind();
        //Timeline.Play();
    }

    private void AssignBind(int trackNumber, TileGroupTransition transition)
    {
       if(RoomCotnrolTimeline.playableAsset == null)
        {
            print("error, room control track has no playable asset");
            return;
        }
        TimelineAsset TileGroupOutput = (TimelineAsset)RoomCotnrolTimeline.playableAsset;
        TrackAsset track = TileGroupOutput.GetOutputTrack(trackNumber);
        print("output track is " + track);
        RoomCotnrolTimeline.SetGenericBinding(track, transition);
        //TileGroupOutput.SetUserData(tileGroup[GroupToBind]);

        //Timeline.SetGenericBinding(TileGroupOutput, tileGroup[GroupToBind]);

    }

    public void ChangeTransitionTrackBinding(TileGroupTransition transition)
    {
        if(!transition)
        {
            print("transition is null ");
            return;
        }
        AssignBind(GroupToBind, transition);
    }

}
