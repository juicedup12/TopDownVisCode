using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileGroupTransition : MonoBehaviour
{
    [SerializeField]
    protected TileContainer.TileTransform[] tiles;
    [SerializeField]
    protected TileDataHandlerSO tileData;
    [SerializeField]
    protected bool firstFrame = false;
    [SerializeField] TileBase tileB;

    public virtual void Awake()
    {
        print("tile group transition awake");
        if (tileData != null)
            tiles = tileData.GetTileTransforms;
        else
            print("no tile data for object " + gameObject.name);
    }

    public void PaintTiles()
    {
        print("painting tiles to map");
        tmap.ClearAllTiles();
        foreach (TileContainer.TileTransform tile in tiles)
        {
            tmap.SetTile(tile.TilePos, tileB);
        }
    }


    protected Tilemap tmap;
    public int TileLength { get { return tiles.Length; } }


    public virtual void LerpObjects(float duration, float time)
    {
       
    }
    public virtual void LerpObjects()
    {
        if (!firstFrame)
        {
            tiles = tileData.GetTileTransforms;
            PaintTiles();
            firstFrame = true;
        }
    }
}
