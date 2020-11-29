using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{

    public LayerMask uwalkable;
    public Vector2 gridworldsize;
    public float noderadius;
    Node[,] grid;
    float nodediameter;
    int gridsizeX, gridsizeY;
    public bool displaygridgizmos = true;


    // Start is called before the first frame update
    void Awake()
    {
        nodediameter = noderadius * 2;
        gridsizeX = Mathf.RoundToInt(gridworldsize.x / nodediameter) ;
        gridsizeY = Mathf.RoundToInt(gridworldsize.y / nodediameter);
        creategrid();
    }

    void creategrid()
    {
        grid = new Node[gridsizeX, gridsizeY];
        Vector3 worldbottomleft = transform.position - Vector3.right * gridworldsize.x / 2 - Vector3.up * gridworldsize.y/2;
        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                Vector3 worldpoint = worldbottomleft + Vector3.right * (x * nodediameter + noderadius) 
                    + Vector3.up * (y * nodediameter + noderadius);
                Collider2D cast = Physics2D.OverlapArea(worldpoint, worldpoint + new Vector3(noderadius,-noderadius,0) , layerMask: uwalkable );
                bool walkable = (cast == null);
                grid[x, y] = new Node(walkable, worldpoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkx = node.gridx + x;
                int checky = node.gridy + y;
                if(checkx >= 0 && checkx < gridsizeX && checky >= 0 && checky < gridsizeY)
                {
                    neighbors.Add(grid[checkx, checky]);
                }
            }
        }
        return neighbors;
    }

    public Node NodeFromWorldPoint( Vector3 worldpos)
    {
        float percentx = (worldpos.x + gridworldsize.x / 2) / gridworldsize.x;
        float percenty = (worldpos.y + gridworldsize.y / 2) / gridworldsize.y;
        percentx = Mathf.Clamp01(percentx);
        percenty = Mathf.Clamp01(percenty);
        int x = Mathf.RoundToInt((gridsizeX - 1) * percentx);
        int y = Mathf.RoundToInt((gridsizeY - 1) * percenty);
        return grid[x, y];


    }
    


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridworldsize.x, gridworldsize.y, 1));
        if(grid!= null && displaygridgizmos)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                
                    Gizmos.color = Color.black;
                
                Gizmos.DrawCube(n.worlppos, Vector3.one * (nodediameter - .1f));
            }
        }
    }

}
