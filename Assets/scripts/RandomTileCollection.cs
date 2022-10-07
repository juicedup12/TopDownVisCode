using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//for registering tiles into a scriptable object and retrieving tile containers randomly
//place this on a tilemap object
public class RandomTileCollection : MonoBehaviour
{
    [SerializeField] TileDataHandlerSO TileData;
    [SerializeField] int TDataIndexToEdit;

    /*
    //commented for later use in a tiledatahandlerSO
    //**************************************************
    public void RegisterTile(Vector3Int tilepos, float angle, int index)
    {

        //TileData.SetData(tilepos, index, TDataIndexToEdit);
    }

    //public TileContainer RetrieveTileContainter()
    //{
    //    //int length = TileData.GetTiles.Length;
    //   // return TileData.GetTiles[Random.Range(0, length)];
    //}
    //
    ************************************************************/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
