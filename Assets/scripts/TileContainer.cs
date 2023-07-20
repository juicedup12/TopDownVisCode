using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//class for holding data about tiles
[Serializable]
public class TileContainer
{
    [SerializeField]
    TileTransform[] TileTransforms;
    public TileTransform[] GetTileTransforms{ get { return TileTransforms; } }

    [Serializable]
    public struct TileTransform
    {
        public Vector3Int TilePos;
        public float TileZAngle;
        public bool FinishedTransition;

        public TileTransform (Vector3Int tilepos, float Zangle)
        {
            TilePos = tilepos;
            TileZAngle = Zangle;
            FinishedTransition = false;
        }
    }

    //data handler scriptable objects provides data
    public void RegisterTile(Vector3Int tilepos,  float angle = 0)
    {
        if (TileTransforms != null)
        {
            Debug.Log("there's already an array");
            //if there's an array make it bigger
            if (TileTransforms.Length == 0)
            {
                Debug.Log("making new array with size of " + 1);
                TileTransforms = new TileTransform[1];
                TileTransforms[0] = new TileTransform(tilepos, angle);
            }
            else
            {
                int NewLegnth = TileTransforms.Length + 1;
                Debug.Log("making new array with size of " + NewLegnth);
                TileTransform[] TempTransforms = new TileTransform[NewLegnth];
                TileTransforms.CopyTo(TempTransforms, 0);
                //change the last index element
                TempTransforms[NewLegnth - 1] = new TileTransform(tilepos, angle);
                TileTransforms = TempTransforms;
            }
        }
        //}
        //else if(Index == 1)
        //{
        //    Debug.Log("no array, creating new array with size 1");
        //    TileTransforms =new TileTransform[1];
        //    TileTransforms[0] = new TileTransform(tilepos, angle);
        //}
        //else
        //{
        //    Debug.Log("can't create array while index is greater than 0");
        //}
    }
}
