using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGroupScale : TileGroupTransition
{

    public override void Awake()
    {
    }

    public override void NewTiles(TileContainer.TileTransform[] tiles)
    {
        base.NewTiles(tiles);
        print("group scale adding new tiles");
        SetTileStartScale();
    }


    public  void LerpObjects(float duration, float time)
    {
        //if (!firstFrame)
        //{
        //    tiles = tileData.GetTileTransforms;
        //    firstFrame = true;
        //}

        //if (tmap == null)
        //{
        //    print("getting tmap reference");
        //    tmap = GetComponent<Tilemap>();
        //}

        float tileLength = tiles.Length;
        if(tileLength > 0)
        for (int i = 0; i < tileLength; i++)
        {
            float durationsplit = duration / tileLength;
            float startTime = i * (duration / tileLength);
            if (time > startTime && time < startTime + durationsplit + .2f)
            {
                float localTime = time - startTime;
                Vector3 TileScale =  Vector2.Lerp(Vector2.zero, Vector2.one, localTime / (duration / tileLength));
                tmap.SetTransformMatrix(tiles[i].TilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, TileScale));
            }
        }
    }

    void SetTileStartScale()
    {
        //set all tiles to zero
        foreach (TileContainer.TileTransform tilepos in tiles)
        {
            Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.zero);
            tmap.SetTransformMatrix(tilepos.TilePos, m);
        }
    }

    public override void LerpOjects(int TileIndex, float TimePercentage)
    {
        
        Vector3 TileScale = Vector2.Lerp(Vector2.zero, Vector2.one, TimePercentage);
        tmap.SetTransformMatrix(tiles[TileIndex].TilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, TileScale));
    }
}
