using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TilePlacementDataContainer", menuName = "ScriptableObjects/TileContainerSO", order = 1)]
public class TileDataContainerSO : TileDataHandlerSO
{
    [SerializeField]
    private TileContainer TileData;
    public override TileContainer.TileTransform[] GetTileTransforms { get => TileData.GetTileTransforms; }

    public override void SetTileData(Vector3Int tilepos, float angle = 0)
    {
        //base.SetTileData(tilepos, angle);
        Debug.Log("data container setting tile data");
        //access tile container
        TileData.RegisterTile(tilepos, angle);
    }
}
