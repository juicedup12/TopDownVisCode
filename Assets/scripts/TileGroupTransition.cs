using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public abstract class TileGroupTransition : GroupTransition
{
    [SerializeField]
    protected TileContainer.TileTransform[] tiles;
    [SerializeField] TileDataHandlerSO TileData;
    public TileContainer.TileTransform[] SetTileTransforms 
    { 
        set 
        { 
            tiles = new TileContainer.TileTransform[value.Length];
            //refData.CopyTo(tiles, 0);
            value.CopyTo(tiles, 0);
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].TilePos += TileOffsetPaintPos;
            }
        }

    }
    [SerializeField]
    protected bool firstFrame = false;
    [SerializeField] TileBase tileB;
    [SerializeField] Vector3Int TileOffsetPaintPos;
    public Vector3Int SetOffset { set => TileOffsetPaintPos = value; }
    [SerializeField]
    protected Tilemap tmap;
    public Tilemap SetTilemap { set => tmap = value; }



    public virtual void Awake()
    {
        //if tile data was previously provided then initialise tiles
        if(TileData)
        {
            NewTiles(TileData.GetTileTransforms);
        }
    }

    public void PaintTiles()
    {

        print("painting tiles to map");

        foreach (TileContainer.TileTransform tile in tiles)
        {
            tmap.SetTile(tile.TilePos, tileB);
        }
    }

    //void ApplyTilesWithOffset()
    //{
    //    //create a reference data variable to hold the tile transforms
    //    //then copy it to tiles then apply offsets
    //    TileContainer.TileTransform[] refData = tileData.GetTileTransforms;

    //    for (int i = 0; i < tiles.Length; i++)
    //    {
    //        tiles[i].TilePos += (Vector3Int)TileOffsetPaintPos;
    //    }
    //}


    public int TileLength { get { return tiles.Length; } }


    public override void IterateGroup(float duration, float time)
    {
        for (int i = 0; i < TileLength; i++)
        {
            float durationsplit = duration / TileLength;
            float startTime = i * durationsplit;
            float localTime = time - startTime;
            float timePercentage = localTime / durationsplit;

            if (time >= startTime + durationsplit && !tiles[i].FinishedTransition)
            {
                LerpOjects(i, 1);
                tiles[i].FinishedTransition = true;
                continue;
            }

            if (time > startTime && time < startTime + durationsplit)
            {
                print("rotate lerp working on " + i);
                LerpOjects(i, timePercentage);

            }
        }

    }

    public abstract void LerpOjects(int TileIndex, float TimePercentage);

    public virtual void NewTiles(TileContainer.TileTransform[] tiles)
    {
        if(tiles == null)
        {
            print("No tiles provided, aborting.");
            return;
        }
        SetTileTransforms = tiles;
        tmap.ClearAllTiles();

        firstFrame = false;
        PaintTiles();
    }
}
