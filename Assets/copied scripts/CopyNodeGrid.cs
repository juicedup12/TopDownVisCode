using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyNodeGrid : MonoBehaviour
{

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    CopyNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse2)) {
            
        //    CreateGrid();
        //}
    }

    private void Start()
    {
        StartCoroutine(ResetGrid());
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void CreateGrid()
    {
        grid = new CopyNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                Collider2D cast = Physics2D.OverlapArea(worldPoint, worldPoint + new Vector3(nodeRadius, -nodeRadius, 1), layerMask: unwalkableMask);
                
                bool walkable = (cast == null ) ;

                //Collider2D[] collider2Ds = new Collider2D[2];
                //ContactFilter2D contactFilter2D = new ContactFilter2D();
                //if (cast != null)
                //    cast.OverlapCollider(contactFilter2D.NoFilter(), collider2Ds);


                //if (collider2Ds[0] != null && collider2Ds[1] != null)
                //{
                //    print(worldPoint + " hit trigger and non trigger " + collider2Ds[0].isTrigger + " " + collider2Ds[1].isTrigger);
                //    walkable = false;
                //}
                grid[x, y] = new CopyNode(walkable, worldPoint, x, y);
            }
        }
    }

    public void ResetGridImmediate()
    {
        CreateGrid();
    }

    public IEnumerator ResetGrid()
    {
        yield return new WaitForSeconds(2);
        print("reseting grid");
        grid = null;
        CreateGrid();
    }

    public List<CopyNode> GetNeighbours(CopyNode node)
    {
        List<CopyNode> neighbours = new List<CopyNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public CopyNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y,1));
        if (grid != null && displayGridGizmos)
        {
            foreach (CopyNode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
