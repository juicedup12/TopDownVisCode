using System.Collections.Generic;
using UnityEngine;


namespace topdown
{
    public class ProcedualLevelCreator : MonoBehaviour, iStageBuild
    {
        public float gridoffset;
        public GameObject roombase, BossRoom;
        public Transform shop;
        [SerializeField] Transform LevelPivot;
        [SerializeField] Vector3 LevelPivotOffset;
        enum LevelTile { empty, floor };
        LevelTile[,] grid;
        //RoomPrefabs with same index positions as grid
        public GameObject[,] roomGrid;
        public bool UseDebug = false;
        struct RandomWalker
        {
            public bool mainwalker;
            public bool Finished;
            public int steps;
            public Vector2 dir;
            public Vector2 pos;
        }
        List<RandomWalker> walkers;
        RandomWalker mainWalker;
        public int levelWidth;
        public int levelHeight;
        public float percentToFill = 0.2f;
        public float chanceWalkerChangeDir = 0.5f;
        public float chanceWalkerSpawn = 0.05f;
        public float chanceWalkerDestoy = 0.05f;
        public int maxWalkers = 2;
        public int iterationSteps = 100000;
        char up = 'u', down = 'd', left = 'l', right = 'r';
        static List<Transform> RoomList = new List<Transform>();
        player player;
        public player _player { set { player = value; } }


        // Start is called before the first frame update
        void Start()
        {
            //Setup();
            //CreateFloors();
            //SpawnLevel();
            //OpenDoors();
            //GenerateRoomAssets();
            //deactivateRooms();
        }


        void Setup()
        {
            // prepare grid
            grid = new LevelTile[levelWidth, levelHeight];
            for (int x = 0; x < levelWidth - 1; x++)
            {
                for (int y = 0; y < levelHeight - 1; y++)
                {
                    grid[x, y] = LevelTile.empty;
                }
            }
            roomGrid = new GameObject[levelWidth, levelHeight];

            //generate first walker
            walkers = new List<RandomWalker>();
            RandomWalker walker = new RandomWalker();
            walker.mainwalker = true;
            Vector2 pos = new Vector2(Mathf.RoundToInt(levelWidth / 2.0f), 0);
            walker.pos = pos;
            if (UseDebug)
                Debug.Log("making new walker at" + walker.pos);
            walker.dir = RandomDirection(walker);
            walkers.Add(walker);
            mainWalker = walker;

            grid[(int)walker.pos.x, (int)walker.pos.y] = LevelTile.floor;
            spawnAtIndex((int)walker.pos.x, (int)walker.pos.y);
            RoomGen room = roomGrid[(int)walker.pos.x, (int)walker.pos.y].GetComponent<RoomGen>();
            passInWarpPoints(room, shop, 'n');

            //spawnAtIndex((int)walker.pos.x + 1, (int)walker.pos.y);
            //spawnAtIndex((int)walker.pos.x - 1, (int)walker.pos.y);
            //spawnAtIndex((int)walker.pos.x, (int)walker.pos.y + 1);
        }

        void CreateFloors()
        {
            int iterations = 0;
            RandomWalker currentWalker;
            GameObject CurrentRoom;
            do
            {

                //create floor at position of every Walker
                for (int i = 0; i < walkers.Count; i++)
                {
                    if (UseDebug)
                        Debug.Log("on walker " + i);
                    currentWalker = walkers[i];

                    //make walker stop spawning
                    finishWalker(currentWalker);
                    walkers[i] = currentWalker;
                    if (currentWalker.Finished)
                        continue;
                    //add a shop to door in direction of the walker direction




                    //check if walker's stuck and move it if it is

                    if (WalkerStuck((int)currentWalker.pos.x, (int)currentWalker.pos.y))
                    {
                        if (UseDebug)
                            Debug.Log("walker Stuck at " + currentWalker.pos.x + ", " + currentWalker.pos.y);
                        currentWalker.pos = TransferWalker(currentWalker);
                        walkers[i] = currentWalker;
                        if (UseDebug)
                            Debug.Log("walker in list's pos is " + walkers[i].pos);
                    }


                    //update currentpos variables
                    int CurrentPosx = (int)currentWalker.pos.x;
                    int CurrentPosy = (int)currentWalker.pos.y;

                    //use current room to connect to next created
                    CurrentRoom = roomGrid[(int)currentWalker.pos.x, (int)currentWalker.pos.y];
                    if (UseDebug)
                        Debug.Log("current walker pos is " + CurrentPosx + " " + CurrentPosy);

                    //check if the next direction will be invalid
                    currentWalker.steps++;
                    if (OutOfBounds((int)(currentWalker.pos.x + currentWalker.dir.x), (int)(currentWalker.pos.y + currentWalker.dir.y))
                        || walkerHitInvalid(currentWalker))
                        do
                        {
                            if (UseDebug)
                                Debug.Log(CurrentPosx + " " + CurrentPosy + " walker hit an invalid spot");
                            currentWalker.dir = RandomDirection(currentWalker);
                            if (UseDebug)
                                Debug.Log("changing invalid walker direction to " + currentWalker.dir);
                        } while (walkerHitInvalid(currentWalker));

                    //change walker pos
                    currentWalker.pos += currentWalker.dir;
                    if (UseDebug) 
                    Debug.Log(" walker moving to " + currentWalker.pos + " in direction of " + currentWalker.dir);


                    //create room and adds it to array
                    walkers[i] = currentWalker;

                    if (currentWalker.mainwalker && (currentWalker.steps >7 || (float)NumberOfFloors() / (float)grid.Length > percentToFill))
                    {
                        SpawnBossRoom((int)currentWalker.pos.x, (int)currentWalker.pos.y);
                        connectRoom(CurrentPosx, CurrentPosy, CurrentRoom, currentWalker.dir);
                        iterations = iterationSteps;
                        break;
                    }
                    spawnAtIndex((int)currentWalker.pos.x, (int)currentWalker.pos.y);
                    if (UseDebug)
                        Debug.Log("spawning room prefab at " + currentWalker.pos);
                    connectRoom(CurrentPosx, CurrentPosy, CurrentRoom, currentWalker.dir);
                    if (UseDebug)
                        Debug.Log("connecting room at " + CurrentPosx + ", " + CurrentPosy + " to " + currentWalker.dir);


                    //check if walker's stuck and move it if it is

                    if (WalkerStuck((int)currentWalker.pos.x, (int)currentWalker.pos.y))
                    {
                        if (UseDebug)
                            Debug.Log("walker Stuck at " + currentWalker.pos.x + ", " + currentWalker.pos.y);
                        currentWalker.pos = TransferWalker(currentWalker);
                        walkers[i] = currentWalker;
                        if (UseDebug) 
                        Debug.Log("walker in list's pos is " + walkers[i].pos);
                    }


                    //chance to change the tiles direction
                    if (Random.value < chanceWalkerChangeDir && !currentWalker.Finished)
                    {
                        if (UseDebug)
                            Debug.Log("changing direciton of walke");

                        GameObject DirectionTile;
                        do
                        {
                            if (UseDebug)
                            {
                                Debug.Log("chance to change walker direction loop");
                                Debug.Log("previous direction was " + currentWalker.dir);
                            }
                            currentWalker.dir = RandomDirection(currentWalker);
                            if (UseDebug)
                                Debug.Log("changing direciton of walker at " + currentWalker.pos + " to " + currentWalker.dir);
                            DirectionTile = roomGrid[(int)currentWalker.pos.x + (int)currentWalker.dir.x, (int)(currentWalker.pos.y + currentWalker.dir.y)];
                            walkers[i] = currentWalker;

                            //repeat if there's a tile where this walker is going
                        } while (DirectionTile != null);

                    }
                    if (UseDebug)
                        Debug.Log("walker direction is " + walkers[i].dir);
                    NewWalker(currentWalker);
                    if (UseDebug)
                        Debug.Log("walker count is " + walkers.Count);
                }

                iterations++;
                if (UseDebug)
                    Debug.Log("iteration num is " + iterations + " " + (iterationSteps - iterations) + " iterations left");
            } while (iterations < iterationSteps);
        }

        int NumberOfFloors()
        {
            int count = 0;
            foreach (GameObject space in roomGrid)
            {
                if (space != null)
                {
                    count++;
                }
            }
            return count;
        }


        Vector2 TransferWalker(RandomWalker walker)
        {
            Vector2 Newpos;
            List<Vector2> WalkerPositions = new List<Vector2>();
            for (int x = 0; x < roomGrid.GetLength(0); x++)
            {
                for (int y = 0; y < roomGrid.GetLength(1); y++)
                {
                    //if there's a room in the space
                    if (roomGrid[x, y] != null)
                    {
                        Vector2 walkerpos = new Vector2(x, y);
                        WalkerPositions.Add(walkerpos);
                    }
                }
            }
            do
            {
                //pick a random position for walker to transfer to as long as it is not stuck
                if (UseDebug)
                    Debug.Log("Testing new pos to transfer walker");
                int randomwalkerpos = Random.Range(0, WalkerPositions.Count - 1);
                Newpos = WalkerPositions[randomwalkerpos];
                //might have to remove previous direction
            } while (WalkerStuck((int)Newpos.x, (int)Newpos.y));
            if (UseDebug)
                Debug.Log("new walker position is " + Newpos);
            return Newpos;
        }

        //if walker is going to run into a spot it cant
        bool walkerHitInvalid(RandomWalker walker)
        {
            if (OutOfBounds((int)(walker.pos.x + walker.dir.x), (int)(walker.pos.y + walker.dir.y)))
            {
                return true;
            }
            GameObject DirectionTile = roomGrid[(int)(walker.pos.x + walker.dir.x), (int)(walker.pos.y + walker.dir.y)];
            if (DirectionTile != null)
                return true;
            else
                return false;
        }

        Vector2 RandomDirection(RandomWalker walker)
        {
            if (UseDebug)
                Debug.Log("getting random dir");
            Vector2 newdir;
            Vector2 oldDir = walker.dir;
            int xpos, ypos;
            int loops = 0;
            do
            {
                loops++;
                int choice = Mathf.FloorToInt(Random.value * 3.99f);
                //print("gettting a random direction");
                switch (choice)
                {
                    case 0:
                        newdir = Vector2.down;
                        xpos = (int)(walker.pos.x + newdir.x);
                        ypos = (int)(walker.pos.y + newdir.y);
                        if (UseDebug)
                            Debug.Log("New dir is going down, next pos is " + xpos + ", " + ypos);
                        break;
                    case 1:
                        newdir = Vector2.left;
                        xpos = (int)(walker.pos.x + newdir.x);
                        ypos = (int)(walker.pos.y + newdir.y);
                        if (UseDebug)
                            Debug.Log("New dir is going left next pos is " + xpos + ", " + ypos);
                        break;
                    case 2:
                        newdir = Vector2.up;
                        xpos = (int)(walker.pos.x + newdir.x);
                        ypos = (int)(walker.pos.y + newdir.y);
                        if (UseDebug)
                            Debug.Log("New dir is going up next pos is " + xpos + ", " + ypos);
                        break;
                    default:
                        newdir = Vector2.right;
                        xpos = (int)(walker.pos.x + newdir.x);
                        ypos = (int)(walker.pos.y + newdir.y);
                        if (UseDebug)
                            Debug.Log("New dir is going right next pos is " + xpos + ", " + ypos);
                        break;
                }
                if (xpos < 0 || xpos > levelWidth - 1)
                {
                    if (UseDebug)
                        Debug.Log("xpos is out of bounds");
                }
                if (ypos < 0 || ypos > levelHeight - 1)
                {
                    if (UseDebug)
                        Debug.Log("ypos is out of bounds");
                }
                if (newdir == oldDir)
                {
                    if (UseDebug)
                        Debug.Log("new dir is old direction, old direction is " + oldDir);
                }
                if (loops > 1000)
                {
                    if (UseDebug)
                        Debug.Log("random dir is stuck");
                    break;
                }

            } while (xpos < 0 || xpos > levelWidth - 1 || ypos < 0 || ypos > levelHeight - 1 || newdir == oldDir);
            if (UseDebug)
                Debug.Log("returning new dir " + newdir);
            return newdir;
        }

        //if all rooms are true
        bool WalkerStuck(int posx, int posy)
        {
            bool RightRoom = true;
            bool LeftRoom = true;
            bool topRoom = true;
            bool BottomRoom = true;
            if (UseDebug)
                Debug.Log("checking if walker is stuck");
            //if there's a room in a valid spot return true
            if (!OutOfBounds(posx + 1, posy))
            {
                RightRoom = roomGrid[posx + 1, posy] == null ? false : true;
                if (RightRoom)
                {
                    if (UseDebug)
                        Debug.Log("there's a room to the rightof " + posx + ", " + posy);
                }
            }
            if (!OutOfBounds(posx - 1, posy))
            {
                LeftRoom = roomGrid[posx - 1, posy] == null ? false : true;
                if (LeftRoom)
                {
                    if (UseDebug)
                        Debug.Log("there's a room to the leftof " + posx + ", " + posy);
                }
            }
            if (!OutOfBounds(posx, posy + 1))
            {
                topRoom = roomGrid[posx, posy + 1] == null ? false : true;
                if (topRoom)
                {
                    if (UseDebug)
                        Debug.Log("there's a room to the top of " + posx + ", " + posy);
                }
            }
            if (!OutOfBounds(posx, posy - 1))
            {
                BottomRoom = roomGrid[posx, posy - 1] == null ? false : true;
                if (BottomRoom)
                {
                    if (UseDebug)
                        Debug.Log("there's a room to the bottom of " + posx + ", " + posy);
                }
            }

            //if there's rooms all around current room then walker stuck returns true
            if (RightRoom && LeftRoom && topRoom && BottomRoom)
            {
                return true;
            }

            //other

            if(UseDebug)
            Debug.Log("walker is not stuck");
            return false;
        }

        bool OutOfBounds(int posx, int posy)
        {
            if (posx < 0 || posx > levelWidth - 1 || posy < 0 || posy > levelHeight - 1)
                return true;
            return false;

        }

        void deactivateRooms()
        {
            foreach (GameObject room in roomGrid)
            {
                if (room == null)
                    continue;

                room.SetActive(false);
            }
        }

        void finishWalker(RandomWalker CurrentWalker)
        {
            //chance: destroy Walker

            if (CurrentWalker.mainwalker)
                return;
            if (Random.value < chanceWalkerDestoy && CurrentWalker.steps > 2)
            {
                if (UseDebug)
                    Debug.Log("ending walker at " + CurrentWalker.pos);
                CurrentWalker.Finished = true;
            }

        }

        public void connectRoom(int posx, int posy, GameObject Room, Vector2 dir)
        {
            if (dir == Vector2.right)
            {
                GameObject ThisRoom = Room;
                GameObject NextRoom = roomGrid[posx + 1, posy];
                RoomGen ThisRoomGen = Room.GetComponent<RoomGen>();
                RoomGen NextRoomGen = NextRoom.GetComponent<RoomGen>();
                //if player goes to the right entrance, player moves to western door of it's neighbor
                Transform WestNeighborEntrance = GetDoor(left, posx + 1, posy);
                Transform ThisEastEntrance = GetDoor(right, posx, posy);
                passInWarpPoints(ThisRoomGen, WestNeighborEntrance, 'w');
                passInWarpPoints(NextRoomGen, ThisEastEntrance, 'e');
                if (ThisRoom != null)
                    ThisRoomGen.OpenDoor(right);
                NextRoomGen.OpenDoor(left);
            }
            if (dir == Vector2.left)
            {
                GameObject ThisRoom = roomGrid[posx, posy].gameObject;
                GameObject NextRoom = roomGrid[posx - 1, posy];
                RoomGen ThisRoomGen = Room.GetComponent<RoomGen>();
                RoomGen NextRoomGen = NextRoom.GetComponent<RoomGen>();
                //if player goes to the right entrance, player moves to western door of it's neighbor
                Transform eastNeightborEntrance = GetDoor(right, posx - 1, posy);
                Transform ThisWestEntrance = GetDoor(right, posx, posy);
                passInWarpPoints(ThisRoomGen, eastNeightborEntrance, 'e');
                passInWarpPoints(NextRoomGen, ThisWestEntrance, 'w');
                if (ThisRoom != null)
                    ThisRoomGen.OpenDoor(left);
                NextRoomGen.OpenDoor(right);

            }
            if (dir == Vector2.up)
            {
                GameObject ThisRoom = roomGrid[posx, posy].gameObject;
                GameObject NextRoom = roomGrid[posx, posy + 1];
                RoomGen ThisRoomGen = Room.GetComponent<RoomGen>();
                RoomGen NextRoomGen = NextRoom.GetComponent<RoomGen>();
                //if player goes to the right entrance, player moves to western door of it's neighbor
                Transform South = GetDoor(down, posx, posy + 1);
                Transform ThisNorthEntrance = GetDoor(up, posx, posy);
                passInWarpPoints(ThisRoomGen, South, 's');
                passInWarpPoints(NextRoomGen, ThisNorthEntrance, 'n');

                if (ThisRoom != null)
                    ThisRoomGen.OpenDoor(up);
                NextRoomGen.OpenDoor(down);

            }
            if (dir == Vector2.down)
            {
                GameObject ThisRoom = roomGrid[posx, posy].gameObject;
                GameObject nextRoom = roomGrid[posx, posy - 1];
                RoomGen ThisRoomGen = Room.GetComponent<RoomGen>();
                RoomGen NextRoomGen = nextRoom.GetComponent<RoomGen>();
                //if player goes to the right entrance, player moves to western door of it's neighbor
                Transform North = GetDoor(up, posx, posy - 1);
                Transform ThisSouthEntrance = GetDoor(down, posx, posy);
                passInWarpPoints(ThisRoomGen, North, 'n');
                passInWarpPoints(NextRoomGen, ThisSouthEntrance, 's');

                if (ThisRoom != null)
                    ThisRoomGen.OpenDoor(down);
                NextRoomGen.OpenDoor(up);
            }
        }

        //deactivates entrance gameobjects depending on neighbors of room
        //also places room warps on open door nodes connecting to other rooms
        //void OpenDoors()
        //{
        //    //go through entire grid of rooms
        //    for (int x = 0; x < levelWidth; x++)
        //    {
        //        for (int y = 0; y < levelHeight; y++)
        //        {
        //            if (grid[x, y] != LevelTile.floor)
        //                continue;

        //            if (x + 1 < levelWidth)
        //            {
        //                if (grid[x + 1, y] == LevelTile.floor)
        //                {
        //                    //get door right
        //                    GameObject ThisRoom = roomGrid[x, y].gameObject;

        //                    //if player goes to the right entrance, player moves to western door of it's neighbor
        //                    Transform WestNeighborEntrance = GetDoor(left, x + 1, y);
        //                    passInWarpPoints(ThisRoom, WestNeighborEntrance, 'w');
        //                    if (ThisRoom != null)
        //                        ThisRoom.GetComponent<RoomGen>().OpenDoor(right);
        //                }
        //            }


        //            if (x - 1 >= 0)
        //            {
        //                if (grid[x - 1, y] == LevelTile.floor)
        //                {
        //                    //get door left
        //                    GameObject ThisRoom = roomGrid[x, y].gameObject;
        //                    Transform EastNeighborEntrance = GetDoor(right, x - 1, y);
        //                    passInWarpPoints(ThisRoom, EastNeighborEntrance, 'e');
        //                    if (ThisRoom != null)
        //                        ThisRoom.GetComponent<RoomGen>().OpenDoor(left);
        //                }
        //            }


        //            if (y + 1 < levelHeight)
        //            {
        //                if (grid[x, y + 1] == LevelTile.floor)
        //                {
        //                    //get door up
        //                    GameObject ThisRoom = roomGrid[x, y].gameObject;
        //                    Transform SouthNeighborEntrance = GetDoor(down, x, y + 1);
        //                    passInWarpPoints(ThisRoom, SouthNeighborEntrance, 's');
        //                    if (ThisRoom != null)
        //                        ThisRoom.GetComponent<RoomGen>().OpenDoor(up);
        //                }
        //            }


        //            if (y - 1 >= 0)
        //            {
        //                if (grid[x, y - 1] == LevelTile.floor)
        //                {
        //                    //get door down
        //                    GameObject ThisRoom = roomGrid[x, y].gameObject;
        //                    Transform NorthNeighborEntrance = GetDoor(up, x, y - 1);
        //                    passInWarpPoints(ThisRoom, NorthNeighborEntrance, 'n');
        //                    if (ThisRoom != null)
        //                        ThisRoom.GetComponent<RoomGen>().OpenDoor(down);
        //                }
        //            }
        //        }
        //    }
        //}

        //returns child game object(door) from the index of roomGrid
        //get a door from a room in a position

        Transform GetDoor(char dir, int x, int y)
        {
            switch (dir)
            {
                case 'u':
                    return roomGrid[x, y].transform.GetChild(10);
                case 'd':
                    return roomGrid[x, y].transform.GetChild(11);
                case 'l':
                    return roomGrid[x, y].transform.GetChild(8);
                case 'r':
                    return roomGrid[x, y].transform.GetChild(9);
            }
            return null;
        }

        //method to pass in entrance points into roomGen class
        void passInWarpPoints(RoomGen room, Transform entrancepositon, char Direction)
        {
            switch (Direction)
            {
                case 'n':
                    room.NorthNeighborPoint = entrancepositon;
                    break;
                case 's':
                    room.SouthNeighborPoint = entrancepositon;
                    break;
                case 'w':
                    room.WestNeighborPoint = entrancepositon;
                    break;
                case 'e':
                    room.EastNeighborPoint = entrancepositon;
                    break;
            }
        }

        void NewWalker(RandomWalker ParentWalker)
        {
            //chance: spawn new Walker
            //refactor to avoid walker from having same direction as original walker
            int numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    RandomWalker walker = new RandomWalker();
                    walker.pos = walkers[i].pos;
                    LevelTile DirectionTile;
                    do
                    {
                        walker.dir = RandomDirection(ParentWalker);
                        walker.mainwalker = false;
                        DirectionTile = grid[(int)walker.pos.x + (int)walker.dir.x, (int)(walker.pos.y + walker.dir.y)];
                    } while (DirectionTile == LevelTile.floor);
                    walkers.Add(walker);
                }
            }
        }


        //creates walls inside of rooms and changes the grid inside of the room
        void GenerateRoomAssets()
        {
            foreach (GameObject room in roomGrid)
            {
                if (room == null)
                    continue;
                RoomGen thisRoomGen = room.GetComponent<RoomGen>();
                //thisRoomGen.EntrancePointsToGridCoordinates();
                //random ammount of rooms from 0 to 4
                int rooms = Random.Range(0, 4);
                if (rooms != 0)
                    for (int i = 0; i < rooms; i++)
                    {
                        thisRoomGen.makewall();
                    }
                //thisRoomGen.ChangeGridEmpty();
                //thisRoomGen.SpawnEnemy();
            }
        }

        public static int GetRoomInOrder(Transform pos)
        {
            for (int i = 0; i < RoomList.Count ; i++)
            {
                if (pos == RoomList[i])
                    return i;
            }

            return 0;
        }

        void spawnAtIndex(int posx, int posy)
        {
            roomGrid[posx, posy] = Spawn(posx, posy, roombase);
        }

        void SpawnBossRoom(int posx, int posy)
        {
            roomGrid[posx, posy] = Spawn(posx, posy, BossRoom);

        }

        //creates room prefabs wherever a non empty tile is and applies index positions
        void SpawnLevel()
        {
           
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    switch (grid[x, y])
                    {
                        case LevelTile.empty:
                            break;
                        case LevelTile.floor:
                            GameObject room = Spawn(x, y, roombase);
                            roomGrid[x, y] = room;
                            break;
                    }
                }
            }
        }

        GameObject Spawn(float x, float y, GameObject toSpawn)
        {
            GameObject NewRoom = Instantiate(toSpawn, new Vector3(x, y, 0) * gridoffset, Quaternion.identity, transform);
            if (UseDebug)
                Debug.Log("spawning boss room at " +( new Vector3(x, y, 0) * gridoffset));
            RoomList.Add(NewRoom.transform);
            return NewRoom;
        }

        public void SetupLevels()
        {
            Setup();
            CreateFloors();
            roomGrid[5, 0].transform.position = LevelPivot.position + LevelPivotOffset;
            roomGrid[5, 0].gameObject.SetActive(true);
        }

        public Vector2 getspawn()
        {
            throw new System.NotImplementedException();
        }
    }
}
