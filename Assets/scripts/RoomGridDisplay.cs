using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGridDisplay : MonoBehaviour
{
    [SerializeField] Transform GridOrigin;
    Grid RoomsGrid;
    [SerializeField] Vector3Int TestPacementGridPos;
    [SerializeField] Vector3Int GridPlacementOffset;

    // Start is called before the first frame update
    void Start()
    {
        RoomsGrid = GetComponent<Grid>();
        transform.position = GridOrigin.position;
        DisplayRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayRoom()
    {
        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), RoomsGrid.CellToWorld(TestPacementGridPos + GridPlacementOffset), Quaternion.identity, transform);
    }
}
