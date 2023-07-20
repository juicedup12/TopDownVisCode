using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{

    public Vector2 gridworldsize;
    public Vector2[] RightEntrancepoints;
    public Vector2[] LeftEntrancepoints;
    public Vector2[] TopEntrancepoints;
    public Vector2[] BottomEntrancepoints;
    List<Room.Roomtile> GridEntrancePoints = new List<Room.Roomtile>();
    int gridsizeX, gridsizeY;
    public float noderadius;
    float nodediameter;
    Vector3 worldbottomleft;
    Room.Roomtile [,] grid;
    public bool drawgrid = false;
    Vector2 startwall;
    Vector2 endwall;
    List<Room> rooms = new List<Room>();
    public GameObject LeftEntrance, RightEntrance, TopEntrance, BottomEntrance;


    void Awake()
    {
        nodediameter = noderadius * 2;
        gridsizeX = Mathf.RoundToInt(gridworldsize.x / nodediameter);
        gridsizeY = Mathf.RoundToInt(gridworldsize.y / nodediameter);
        worldbottomleft = transform.position - Vector3.right * gridworldsize.x / 2 - Vector3.up * gridworldsize.y / 2;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        creategrid();
        EntrancePointsToGridCoordinates();
        makewall();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            makewall();
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            rooms.Clear();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //ChangeGridEmpty();
            ChangeGridInRoom();
        }
    }

    void creategrid()
    {
        grid = new Room.Roomtile[gridsizeX, gridsizeY];
        Vector3 worldbottomleft = transform.position - Vector3.right * gridworldsize.x / 2 - Vector3.up * gridworldsize.y / 2;
        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                Vector3 worldpoint = worldbottomleft + Vector3.right * (x * nodediameter + noderadius)
                    + Vector3.up * (y * nodediameter + noderadius);
                grid[x, y].point = worldpoint;
                grid[x, y].empty = true;
                grid[x, y].onEntrance = false;
            }
        }
    }

    void EntrancePointsToGridCoordinates()
    {
        //if right is open
        if (!RightEntrance.activeSelf)
        for (int i = 0; i < RightEntrancepoints.Length; i++)
        {
                Room.Roomtile tile = NodeFromWorldPoint(RightEntrancepoints[i]);
                Debug.Log("adding right entrance points to grid " + tile.point);
                GridEntrancePoints.Add(tile);
                tile.onEntrance = true;
            }
        if(!LeftEntrance.activeSelf)
            for (int i = 0; i < LeftEntrancepoints.Length; i++)
            {
                Room.Roomtile tile = NodeFromWorldPoint(LeftEntrancepoints[i]);
                Debug.Log("adding Left entrance points to grid " + tile.point);
                GridEntrancePoints.Add(tile);
                tile.onEntrance = true;
            }
        if (!TopEntrance.activeSelf)
            for (int i = 0; i < TopEntrancepoints.Length; i++)
            {
                Debug.Log("adding Top entrance points to grid");
                GridEntrancePoints.Add(NodeFromWorldPoint(TopEntrancepoints[i]));
            }
        if (!BottomEntrance.activeSelf)
            for (int i = 0; i < BottomEntrancepoints.Length; i++)
            {
                Debug.Log("adding Bottom entrance points to grid" + BottomEntrancepoints[i]);
                GridEntrancePoints.Add(NodeFromWorldPoint(BottomEntrancepoints[i]));
            }
    }
    

    void makewall()
    {
        Room wall = new Room(gridsizeX, gridsizeY, grid, rooms, GridEntrancePoints);
        wall.Makesegments();
        rooms.Add(wall);
        
    }

    //change all tiles that are within walls to notempty
    public void ChangeGridEmpty()
    {
        foreach (Room room in rooms)
        {
            int starty = room.firstwall;
            int endy = room.secondwall;
            grid[room.startx, starty].empty = false;
            do
            {
                if(room.firstwall == 0)
                {
                    starty++;
                }
                else
                {
                    starty--;
                }
                grid[room.startx, starty].empty = false;
            } while (starty != endy);


            int startx = room.startx;
            int endx = room.closingwall;
            do
            {
                
                if (room.enclosingleft)
                {
                    startx--;
                }
                else
                {
                    startx++;
                }
                grid[startx, starty].empty = false;
            } while (startx != endx);
        }
    }

    public void ChangeGridInRoom()
    {
        foreach (Room room in rooms)
        {
            int starty = room.firstwall;
            int endy = room.secondwall;
            //grid[room.startx, starty].empty = false;
            int startx = room.startx;
            while (startx != room.closingwall)
            {
                startx -= room.enclosingleft ? 1 : -1;
                grid[startx, starty].insideRoom = true;
            }

            do
            {
                startx = room.startx;
                if (room.firstwall == 0)
                {
                    starty++;
                }
                else
                {
                    starty--;
                }
                while (startx != room.closingwall)
                {
                    startx -= room.enclosingleft ? 1 : -1;
                    grid[startx, starty].insideRoom = true;
                }

            } while (starty != endy);


            
            //int endx = room.closingwall;
            //do
            //{

            //    if (room.enclosingleft)
            //    {
            //        startx--;
            //    }
            //    else
            //    {
            //        startx++;
            //    }
            //    while (startx != room.closingwall)
            //    {
            //        startx -= room.enclosingleft ? 1 : -1;
            //        grid[startx, starty].insideRoom = true;
            //    }
            //} while (startx != endx);
        }
    }

    public Room.Roomtile NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridworldsize.x / 2) / gridworldsize.x;
        float percentY = (worldPosition.y + gridworldsize.y / 2) / gridworldsize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridsizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridsizeY - 1) * percentY);
        //Debug.Log("returning " + x + " " + y + " from " + worldPosition);
        if (x > gridsizeX && y > gridsizeY)
        {
            Debug.Log("x and y out of bounds");
        }
        Debug.Log("returning grid point " + x + " and " + y +" position"  + grid[x,y].point);
        return grid[x, y];
        
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridworldsize.x, gridworldsize.y, 1));
        if (grid != null && drawgrid) 
        foreach (Room.Roomtile n in grid)
        {
            
            Gizmos.color = Color.black;
                if(!n.empty)
                    Gizmos.color = Color.white;
                if (n.insideRoom)
                    Gizmos.color = Color.blue;
                if (n.onEntrance)
                    Gizmos.color = Color.red;
                Gizmos.DrawCube(n.point, Vector3.one * (nodediameter - .1f));
        }

        Gizmos.color = Color.blue;
        //if (startwall != null && endwall != null)
        //Gizmos.DrawLine(startwall, endwall);
        if(rooms != null)
        foreach(Room room in rooms)
        {
            if (room.startwall != null && room.corner != null)
            {
                Gizmos.DrawLine(room.startwall, room.corner);
                Gizmos.DrawLine(room.corner, room.lastwall);
            }

        }

        

        foreach(Vector2 point in RightEntrancepoints)
        {
            Gizmos.DrawWireSphere(point, .3f);
        }
        foreach (Vector2 point in LeftEntrancepoints)
        {
            Gizmos.DrawWireSphere(point, .3f);
        }
        foreach (Vector2 point in TopEntrancepoints)
        {
            Gizmos.DrawWireSphere(point, .3f);
        }
        foreach (Vector2 point in BottomEntrancepoints)
        {
            Gizmos.DrawWireSphere(point, .3f);
        }
    }


}
