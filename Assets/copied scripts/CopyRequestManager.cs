using System;
using System.Collections.Generic;
using UnityEngine;


//queues up pathfinding requests
public class CopyRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    //static CopyRequestManager instance;
    CopyPathfinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        //instance = this;
        pathfinding = GetComponent<CopyPathfinding>();
    }

    public  void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback, float offset)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback, offset);
        pathRequestQueue.Enqueue(newRequest);
        TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.offset);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public float offset;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback, float _offset)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
            offset = _offset;
        }

    }
}
