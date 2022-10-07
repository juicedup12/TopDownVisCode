using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileScaleController : TileGroupTransition
{
    public TileContainer tileContainer;

    public override void Awake()
    {
        base.Awake();
        firstFrame = false;
    }


    public override void LerpObjects(float duration, float time)
    {
        if (!firstFrame)
        {
            tiles = tileData.GetTileTransforms;
            firstFrame = true;
        }

        if (tmap == null)
        {
            print("getting tmap reference");
            tmap = GetComponent<Tilemap>();
        }

        float tileLength = tiles.Length;
        if(tileLength > 0)
        for (int i = 0; i < tileLength; i++)
        {
            float startTime = i * (duration / tileLength);
            if (time > startTime)
            {
                float localTime = time - startTime;
                Vector3 TileScale =  Vector2.Lerp(Vector2.one, Vector2.one * 2, localTime / (duration / tileLength));
                tmap.SetTransformMatrix(tiles[i].TilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, TileScale));
            }
        }
    }

}
