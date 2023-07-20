using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class pathfinding : MonoBehaviour
{
    public int dist = 30;
    Node distnode;

    NodeGrid grid;
    public bool movetowardtarget = true;
    public float offset;
    PathRequestManager pathmanager;

    private void Awake()
    {
        grid = GetComponent<NodeGrid>();
        pathmanager = GetComponent<PathRequestManager>();

    }


    void findpath(Vector3 startpos, Vector3 targetpos)
    {
        //get starting and end node
        Node startnode = grid.NodeFromWorldPoint(startpos);
        Node targetnode = grid.NodeFromWorldPoint(targetpos);
        
        List<Node> openset = new List<Node>();
        HashSet<Node> closedset = new HashSet<Node>();
        openset.Add(startnode);


        while (openset.Count > 0)
        {
            Node currentnode = openset[0];

            //asign current node to the one with lowest f cost
            for (int i = 1; i < openset.Count; i++)
            {
                if (openset[i].fcost < currentnode.fcost || openset[i].fcost == currentnode.fcost && openset[i].hcost < currentnode.hcost)
                {
                    currentnode = openset[i];
                }
            }
            //add node to explored
            openset.Remove(currentnode);
            closedset.Add(currentnode);

            if (currentnode == targetnode)
            {
                retracepath(startnode, targetnode);
                return;
            }

            foreach (Node neighbors in grid.GetNeighbors(currentnode))
            {
                if (!neighbors.walkable || closedset.Contains(neighbors))
                { continue; }

                //asign values to neighbors
                int newmovementcosttoneighbor = currentnode.gcost + Getdistance(currentnode, neighbors);
                if (newmovementcosttoneighbor < neighbors.gcost || !openset.Contains(neighbors))
                {
                    neighbors.gcost = newmovementcosttoneighbor;
                    neighbors.hcost = Getdistance(neighbors, targetnode);
                    neighbors.parent = currentnode;

                    //add to unexplored nodes
                    if(!openset.Contains(neighbors))
                    {
                        openset.Add(neighbors);
                    }
                }
            }
        }
    }


    Vector3[] retracepath(Node startnode, Node targetnode)
    {
        List<Node> path = new List<Node>();
        Node currentnode = targetnode;
        while (currentnode != startnode)
        {
            path.Add(currentnode);
            currentnode = currentnode.parent;
        }
        Vector3[] waypoints = simplifypath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] simplifypath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>() ;
        Vector3 oldDirection = Vector3.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 newdirection = new Vector3(path[i - 1].gridx - path[i].gridx, path[i - 1].gridy - path[i].gridy);
            if(newdirection != oldDirection)
            {
                waypoints.Add(path[i -1].worlppos);
            }
            oldDirection = newdirection;
        }
        return waypoints.ToArray();
    }


    int Getdistance(Node nodea, Node nodeb)
    {
        int distx = Mathf.Abs(nodea.gridx - nodeb.gridx);
        int disty = Mathf.Abs(nodea.gridy - nodeb.gridy);

        if (distx > disty) return 14 * disty + 10 * (distx - disty);

         return 14 * distx + 10 * (disty - distx);
    }

    




     public void Startfindingpath(Vector3 start, Vector3 end)
    {
        StartCoroutine(FindpathwithOffset(start, end, offset));
    }


    IEnumerator FindpathwithOffset (Vector3 startpos, Vector3 targetpos, float offset)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathfindsuccess = false;
        Vector3 directiontotarget = startpos - targetpos ;

        //get starting and end node
        Node startnode = grid.NodeFromWorldPoint(startpos);
        Node targetnode = grid.NodeFromWorldPoint(targetpos + directiontotarget.normalized * offset);

        List<Node> openset = new List<Node>();
        HashSet<Node> closedset = new HashSet<Node>();
        openset.Add(startnode);
        
        if (startnode.walkable && targetnode.walkable)
        {
            while (openset.Count > 0)
            {
                Node currentnode = openset[0];

                //asign current node to the one with lowest f cost
                for (int i = 1; i < openset.Count; i++)
                {
                    if (openset[i].fcost < currentnode.fcost || openset[i].fcost == currentnode.fcost && openset[i].hcost < currentnode.hcost)
                    {
                        currentnode = openset[i];
                    }
                }
                //add node to explored
                openset.Remove(currentnode);
                closedset.Add(currentnode);

                if (currentnode == targetnode)
                {
                    pathfindsuccess = true;
                    break;
                }

                foreach (Node neighbors in grid.GetNeighbors(currentnode))
                {
                    if (!neighbors.walkable || closedset.Contains(neighbors))
                    { continue; }

                    //asign values to neighbors
                    int newmovementcosttoneighbor = currentnode.gcost + Getdistance(currentnode, neighbors);
                    if (newmovementcosttoneighbor < neighbors.gcost || !openset.Contains(neighbors))
                    {
                        neighbors.gcost = newmovementcosttoneighbor;
                        neighbors.hcost = Getdistance(neighbors, targetnode);
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
            if (pathfindsuccess)
            {
                waypoints = retracepath(startnode, targetnode);
            }
            pathmanager.finishedprocessing(waypoints, pathfindsuccess);
        }
        
        
    }

    public void fleepath(Vector3 startpos, Vector3 targetpos)
    {
        
        Node startnode = grid.NodeFromWorldPoint(startpos);
        startnode.gcost = 0;
        Node targetnode = grid.NodeFromWorldPoint(targetpos);
        List<Node> path = new List<Node>();

        int currentnodeHcost = Getdistance(startnode, targetnode);
        //int lowestgcost = 0;
        //int highestHcost = 0;
        //Node nodetogoto = null;

        List<Node> openset = new List<Node>();
        HashSet<Node> closedset = new HashSet<Node>();
        openset.Add(startnode);
        //while (openset.Count > 0)
        //{
        int cycles = 0;
        while (cycles <6)
        {
            cycles++;
        
            
            Node currentnode = openset[0];
            for (int i = 1; i < openset.Count; i++)
            {
                if (openset[i].fcost > currentnode.fcost && openset[i].hcost >currentnode.hcost
                    || openset[i].fcost == currentnode.fcost && openset[i].gcost< currentnode.gcost)
                {
                    currentnode = openset[i];
                    //path.Add(currentnode);
                }
            }

            openset.Remove(currentnode);
            closedset.Add(currentnode);


            //if current node is far enough away from start node then retrace path
            if(currentnode.gcost >= dist && currentnode != startnode)
            {
                retracepath(startnode, currentnode);
                return;
            }

            foreach (Node neighbors in grid.GetNeighbors(currentnode))
            {
                int newmovementcosttoneighbor = currentnode.gcost + Getdistance(currentnode, neighbors);

               

                //if neighbor is walkable and has a higher Hcost go to it
                if (!neighbors.walkable)
                { continue; }

                
                if (!closedset.Contains(neighbors) && !openset.Contains(neighbors))
                {
                    neighbors.gcost = newmovementcosttoneighbor;
                    neighbors.hcost = Getdistance(neighbors, targetnode);
                   
                    neighbors.parent = currentnode;
                    

                    //add to unexplored nodes
                    if (!openset.Contains(neighbors))
                    {
                        openset.Add(neighbors);
                    }
                }
            }

            
        }
    }

    //public void IsFarEnough(int dist)
    //{
    //    Node startnode = grid.NodeFromWorldPoint(seeker.position);
    //    Node targetnode = grid.NodeFromWorldPoint(target.position);
    //    if (Getdistance(startnode, targetnode) > dist)
    //    {
    //        movetowardtarget = true;
            
    //    }
    //}

    void distancepath(Vector3 startpos, Vector3 targetpos, int dist)
    {

        //Node NodeNearestToDist;

        Node startnode = grid.NodeFromWorldPoint(startpos);
        Node targetnode = grid.NodeFromWorldPoint(targetpos);

        List<Node> openset = new List<Node>();
        HashSet<Node> closedset = new HashSet<Node>();
        openset.Add(startnode);
        while (openset.Count > 0)
        {
            Node currentnode = openset[0];
            for (int i = 1; i < openset.Count; i++)
            {
                if (openset[i].fcost < currentnode.fcost || openset[i].fcost == currentnode.fcost && openset[i].hcost > currentnode.hcost)
                {
                    currentnode = openset[i];
                }
            }
            openset.Remove(currentnode);
            closedset.Add(currentnode);
            if (currentnode.fcost == startnode.hcost + dist)
            {
                distnode = currentnode;
                //retracepathtodistnode(startnode, distnode);
                return;
            }

            foreach (Node neighbors in grid.GetNeighbors(currentnode))
            {
                if (!neighbors.walkable || closedset.Contains(neighbors))
                { continue; }


                int newmovementcosttoneighbor = currentnode.gcost + Getdistance(currentnode, neighbors);
                if (newmovementcosttoneighbor > neighbors.gcost || !openset.Contains(neighbors))
                {
                    neighbors.gcost = newmovementcosttoneighbor;
                    neighbors.hcost = Getdistance(neighbors, targetnode);
                    neighbors.parent = currentnode;

                    if (!openset.Contains(neighbors))
                    {
                        openset.Add(neighbors);
                    }
                }
            }
        }
    }


   
}
