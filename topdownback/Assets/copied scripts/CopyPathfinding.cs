using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPathfinding : MonoBehaviour
{
    CopyRequestManager requestManager;
    CopyNodeGrid grid;
    public float dist = 30;
    public LayerMask wallcheck;

    void Awake()
    {
        requestManager = GetComponent<CopyRequestManager>();
        grid = GetComponent<CopyNodeGrid>();
    }


    public void StartFindPath(Vector3 startPos, Vector3 targetPos, float offset)
    {
        StartCoroutine(FindPath(startPos, targetPos, offset));
        //StartCoroutine(fleepath(startPos, targetPos));

    }


    public bool checkBetween(Vector3 startpos, Vector3 endpos)
    {
        Vector3 dirtoplayer = endpos - startpos;
        RaycastHit2D hit = Physics2D.Raycast(startpos, dirtoplayer, distance: dirtoplayer.magnitude, layerMask: wallcheck);
        if (hit.collider == true)
        {
            return true;
        }
        return false;
    }

    //for putting offset somewhere that is walkable
    public void FixOffset(float offset)
    {
        //if there's a wall between enemy and player
        //Vector3 dirtoplayer = playertransform.position - transform.position;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, dirtoplayer, distance: dirtoplayer.magnitude, layerMask: wallcheck);

        //Vector3 offsetpos = offsetpos = playertransform.position + (dirtoplayer.normalized * offset); 

        //while offset pos hits a collider


        //if (offset <= 0)
        //{
        //    return;
        //}
        //while (CheckOffset(offset))
        //{
        //    offset -= .15f;
        //}
        //Debug.Log("offset is " + offset);


    }

    //if offset is in an unwalkable location
    //returns true if node isn't walkable
    public bool CheckOffset(CopyNode offsetnode, Vector3 startpos ,  Vector3 endpos)
    {
       

        //CodeMonkey.CMDebug.TextPopup("offset overlapping wall", transform.position);
        if (offsetnode.walkable)
        { return false; }


        return true;
    }

    IEnumerator fleepath(Vector3 startpos, Vector3 targetpos)
    {

        CopyNode startnode = grid.NodeFromWorldPoint(startpos);
        startnode.gCost = 0;
        CopyNode targetnode = grid.NodeFromWorldPoint(targetpos);
        List<Node> path = new List<Node>();
        Vector3[] waypoints = new Vector3[0];
        bool pathsuccess = false;
        CopyNode finalnode = null;


        int currentnodeHcost = GetDistance(startnode, targetnode);

        if(currentnodeHcost > 30)
        {
            //CodeMonkey.CMDebug.TextPopup("Max dist of fleepath", startpos);
            yield return null;
        }

        //int lowestgcost = 0;
        //int highestHcost = 0;
        //Node nodetogoto = null;

        List<CopyNode> openset = new List<CopyNode>();
        HashSet<CopyNode> closedset = new HashSet<CopyNode>();
        openset.Add(startnode);
        //while (openset.Count > 0)
        //{
        int cycles = 0;
        while (cycles < 6)
        {
            cycles++;


            CopyNode currentnode = openset[0];
            for (int i = 1; i < openset.Count; i++)
            {
                if (openset[i].fCost > currentnode.fCost && openset[i].hCost > currentnode.hCost
                    || openset[i].fCost == currentnode.fCost && openset[i].gCost < currentnode.gCost)
                {
                    currentnode = openset[i];
                    //path.Add(currentnode);
                }
            }

            openset.Remove(currentnode);
            closedset.Add(currentnode);


            //if current node is far enough away from start node then retrace path
            if (currentnode.gCost >= dist && currentnode != startnode)
            {
                finalnode = currentnode;
                pathsuccess = true;
                break;
            }

            foreach (CopyNode neighbors in grid.GetNeighbours(currentnode))
            {
                int newmovementcosttoneighbor = currentnode.gCost + GetDistance(currentnode, neighbors);



                //if neighbor is walkable and has a higher Hcost go to it
                if (!neighbors.walkable)
                { continue; }


                if (!closedset.Contains(neighbors) && !openset.Contains(neighbors))
                {
                    neighbors.gCost = newmovementcosttoneighbor;
                    neighbors.hCost = GetDistance(neighbors, targetnode);

                    neighbors.parent = currentnode;


                    //add to unexplored nodes
                    if (!openset.Contains(neighbors))
                    {
                        openset.Add(neighbors);
                    }
                }
            }


        }
        yield return null;
        if(pathsuccess)
        {
            waypoints = RetracePath(startnode, finalnode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathsuccess);

    }


    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, float offset)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        CopyNode startNode = grid.NodeFromWorldPoint(startPos);
        Vector3 directiontotarget = startPos - targetPos;
        CopyNode targetNode = grid.NodeFromWorldPoint(targetPos + directiontotarget.normalized * offset);

        //if theres a wall and offset is unwalkable go to plyaer
        if (checkBetween(startPos, targetPos) && CheckOffset(targetNode,startPos , targetPos))
        {
            //offset = 0;
            targetNode = grid.NodeFromWorldPoint(targetPos);
            //CodeMonkey.CMDebug.TextPopup("raycast hit and offset in wall", transform.position);
        }
        

        //change offset to 0 if there's a wall between player and enemy

        if (targetNode.walkable)
        {
            CopyHeap<CopyNode> openSet = new CopyHeap<CopyNode>(grid.MaxSize);
            HashSet<CopyNode> closedSet = new HashSet<CopyNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                CopyNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (CopyNode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        else
        {
            StartCoroutine(fleepath(startPos, targetPos));
            yield return null;
        }

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

        yield return null;
    }

    Vector3[] RetracePath(CopyNode startNode, CopyNode endNode)
    {
        List<CopyNode> path = new List<CopyNode>();
        CopyNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
            
        }

        if (currentNode == startNode)
            path.Add(currentNode);

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<CopyNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(CopyNode nodeA, CopyNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
