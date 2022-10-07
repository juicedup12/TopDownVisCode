using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{

    PlayableDirector _PlayableDirector;

    // Start is called before the first frame update
    void Start()
    {
        _PlayableDirector = GetComponent<PlayableDirector>();
    }

    public void AddRoom(GameObject g)
    {
        var outputs = _PlayableDirector.playableAsset.outputs;
        foreach (var itm in outputs)
        {
            Debug.Log(itm.streamName);
            if (itm.streamName == "Animation Track")
            {
                _PlayableDirector.SetGenericBinding(itm.sourceObject, g);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
