using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//responsible for placing tiles on the tilemap using a tiledatahandler object
public class TilePainter : MonoBehaviour
{
    [SerializeField] TileDataHandlerSO TileData;
    private Tilemap Tmap;
    [SerializeField] TileBase tile;

    private void Awake()
    {
        Tmap = GetComponent<Tilemap>();

    }

    void SetTiles()
    {
        Tmap.ClearAllTiles();
        foreach (TileContainer.TileTransform tileTransform in TileData.GetTileTransforms)
        {
            //need to set matrix with rotation
            Tmap.SetTile(tileTransform.TilePos, tile);
            print("painting tile " + tileTransform.TilePos);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
