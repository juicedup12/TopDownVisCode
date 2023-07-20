using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool walkable;
    public Vector3 worlppos;

    public int gcost, hcost;
    public int gridx, gridy;
    public Node parent;

    public int fcost
    {
        get {
            return gcost + hcost;
        }
    }

    public Node(bool _walkable, Vector2 _worldpos,int _gridx, int _gridy )
    {
        walkable = _walkable;
        worlppos = _worldpos;
        gridx = _gridx;
        gridy = _gridy;
    }
}
