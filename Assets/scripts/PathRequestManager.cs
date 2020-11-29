using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{

    public Queue<pathrequest> pathrequestqueue = new Queue<pathrequest>();
    public static PathRequestManager instance;
    pathfinding pathcreate;
    public bool isprocessing;
    pathrequest currentpathrequest;

    private void Awake()
    {
        instance = this;
        pathcreate = GetComponent<pathfinding>();
    }

    public static void requestpath(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
    {
        pathrequest newrequest = new pathrequest(start, end, callback);
        instance.pathrequestqueue.Enqueue(newrequest);
        instance.Tryprocessnext();
    }

    void Tryprocessnext()
    {
        if(!isprocessing && pathrequestqueue.Count >0)
        {
            currentpathrequest = pathrequestqueue.Dequeue();
            isprocessing = true;
            pathcreate.Startfindingpath(currentpathrequest.start, currentpathrequest.end);
            
        }
    }

    public void finishedprocessing(Vector3[] waypoints, bool success)
    {
        Debug.Log("finished processing");
        currentpathrequest.callback(waypoints, success);
        isprocessing = false;
        Tryprocessnext();
    }

    



    public struct pathrequest
    {
        public Vector3 start;
        public Vector3 end;
        public Action<Vector3[], bool> callback;
        public pathrequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            start = _start;
            end = _end;
            callback = _callback;
        }

    }
        

}
