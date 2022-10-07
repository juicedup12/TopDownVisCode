using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "TilePlacementData", menuName = "ScriptableObjects/TilePlacementSO", order = 1)]
public class TileDataHandlerSO : ScriptableObject
{
    
    //[SerializeField]  TileContainer[] tiles;
    //public TileContainer[] GetTiles { get { return tiles; } }

    //public void SetData(Vector3Int tilepos, int Index, int TileIndex, float angle = 0)
    //{
    //    if(tiles.Length < TileIndex)
    //    {
    //        Debug.Log($"index to edit is out of bounds, tiles array length ={tiles.Length} index to edit is {TileIndex}");
    //        return;
    //    }
    //    tiles[TileIndex].RegisterTile(tilepos, Index, angle);
    //    Debug.Log("tiles added to " + TileIndex);
        
    //}

    public virtual TileContainer.TileTransform[] GetTileTransforms { get ;  } 

    public virtual void SetTileData(Vector3Int tilepos, float angle = 0)
    {
        Debug.Log($"setting tile data for tilepos {tilepos} with angle {angle}");
    }


}
