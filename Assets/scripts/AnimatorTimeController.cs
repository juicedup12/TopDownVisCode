using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

//a replacement for the animator controller controller component
public class AnimatorTimeController : MonoBehaviour
{

    public AnimationClip clip;


    PlayableGraph playableGraph;

    AnimationClipPlayable playableClip;


    void Start()

    {


        playableGraph = PlayableGraph.Create();
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());

        // Wrap the clip in a playable

        playableClip = AnimationClipPlayable.Create(playableGraph, clip);

        // Connect the Playable to an output

        playableOutput.SetSourcePlayable(playableClip);

        // Plays the Graph.

        playableGraph.Play();

        // Stops time from progressing automatically.

        playableClip.Pause();

    }



    // Control the time manually
    public void SetTime(double time)
    {
        if(playableGraph.IsValid())
        playableClip.SetTime(time);
    }



    void OnDisable()

    {

        // Destroys all Playables and Outputs created by the graph.
        if(playableGraph.IsValid())
        playableGraph.Destroy();

    }

}

