using System.Collections.Generic;
using UnityEngine;


//some methods can be moved to another class called roomcreator
public class Room
{
    int startnode;
    /// <summary>
    /// the first point where the wall connects to the x axis
    /// </summary>
    public Vector2 startwall;
    /// <summary>
    /// the middle point of the wall
    /// </summary>
    public Vector2 corner;
    /// <summary>
    /// the last point where the wall connects to the y axis
    /// </summary>
    public Vector2 lastwall;
    public Roomtile[,] grid;
    List<Roomtile> entrancepoints = new List<Roomtile>();
    int gridsizex;
    int gridsizey;
    GameObject SubRoomBase;
    public GameObject VerticalWall;
    public GameObject HorizontalWall;
    public GameObject InstancedVertWall;
    public GameObject Parent;

    public int closingwall;
    public GameObject wall1, wall2, wall3;
    public bool enclosingleft;
    List<Room> rooms;
    public int firstwall;
    public int startx;
    public int secondwall;
    public int thirdwall;
    string InnerWallLayer;
    public GameObject roombase;

    public Room(int gridsizex, int gridsizey, Roomtile[,] grid, List<Room> rooms, List<Roomtile> entrancepoints, GameObject SubRoomBase, GameObject parent, string InnerWallLayer)
    {
        this.gridsizex = gridsizex;
        this.gridsizey = gridsizey;
        this.grid = grid;
        this.rooms = rooms;
        this.entrancepoints = entrancepoints;
        this.SubRoomBase = SubRoomBase;
        this.Parent = parent;
        this.InnerWallLayer = InnerWallLayer;
    }

    //creates a game object holding asstes that make up a small room for RoomGen
    public GameObject Makesegments()
    {
        //set room base child of roomgen
        roombase = GameObject.Instantiate(SubRoomBase);
        roombase.transform.parent = Parent.transform;

        ChoseStartingPoint();


        //choose a random range between min and max lenghts
        int wallrangestart = (23 * gridsizex - 1) / 100;
        int wallrangeend = (77 * gridsizex - 1) / 100;
        int randomnode = GetRandomPoint(wallrangestart, wallrangeend);
       
        //create first point of wall
        startwall = firstwall == 0 ? grid[randomnode, firstwall].point + (Vector2.up * -.25f) : grid[randomnode, firstwall].point + (Vector2.up * .25f);

        while(WallOnEntrance(startwall))
        {
            randomnode = GetRandomPoint(wallrangestart, wallrangeend);

            //create first point of wall again
            startwall = firstwall == 0 ? grid[randomnode, firstwall].point + (Vector2.up * -.25f) : grid[randomnode, firstwall].point + (Vector2.up * .25f);
        }
        //Debug.Log("start wall is" + startwall);
        roombase.transform.position =  startwall;
        //InstancedVertWall = GameObject.Instantiate(VerticalWall);
        //InstancedVertWall.transform.SetParent(roombase.transform);


        startx = randomnode;
        //Debug.Log("wall 1 starts at " + randomnode + " " + firstwall);
        //make wall go go to the end or to an endpoint based on random

        //Walls only end a certain percent away from their start
        int wallendpercent =  (25 * gridsizey - 1) / 100 ;

        //make second point in wall
        int wallpoint2 = CreateCornerPoint(randomnode, wallendpercent);
        
        //make 3rd point in wall go left or right
        CloseWall();
        lastwall = enclosingleft ? grid[closingwall, wallpoint2].point - (Vector2.right * .25f) : grid[closingwall, wallpoint2].point + (Vector2.right * .25f);

        if (WallOnEntrance(lastwall))
        {
            //keep getting random y axis until it finds one that isn't on the door
            while(WallOnEntrance(lastwall))
            {
                wallpoint2 = CreateCornerPoint(randomnode, wallendpercent);
                lastwall = enclosingleft ? grid[closingwall, wallpoint2].point - (Vector2.right * .30f) : grid[closingwall, wallpoint2].point + (Vector2.right * .30f);
            }
        }



        roombase.GetComponentInChildren<wall>().setwallpos(startwall, corner, firstwall);
        //wall3 = new GameObject("last point");
        thirdwall = closingwall;
        //wall3.transform.position = lastwall;
        //GameObject horiwall = GameObject.Instantiate(HorizontalWall);

        //horiwall.transform.SetParent(roombase.transform);
        //horiwall.transform.position = corner;
        //Debug.Log("corner is " + corner);
        wallDoor HoriwallDoorRef = roombase.GetComponentInChildren<wallDoor>();
        //commented to position the door in a relative position
        //HoriwallDoorRef.transform.parent.position = corner;
        float RelativeCorner = Mathf.Abs(startwall.y - corner.y) ;
        HoriwallDoorRef.transform.parent.position += new Vector3(0, RelativeCorner) ;
        //HoriwallDoorRef.transform.position = new Vector3(0, RelativeCorner) ;
        HoriwallDoorRef.SetHorizontalWall(corner, lastwall, enclosingleft);
        wall vertwallref = roombase.GetComponentInChildren<wall>();
        //if the wall is on the bottom
        if(firstwall == 0)
        {
            HoriwallDoorRef.SetInnerWallLayer(InnerWallLayer);
            vertwallref.LowerWall = true;
        }
        else
        {
            roombase.transform.localScale = new Vector3(roombase.transform.localScale.x, roombase.transform.localScale.y * -1);
            HoriwallDoorRef.RoomOnTopRow();
        }
        //endwall = new Vector2(randomnode, gridworldsize.y);
        if (!enclosingleft)
        {
            Debug.Log("not enclosing left", SubRoomBase);
            roombase.transform.localScale = new Vector3(roombase.transform.localScale.x * -1, roombase.transform.localScale.y );
        }
        return roombase;
    }




    //choses where the vertical wall will end
    int CreateCornerPoint(int randomnode, int wallendpercent)
    {
        //random point on y axis
        int wallpoint2 = firstwall == 0 ? Random.Range(firstwall + wallendpercent, (gridsizey - 1) - wallendpercent) : Random.Range((gridsizey - 1) - wallendpercent, 0 + wallendpercent);
        //Debug.Log("searching for random range from " + (firstwall + wallendpercent) + " and " + (( gridsizey - 1) - wallendpercent));
        
        FixLength(ref wallpoint2);
        //Debug.Log("corner wall is " + randomnode + " " + wallpoint2);

        
        corner = grid[randomnode, wallpoint2].point;
        secondwall = wallpoint2;
        return wallpoint2;
    }

    //checks the room with the lowest or highest y coordinate and changes this y coordinate to give enough space
    void FixLength(ref int yVal)
    {

        bool roomonotherside = false;
        int maxheight = firstwall == 0 ? int.MaxValue : int.MinValue;

        //if there is no room on the other side then return same
        if (rooms.Count < 1) return;

        foreach (Room room in rooms)
        {
            //if room is above
            if (room.firstwall == gridsizey - 1 && room.firstwall != firstwall)
            {
                //save the room with the wall that goes farthest down in maxheight
                if (room.secondwall < maxheight)
                {
                    maxheight = room.secondwall ;
                    roomonotherside = true;
                }
                
            }

            //if room is below
            if (room.firstwall == 0 && room.firstwall != firstwall)
            {
                if(room.secondwall > maxheight)
                {
                    maxheight = room.secondwall;
                    roomonotherside = true;
                }
            }
        }
        //if no room found
        if (!roomonotherside) return;

        //if yval exceeds max height or lowest height, change it to specific value depending on top or bottom wall
        //yVal =  firstwall == 0 ? maxheight - 2 : maxheight + 2;

        if(firstwall == 0 && yVal > maxheight -2)
        {
            yVal = maxheight - 2;
            //Debug.Log("fixing y axis on room on bottom axis");
        }

        if(firstwall == gridsizey - 1 && yVal < maxheight + 2)
        {
            yVal = maxheight + 2;
            //Debug.Log("fixing y axis on room on top axis");
        }
    }

    //changes local bool indicating which direction the room's horizontal wall goes
    //should change to return an enum maybe
    void CloseWall()
    {
        bool RoomOnAxis = false;
        foreach(Room room in rooms)
        {
            //if room on the same axis
            if(room.firstwall == firstwall)
            {
                //switch our enclosing wall to it's opposite
                closingwall = room.enclosingleft ? gridsizex -1: 0;
                enclosingleft = room.enclosingleft ? false : true;
                RoomOnAxis = true;
            }
        }

        //choose random direction when no other wall on left or right
        if(!RoomOnAxis)
        {
            float lastcheck = Random.value;
            //apply to local variables
            closingwall = lastcheck < .5 ? 0 : gridsizex - 1;
            enclosingleft = lastcheck < .5 ? true : false;
        }
        //temporary code to test scale modification
        //enclosingleft = true;
    }

    

    bool WallOnEntrance(Vector2 point)
    {
        if (entrancepoints.Count > 0)
        {
            foreach (Roomtile entrancepoint in entrancepoints)
            {
                float EntranceDist = Vector2.Distance(entrancepoint.point, point);

                //Debug.Log("checking entrance point " + entrancepoint.point + " with " + point + "dist is " + EntranceDist);
                if (EntranceDist <= .4)
                {

                    //Debug.Log(point + " equals entrance point " + entrancepoint.point);
                    return true;
                }
            }
        }
        return false;
    }

    //if room encloses door then change values of wall
    //needs to be modular when the door is on x and y coordinates
    void AvoidDoor()
    {
        //not programmed yet
    }

    //picks a random wall to spawn a subroom on
    void ChoseStartingPoint()
    {
        float startcheck = Random.value;
        //start on the bottom or top of room?
        firstwall = startcheck < .5 ? 0 : gridsizey - 1;
        if(IsRowFull(firstwall))
        {
            //reverse firstwall var
            firstwall = firstwall == 0 ? gridsizey - 1 : 0;
        }
    }

    //checks if top or bottom row is full
    bool IsRowFull(int Wall)
    {
        int RoomsOnAxis = 0;
        //could change to a for loop with a variable of maxroom size
        foreach (Room room in rooms)
        {
            if (room.firstwall == Wall)
            {
                RoomsOnAxis++;
                //if there's more than 1 room on this axis
                if (RoomsOnAxis > 1)
                {
                    //reverse firstwall var
                    //Debug.Log("switching wall");
                    return true;
                }
            }
        }
        return false;
    }

    //checks if there's already 2 small rooms on the same side
    void SwitchFirstWall()
    {
        int RoomsOnAxis = 0;

        //check if there's space on axis otherwise move to other axis
        foreach (Room room in rooms)
        {
            if (room.firstwall == firstwall)
            {
                RoomsOnAxis++;
            }
            //if there's more than 1 room on this axis
            if (RoomsOnAxis > 1)
            {
                firstwall = firstwall == 0 ? gridsizey - 1 : 0;
                //Debug.Log("switching wall");
                return;
            }
        }
    }

    //choose a random point in the x axis that isn't enclosed by any room
    int GetRandomPoint(int wallrangestart, int wallrangeend)
    {
        if (rooms != null)
        foreach(Room room in rooms)
        {
            //if there is a room that is on the same axis
            if(room.firstwall == firstwall)
            {
                //check if random point on the x axis will be enclosed by previous room
                if(room.enclosingleft)
                {
                    //range to right of room
                    return  Random.Range( room.startx, wallrangeend );
                }
                else
                {
                    //range to left of room
                    return Random.Range(wallrangestart, room.startx);
                }
            }
        }
        return Random.Range(wallrangestart, wallrangeend);
    }


    //change to take an enum
    //checks to see if specified axis has 2 rooms on it
    //returns true if space is available
    //axis is either 0 or gridsizey-1
    bool CheckRoomsInAxis(int axis)
    {
        int Roomcount = 0;
        foreach(Room room in rooms)
        {
            if(room.firstwall == axis)
            {
                Roomcount++;
            }
        }
        if(Roomcount >1)
        {
            return false;
        }

        return true;
    }




    public bool SpaceForRoom()
    {
        if (rooms.Count > 3)
            return false;
        return true;
    }

   


    public struct Roomtile
    {
        public Vector2 point;
        public bool empty;
        public bool insideRoom;
        public bool onEntrance;
    }

}
