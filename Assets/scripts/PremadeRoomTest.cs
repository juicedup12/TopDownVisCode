using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using topdown;

public class PremadeRoomTest : BaseRoom, ITileTransitionDataProvider
{
    [SerializeField] TileDataContainerSO tileData;
    [SerializeField] player Player;

    public TileContainer.TileTransform[] GetTilesToTransform => tileData.GetTilesToTransform;

    public TileGroupTransitionType GetTileTransitionType => tileData.GetTileTransitionType;

    public TileDataHandlerSO GetTileDataHandler => tileData;


    public override Transform GetDoorFromDirection(DoorDirection doorDirection)
    {
        print("premade room retrieving neighbor door");
        return DoorUtilities.RetrieveNeighborDoor(doorDirection, Doors);
    }

    public override void OnTransitionFinish()
    {
        print("premade room transition is finished");
        Player.SetUIControls(false);
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
