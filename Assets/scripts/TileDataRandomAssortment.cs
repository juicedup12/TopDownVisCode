using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds an array of tile containers 
//can retrieve a random tiletransform array
[CreateAssetMenu(fileName = "TPlacementRandomDataContainer", menuName = "ScriptableObjects/TileRandomContainerSO", order = 1)]
public class TileDataRandomAssortment : TileDataHandlerSO
{
    [SerializeField]
    TileContainer[] TileDataContainers;
    [SerializeField] int TContainerRegisterIndex;
    //I don't think selectedrandomtransforms is needed
    private TileContainer.TileTransform[] SelectedRandomTransforms;
    public override TileContainer.TileTransform[] GetTileTransforms => GetRandomTileTransform();

    public void Awake()
    {
        Debug.Log("changing random transforms");
        ChangeSelectedRandomTransforms();
    }


    private void ChangeSelectedRandomTransforms()
    {

        SelectedRandomTransforms = GetRandomTileTransform();
    }

    //sets tile coordinates and rotation to data container array based on register index
    public override void SetTileData(Vector3Int tilepos, float angle = 0)
    {
        base.SetTileData(tilepos, angle);
        if (TileDataContainers.Length < 0)
            return;
        else
            if (TContainerRegisterIndex < TileDataContainers.Length && TContainerRegisterIndex >= 0)
            TileDataContainers[TContainerRegisterIndex].RegisterTile(tilepos, angle);
    }

    TileContainer.TileTransform[] GetRandomTileTransform()
    {
        if (TileDataContainers.Length < 1) return null;

        int RandomIndex = Random.Range(0, TileDataContainers.Length);
        Debug.Log("returning index " + RandomIndex + " with size of " + TileDataContainers[RandomIndex].GetTileTransforms.Length);
        return TileDataContainers[RandomIndex].GetTileTransforms;
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
