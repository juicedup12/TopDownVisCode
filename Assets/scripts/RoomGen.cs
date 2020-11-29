using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

namespace topdown {
    public class RoomGen : MonoBehaviour
    {
        //local space vectors
        public float EdgeOffset;
        public Vector2 gridworldsize;
        public Vector3[] RightEntrancepoints;
        public Vector3[] LeftEntrancepoints;
        public Vector3[] TopEntrancepoints;
        public Vector3[] BottomEntrancepoints;
        public Material unlit;
        public RoomGenScriptableObj roomdata;
        public float TileMultiplier;
        public delegate void DoorAction(Vector2 pos ,Vector2 dir);
        public static event DoorAction OnDoor;
        Tween walltween;
        public bool SpawnOnKey = false;
        public LayerMask wallmask;
        Transform SptriteMskTransofrm;

        //world space positions of neighboring enterances
        public Transform EastNeighborPoint, WestNeighborPoint, NorthNeighborPoint, SouthNeighborPoint;

        delegate void activedel (GameObject gobj);

        public float TweenWait = 2;

        List<Room.Roomtile> GridEntrancePoints = new List<Room.Roomtile>();
        int gridsizeX, gridsizeY;

        public float noderadius;
        float nodediameter;
        Vector3 worldbottomleft;
        Room.Roomtile[,] grid;
        Vector3[,] TileGrid;
        public bool drawgrid = false;
        Vector2 startwall;
        Vector2 endwall;
        List<Room> rooms = new List<Room>();
        public SpriteRenderer LeftEntrance, RightEntrance, TopEntrance, BottomEntrance;
        public SpriteMask maskleft, maskright, masktop, maskbottom;
        public GameObject verticalwallprefab;
        public GameObject horizontalwallprefab;
        List<GameObject> roomgameobjects = new List<GameObject>();
        public GameObject[] enemyPrefabs;
        player player;
        public List<Transform> TweenObjects;
        CopyNodeGrid nodegrid;
        public GameObject InnerRoomLight;
        
        

        void Awake()
        {
            
            nodediameter = noderadius * 2;
            gridsizeX = Mathf.RoundToInt(gridworldsize.x / nodediameter);
            gridsizeY = Mathf.RoundToInt(gridworldsize.y / nodediameter);
            worldbottomleft = transform.position - Vector3.right * gridworldsize.x / 2 - Vector3.up * gridworldsize.y / 2;
            player = GameObject.Find("Player ").GetComponent<player>();
        }

        //Start is called before the first frame update
        void Start()
        {
            nodegrid = GetComponentInChildren<CopyNodeGrid>();
            creategrid();
            createTilegrid();
            EntrancePointsToGridCoordinates();

           
            //put walls in a sequence
            WallsToSeq();

            makewall();
            makewall();
            ChangeGridEmpty();
            ChangeGridInRoom();
            StartCoroutine( StartTweening());

            //makewall();
            //SpawnEnemy();

            //int rooms = Random.Range(0, 4);
            //if (rooms != 0)
            //    for (int i = 0; i < rooms; i++)
            //    {
            //        makewall();
            //    }
            
            //PlaceFloorInRoom();

            //LightsForLoop();
            //OuterWallsFallInTween();
            //RotateRooms();
            //RotateFloorTiles();
        }

        private void Update()
        {
            //if(AtEnterance(playerPos.position))
            //{
            //    Debug.Log("player at entrance");
            //    OnEntrancePoint();
            //}

            //if (Input.GetKeyDown(KeyCode.Mouse0))
            //{
            //    //foreach (Room.Roomtile enterance in GridEntrancePoints)
            //    //{
            //    //    float distToEnterance = Vector3.Distance(playerPos.position, enterance.point);
            //    //    Debug.Log(playerPos.position + "Player is " + distToEnterance + " away from " + (Vector3)enterance.point);
            //    //}
                
            //}

            //if (Input.GetKeyDown(KeyCode.Space) && SpawnOnKey)
            //{
            //    //makewall();
            //    SpawnEnemy();
                
            //}
        }

        //creates tiles to pass into room creation
        void creategrid()
        {
            grid = new Room.Roomtile[gridsizeX, gridsizeY];
            Vector3 worldbottomleft = transform.position - Vector3.right * gridworldsize.x / 2 - Vector3.up * gridworldsize.y / 2;
            //print("node grid bottom left = " + worldbottomleft);
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

        void createTilegrid()
        {
            float GridSizeScaler = 1.2f;
            float TileRadius =  TileMultiplier;
            float TileDiameter = TileRadius * 2 ;
            int TileGridSizeX = Mathf.RoundToInt(gridworldsize.x * GridSizeScaler / TileDiameter );
            int TileGridSizeY = Mathf.RoundToInt(gridworldsize.y * GridSizeScaler / TileDiameter );
            TileGrid = new Vector3[TileGridSizeX, TileGridSizeY];
            Vector3 worldbottomleft = transform.position - Vector3.right * (gridworldsize.x * GridSizeScaler) / 2 - Vector3.up * (gridworldsize.y * GridSizeScaler) / 2;
            //print("tilegrid bottom left is " + worldbottomleft);
            for (int x = 0; x < TileGridSizeX; x++)
            {
                for (int y = 0; y < TileGridSizeY; y++)
                {
                    Vector3 worldpoint = worldbottomleft + Vector3.right * (x * TileDiameter + TileRadius)
                        + Vector3.up * (y * TileDiameter + TileRadius);
                    //CodeMonkey.CMDebug.Text(worldpoint.ToString(), worldpoint, fontSize: 40,textAnchor: TextAnchor.MiddleCenter, sortingOrder: 0);
                    TileGrid[x, y] = worldpoint;
                }
            }
        }

        public void EntrancePointsToGridCoordinates()
        {
            //if right sprite renderer is disabled
            if (!RightEntrance.enabled)
                foreach (Vector3 point in RightEntrancepoints)
                {

                    //Debug.Log(point, this);
                    Room.Roomtile tile = NodeFromWorldPoint(point);
                    //Debug.Log("adding right entrance points to grid " + tile.point);
                    GridEntrancePoints.Add(tile);
                    tile.onEntrance = true;
                }
                if (!LeftEntrance.enabled)
                for (int i = 0; i < LeftEntrancepoints.Length; i++)
                {
                    //print("left points");

                    Room.Roomtile tile = NodeFromWorldPoint(LeftEntrancepoints[i]);
                    //Debug.Log("adding Left entrance points to grid " + tile.point, this);
                    GridEntrancePoints.Add(tile);
                    tile.onEntrance = true;
                }
                if (!TopEntrance.enabled)
                for (int i = 0; i < TopEntrancepoints.Length; i++)
                {
                    //print("top points");

                    Room.Roomtile tile = NodeFromWorldPoint(TopEntrancepoints[i]);
                    GridEntrancePoints.Add(tile);
                    //Debug.Log("adding Top entrance points to grid " + (TopEntrancepoints[i]));
                    tile.onEntrance = true;
                }
                if (!BottomEntrance.enabled)
                for (int i = 0; i < BottomEntrancepoints.Length; i++)
                {
                    //print("bottoms points");

                    // Debug.Log("adding Bottom entrance points to grid" + BottomEntrancepoints[i]);
                    GridEntrancePoints.Add(NodeFromWorldPoint(BottomEntrancepoints[i]));
                }
        }

        void AddLightsToHallway()
        {
            //if (!RightEntrance.enabled)
            //    foreach (Vector3 point in RightEntrancepoints)
            //    {
            //    }

            
                for (int i = 0; i < LeftEntrancepoints.Length; i +=3)
                {
                    bool leftEntranceOpen = false;
                    Room.Roomtile tile = NodeFromWorldPoint(LeftEntrancepoints[i]);
                    if(!tile.insideRoom)
                    {
                        Debug.Log("tile by left entrance is empty, tilepoint is " + tile.point);
                        leftEntranceOpen = false;
                    }
                    else if(tile.insideRoom)
                    {
                        Debug.Log("Tile by left isn't emptytilepoint is " + tile.point);
                        leftEntranceOpen = true;
                    }

                    if(leftEntranceOpen)
                    {
                        int leftTileIndexy = Mathf.FloorToInt(gridsizeY / 2);
                        Room.Roomtile CurrenTile = grid[0, leftTileIndexy];
                        for (int x = 0; x < gridsizeX; x++)
                        {
                            if(IsTileEmpty(CurrenTile))
                            {

                            }
                        }
                    }

                }

            //if (!TopEntrance.enabled)
            //    for (int i = 0; i < TopEntrancepoints.Length; i++)
            //    {
            //    }


            //if (!BottomEntrance.enabled)
            //    for (int i = 0; i < BottomEntrancepoints.Length; i++)
            //    {
            //    }

        }


        bool IsTileEmpty(Room.Roomtile tile)
        {
            if (tile.insideRoom || !tile.empty)
                return false;

            return true;
        }

        //adds lights in a grid
        void LightsForLoop()
        {
            //int EmtpyTileIndexY = int.MaxValue;
            //int EmptyTileIndexX = int.MaxValue;
            Room.Roomtile CurrentTile;
            Sequence seq = DOTween.Sequence();
            GameObject lights = new GameObject("Lights");
            lights.transform.parent = gameObject.transform;
            for (int x = 0; x < gridsizeX; x += 4)
            {
                for (int y = 0; y < gridsizeY; y+= 3)
                {
                    CurrentTile = grid[x, y];
                    if (IsTileEmpty(CurrentTile))
                    {
                        ////only assign empty tile index for the first time
                        //if (EmtpyTileIndexY == int.MaxValue)
                        //{
                        //    print("Assigning emtpyt tile indexes " + x + " and " + y);
                        //    EmtpyTileIndexY = y;
                        //    EmptyTileIndexX = x;
                        //}

                        //if (y == EmtpyTileIndexY   )
                        
                        Light2D light = Instantiate(roomdata.HallwayLight, CurrentTile.point, Quaternion.identity, lights.transform).GetComponent<Light2D>();
                        
                        Ease lightease = Ease.InCirc;
                        seq.Insert((x * .3f) + y * .5f,(DOTween.To(() => light.intensity, l => light.intensity = l, (float).7, .5f).OnStart ( () => light.gameObject.SetActive(true)).SetEase(lightease))); 
                        //print(CurrentTile.point + "is on the same X index of " + EmptyTileIndexX);
                        //EmtpyTileIndexY += 2;
                        
                    }
                    //else
                    //{
                    //    if (EmtpyTileIndexY != int.MaxValue && EmptyTileIndexX != int.MaxValue)
                    //    {
                    //        x += 2;
                    //    }
                    //    EmtpyTileIndexY = int.MaxValue;
                    //    EmptyTileIndexX = int.MaxValue;
                        

                    //}
                    
                }
            }
        }
        

        public Sequence SpawnEnemy()
        {
            if (enemyPrefabs.Length < 1) return null;
            int enemycount = 0;

            Sequence SpawnSeq = DOTween.Sequence();

            Debug.Log("this room is number " + LevelCreator.GetRoomInOrder(transform) + " in the list", gameObject);
            int ListPos = LevelCreator.GetRoomInOrder(transform);
            float IndexFraction = (float)ListPos / (float)8;
            //Debug.Log("index fraction is " + IndexFraction + " in list pos " + ListPos);
            float MaxEnemySpawn =  (Random.Range(10, 20)  * IndexFraction) + 1;
            //Debug.Log("Max enemy spawn is " + MaxEnemySpawn, gameObject);
            int EnemySpawnAmount =  Random.Range(5,(int) MaxEnemySpawn);
            //Debug.Log(ListPos + " Room spawning " + EnemySpawnAmount);
            if(EnemySpawnAmount >0)
            do
            {
                
                Room.Roomtile TileToSpawn = GetRandomRoomTile();
                if (TileToSpawn.empty == true)
                {
                        Debug.Log("Enemy tiles spawn point" + TileToSpawn.point + " is " + TileToSpawn.empty);
                    int enemynum = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemy = Instantiate(enemyPrefabs[enemynum], TileToSpawn.point, Quaternion.identity);Enemy enemyref = enemy.GetComponent<Enemy>();
                    enemy.transform.parent = gameObject.transform;
                    SpawnSeq.Insert(enemycount, enemy.transform.DOPunchScale(enemy.transform.localScale + new Vector3(5,-.6f,0), .8f, 1, 1f).SetEase(roomdata.enemyScaleEase).OnStart(() => enemy.SetActive(true)));
                    SpawnSeq.Insert(enemycount, enemy.transform.DOPunchPosition( new Vector2(0, -.4f), .4f, 1,1).SetEase(roomdata.enemyLowerEase));
                    //Debug.Log("doing punch pos to " + (transform.localPosition + new Vector3(0, -1f)) + "from " + transform.localPosition + "where trans.pos is " +  )
                    SpawnSeq.Insert( enemycount + .2f , enemy.transform.DOPunchPosition(new Vector3(0, .6f, 0), .4f, 1, 1).SetEase(roomdata.enemyHopEase));
                    enemycount++;
                    TileToSpawn.empty = false;
                    StartCoroutine(SetEnemyPatrolPoints(enemy.GetComponent<Enemy>(), TileToSpawn.point));
                    

                }
            } while (enemycount < EnemySpawnAmount);
            //SpawnSeq.OnKill( () =>
            //{
            //    print("spawn enemy seq complete");
            //    player.SequenceDone = true;
            //    Gmanager.instance.ReturnToPlayer();
            //});
            return SpawnSeq;
        }

        Room.Roomtile GetRandomRoomTile()
        {
            int randomx = Random.Range(0, grid.GetLength(0));
            int randomy = Random.Range(0, grid.GetLength(1));
            return grid[randomx, randomy]; 
        }

        IEnumerator SetEnemyPatrolPoints(Enemy enemy, Vector2 TilePoint)
        {
            yield return new WaitForSeconds(6);
            enemy.CantMove = true;
            float PatrolDirChance = Random.value;
            float patrolPointDist;
            Vector2[] patrolpoints;
            int iterations = 0;
            do
            {
                iterations++;
                patrolpoints = PatrolDirChance < .5 ? setVerticalPatrolPoints(enemy, TilePoint) : setHorizontalPatrolPoint(enemy, TilePoint);
                patrolPointDist = Vector2.Distance(patrolpoints[0], patrolpoints[1]);
                if(iterations > 20)
                {
                    patrolpoints[1] = GetRandomEmptyRoomTile();
                    print("breaking patrol assign while loop");
                    break;
                }
            } while (patrolPointDist < 1);

            enemy.Patrolpoints = patrolpoints;
            Debug.Log("Patrol points are " + patrolpoints[0] + " " + patrolpoints[1]);
            yield return new WaitForSeconds(2);
            enemy.CantMove = false;
            yield return null;
        }

        Vector2 GetRandomEmptyRoomTile()
        {
            int RandomX;
            int RandomY;
            do
            {
                RandomX = Random.Range(0, grid.GetLength(0) - 1);
                RandomY = Random.Range(0, grid.GetLength(1) - 1);
            }
            while (grid[RandomX, RandomY].empty);
            return grid[RandomX, RandomY].point;
        }

        Vector2[] setVerticalPatrolPoints(Enemy enemy, Vector2 TilePoint)
        {
            Vector2[] patrolpoints = new Vector2[2];
            RaycastHit2D TopHit;
            RaycastHit2D BottomHit;

            TopHit = Physics2D.Raycast(enemy.transform.position, Vector2.up, 20, wallmask);
            Debug.Log("getting raycast from " + TilePoint + " and " + (TilePoint + Vector2.up * 20), enemy);
            if (TopHit.collider != null)
                Debug.Log("tophit hit " + TopHit.collider.name + " patrol point 0 is " + TopHit.point, TopHit.collider.gameObject);
            else
                Debug.Log("didn't hit anything, patrol point is " + new Vector2(TilePoint.x, transform.position.y + gridworldsize.y / 2), enemy);
            //might have to use transform.position.y + Vector3.up * gridworldsize.y / 2
            BottomHit = Physics2D.Raycast(TilePoint, Vector2.down, 20, wallmask);
            Debug.Log("getting bottom raycass from " + TilePoint + " and " + (TilePoint + Vector2.down * 20) + " bitmask is " + LayerMask.NameToLayer("wall"), enemy);
            Debug.Log(BottomHit.collider == null ? "Bottom raycast hit nothing, patrol point is " + new Vector2(TilePoint.x, transform.position.y - (gridworldsize.y + .3f) / 2) : "bottom raycast hit " + BottomHit.collider.name + " patrol point 1 is " + BottomHit.point);

            float PointPadding = .25f;
            patrolpoints[0] = TopHit.collider == null ? new Vector2(TilePoint.x, transform.position.y + (gridworldsize.y - .3f) / 2) : TopHit.point - new Vector2(0, PointPadding);
            patrolpoints[1] = BottomHit.collider == null ? new Vector2(TilePoint.x, transform.position.y - (gridworldsize.y + .3f) / 2) : BottomHit.point + new Vector2(0, PointPadding);
            return patrolpoints;
        }

        Vector2[] setHorizontalPatrolPoint(Enemy enemy, Vector2 TilePoint)
        {
            Vector2[] patrolpoints = new Vector2[2];
            RaycastHit2D LeftHit;
            RaycastHit2D RightHit;

            LeftHit = Physics2D.Raycast(enemy.transform.position, Vector2.left, 20, wallmask);
            Debug.Log("getting Left raycast from " + TilePoint + " and " + (TilePoint + Vector2.up * 20), enemy);
            if (LeftHit.collider != null)
                Debug.Log("Left hit " + LeftHit.collider.name + " patrol point 0 is " + LeftHit.point, LeftHit.collider.gameObject);
            else
                Debug.Log("didn't hit anything, patrol point is " + new Vector2(TilePoint.x, transform.position.y + gridworldsize.y / 2), enemy);
            //might have to use transform.position.y + Vector3.up * gridworldsize.y / 2
            RightHit = Physics2D.Raycast(TilePoint, Vector2.right, 20, wallmask);
            Debug.Log("getting Right raycass from " + TilePoint + " and " + (TilePoint + Vector2.down * 20) + " bitmask is " + LayerMask.NameToLayer("wall"), enemy);
            Debug.Log(RightHit.collider == null ? "Right raycast hit nothing, patrol point is " + new Vector2(TilePoint.x, transform.position.y - (gridworldsize.y + .3f) / 2) : "right raycast hit " + RightHit.collider.name + " patrol point 1 is " + RightHit.point);

            float PointPadding = .25f;
            patrolpoints[0] = LeftHit.collider == null ? new Vector2(TilePoint.x, transform.position.y + (gridworldsize.y - .3f) / 2) : LeftHit.point + new Vector2(PointPadding, 0);
            patrolpoints[1] = RightHit.collider == null ? new Vector2(TilePoint.x, transform.position.y - (gridworldsize.y + .3f) / 2) : RightHit.point + new Vector2(-PointPadding, 0);
            return patrolpoints;
        }

        public void makewall()
        {
            Room wall = new Room(gridsizeX, gridsizeY, grid, rooms, GridEntrancePoints, verticalwallprefab, horizontalwallprefab, gameObject, roomdata.InsideOfRoomSortLyaer);
            GameObject roombase = wall.Makesegments();
            roomgameobjects.Add(roombase);
            rooms.Add(wall);

        }

        public static TweenCallback Setactive(GameObject gobject)
        {
            return () => gobject.SetActive(true);
        }

        public void ActivateSpears(GameObject obj)
        {
            StartCoroutine(Spears(obj));
        }

        IEnumerator Spears(GameObject obj )
        {

            for (int x = 0; x < gridsizeX; x++)
            {

                if(grid[x, 1].empty)
                {
                    Vector2 posToInstantiate = grid[x, 0].point;
                    Instantiate(obj, position: posToInstantiate, Quaternion.identity);
                    yield return new WaitForSeconds(.5f);
                }
            }
            

        }


        IEnumerator StartTweening()
        {
            //yield return new WaitForSeconds(.5f);

            Sequence FullSeq = DOTween.Sequence();
            FullSeq.Join( OuterWallsFallInTween());
            //FullSeq.Join(RotateRooms());
            //FullSeq.Join(RotateFloorTiles());
            //float i = 3.5f;
            //foreach (Sequence seq in PlaceFloorInRoom())
            //{
                
            //    FullSeq.Insert(i,seq);
            //    i += i *.3f;
            //}
            //LightsForLoop();
            //FullSeq.Append(PlaceLightsInRooms());
            //On complete code is inside spawn enemy
            //FullSeq.Append(SpawnEnemy());
            FullSeq.onKill = () =>
            {
                print("spawn enemy seq complete");
                player.SequenceDone = true;
                if (Gmanager.instance != null)
                    Gmanager.instance.ReturnToPlayer();
            };

            yield return null;
        }

        Sequence  OuterWallsFallInTween()
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < TweenObjects.Count; i++)
            {
                Vector2 originalpos = TweenObjects[i].position;
                TweenObjects[i].position += new Vector3(0, 15);
                walltween = seq.Insert(i * .5f - i * .08f, TweenObjects[i].DOMove(originalpos, 2).SetEase(Ease.OutBounce));
                if (i == TweenObjects.Count - 1)
                {
                    seq.AppendCallback(() => { Debug.Log("First callback!"); });
                }
                
                //seq.Append(TweenObjects[i].transform.DOMove(originalpos, 2).SetEase(Ease.OutBounce));
            }
            return seq;
        }

        

        //generates tiles for room and rotates them
        Sequence RotateFloorTiles()
        {

            Sequence TileRotSeq = DOTween.Sequence();
            Transform[,] TileHolder = GenerateWholeRoomFloor();
            int TileGridsizeX = TileHolder.GetLength(0);
            int TileGridSizeY = TileHolder.GetLength(1);
            int xpos;
            for (int x = 0; x < TileGridsizeX; x++)
            {
                for (int y = 0; y <= x ; y++)
                {
                    if(y > TileGridSizeY -1)
                    { break; }
                    //Tween thisTween = TileHolder[x, y].DOPunchRotation(new Vector3(360, 0, 0), 3.5f - (x * .3f), 3, 1).OnStart(Setactive(TileHolder[x, y].gameObject));
                    Tween thisTween = TileHolder[x, y].DOLocalRotate(new Vector3(810, 0), 4.5f - (x * .4f), RotateMode.WorldAxisAdd).SetEase(roomdata.tileEase).OnStart(Setactive(TileHolder[x, y].gameObject));
                    TileRotSeq.Insert((x * .3f) + .3f, thisTween);
                    xpos = x;
                    if(y == x)
                    {
                        do
                        {
                            x--;
                            if (x < 0) break;
                            Tween ThisTween = TileHolder[x, y].DOLocalRotate(new Vector3(810, 0), Mathf.Clamp( 4.5f - (xpos * .4f),.5f, 2 ), RotateMode.WorldAxisAdd).SetEase(roomdata.tileEase) .OnStart(Setactive(TileHolder[x, y].gameObject));
                            TileRotSeq.Insert((xpos * .3f) + .3f,
                        ThisTween);
                        }
                        while (x != 0);
                    }
                    x = xpos;
                }
            }
            return TileRotSeq;
            //for (int i = 0; i < TileHolder.transform.childCount; i++)
            //{
            //    TileRotSeq.Insert(i * .05f,
            //    TileHolder.transform.GetChild(i).DOPunchRotation(new Vector3(90, 0), 2, 3, 1));
            //}
        }

        Sequence RotateRooms()
        {
            Sequence RotSeq = DOTween.Sequence();
            for (int i = 0; i < roomgameobjects.Count; i++)
            {
                Vector2 originalrot = roomgameobjects[i].transform.rotation.eulerAngles;
                roomgameobjects[i].transform.eulerAngles += new Vector3(0,0, -90);
                RotSeq.Insert((i *1) + 2.5f, roomgameobjects[i].transform.DORotate(originalrot, 1.5f).SetEase(roomdata.RotEase));
                GameObject HorizontalWall = roomgameobjects[i].transform.GetChild(1).gameObject;
                Vector2 HoriOriginalRot = HorizontalWall.transform.rotation.eulerAngles;
                HorizontalWall.transform.eulerAngles += new Vector3(0, 0, -90);
                RotSeq.Insert((i * 1) + 2.5f , HorizontalWall.transform.DORotate(HoriOriginalRot, 2).SetEase(roomdata.RotEaseHorizontal));

                //if (i == roomgameobjects.Count - 1)
                //{
                //    RotSeq.AppendCallback(() => { Debug.Log("rooms callback!"); });
                //}
                //seq.Append(TweenObjects[i].transform.DOMove(originalpos, 2).SetEase(Ease.OutBounce));
            }
            RotSeq.onComplete = () => { 
                nodegrid.ResetGridImmediate();
            } ;
            return RotSeq;
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
                    if (room.firstwall == 0)
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
            //Debug.Log("returning grid point " + x + " and " + y +" position"  );
            return grid[x, y];


        }

        public void moveToNextRoom(Transform nextRoom)
        {
            gameObject.SetActive(false);
            nextRoom.parent.gameObject.SetActive(true);
            Gmanager.instance.StartRoomEffect(nextRoom.parent.gameObject);
        }


        //gets RoomGen's outer walls to tween
        void WallsToSeq()
        {
            TweenObjects.Add(transform.GetChild(5));
            TweenObjects.Add(transform.GetChild(6));
            TweenObjects.Add(transform.GetChild(7));
            TweenObjects.Add(transform.GetChild(0));
            TweenObjects.Add(transform.GetChild(3));
            TweenObjects.Add(transform.GetChild(4));
            TweenObjects.Add(transform.GetChild(1));
            TweenObjects.Add(transform.GetChild(2));
        }


       /// <summary>
       /// use character u,d,r,l to open respective openingsin rooms
       /// </summary>
       /// <param name="Dir"></param>
        public void OpenDoor(char Dir)
        {
            switch (Dir)
            {
                case 'u':
                    GameObject TopDoor = transform.GetChild(10).gameObject;
                    TopDoor.GetComponent<SpriteRenderer>().enabled = false;
                    TopDoor.GetComponent<BoxCollider2D>().isTrigger = true;
                    TopDoor.GetComponent<SpriteMask>().enabled = true;
                    //masktop.enabled = true;
                    break;
                case 'd':
                    GameObject BottomDoor = transform.GetChild(11).gameObject;
                    BottomDoor.GetComponent<SpriteRenderer>().enabled = false;
                    BottomDoor.GetComponent<BoxCollider2D>().isTrigger = true;
                    BottomDoor.GetComponent<SpriteMask>().enabled = true;
                    //maskbottom.enabled = true;
                    break;
                case 'l':
                    GameObject LeftDoor = transform.GetChild(8).gameObject;
                    LeftDoor.GetComponent<SpriteRenderer>().enabled = false;
                    LeftDoor.GetComponent<BoxCollider2D>().isTrigger = true;
                    LeftDoor.GetComponent<SpriteMask>().enabled = true;
                    //maskleft.enabled = true ;
                    break;
                case 'r':
                    GameObject RightDoor = transform.GetChild(9).gameObject;
                    RightDoor.GetComponent<SpriteRenderer>().enabled = false;
                    RightDoor.GetComponent<BoxCollider2D>().isTrigger = true;
                    RightDoor.GetComponent<SpriteMask>().enabled = true;
                    //maskright.enabled = true;
                    break;
            }
        }



        Transform[,] GenerateWholeRoomFloor()
        {
            GameObject TileHolder = new GameObject("FloorTileHolder");
            TileHolder.transform.SetParent(gameObject.transform);
            Transform[,] Tiles = new Transform[TileGrid.GetLength(0), TileGrid.GetLength(1)];
            for (int x = 0; x < TileGrid.GetLength(0); x++)
            {
                for (int y = 0; y < TileGrid.GetLength(1); y++)
                {
                    GameObject Thistile = Instantiate(roomdata.FloorTile, TileGrid[x, y], Quaternion.Euler(new Vector3(90,0)));
                    Thistile.transform.SetParent(TileHolder.transform);
                    Thistile.SetActive(false);
                    Tiles[x, y] = Thistile.transform;
                }
            }
            TileHolder.transform.position += new Vector3(0, 0.7f);
            return Tiles;
        }

        Sequence PlaceLightsInRooms()
        {
            Sequence seq = DOTween.Sequence();
            int i = 1;
            foreach(Room room in rooms)
            {
                //add an if condition when room is too big and needs more than 1 light
                
                //Vector3 RoomStart = new Vector2(room.startwall.x, room.startwall.y);
                //Vector3 RoomEnd = new Vector2(room.lastwall.x, room.lastwall.y);
                Vector3 midpoint = (room.startwall + room.lastwall) * .5f;
                Light2D light = Instantiate(InnerRoomLight, midpoint, Quaternion.identity, gameObject.transform).GetComponent<Light2D>();
                seq.Insert(i, DOTween.To(() => light.intensity, x => light.intensity = x, 8, .6f));
                light.color = Random.ColorHSV(saturationMin: .3f, saturationMax: .8f, hueMax: 1, hueMin: 0, alphaMin: 100, alphaMax: 100, valueMin: .3f, valueMax: .6f);
                //light.pointLightOuterRadius = Mathf.Max(room.)
                i++;
            }
            return seq;
        }

        //places inner room carpet prefab inside
        Sequence[] PlaceFloorInRoom()
        {
            Sequence[] Seqs = new Sequence[rooms.Count];
            int index = 0;
            foreach(Room _room in rooms)
            {
                Sequence seq = DOTween.Sequence();
                //print("Start wall is " + _room.startwall);
                //print("endpoint is " + _room.lastwall);
                Vector3 RoomStart = new Vector2(_room.startwall.x , _room.startwall.y);
                Vector3 RoomEnd = new Vector2(_room.lastwall.x, _room.lastwall.y );
                Vector3 midpoint = (RoomStart + RoomEnd) * .5f;
                float StartEndDistX = Mathf.Abs( RoomEnd.x - RoomStart.x );
                float StartEndDistY = Mathf.Abs( RoomEnd.y  - RoomStart.y);
                //print("midpoint is " + midpoint);
                GameObject carpet =  Instantiate(roomdata.Carpet, position: midpoint, Quaternion.identity);
                SpriteRenderer CarpetSpriteRender = carpet.GetComponent<SpriteRenderer>();
                //CarpetSpriteRender.size = new Vector2(StartEndDistX,StartEndDistY );
                Vector2 myVector = Vector2.zero;
                //DOTween.To(() => myVector, x => myVector = x, new Vector2(StartEndDistX, StartEndDistY), 1);
                CarpetSpriteRender.size = myVector;

                   seq.Insert(0,( DOTween.To(
                     () => CarpetSpriteRender.size, // This is the getter method
                        x => CarpetSpriteRender.size = x, // This is the setter method
                    new Vector2(StartEndDistX, StartEndDistY /4),
                    1
                        ).SetEase(Ease.OutBack)));

                seq.Insert(1,(DOTween.To(
                     () => CarpetSpriteRender.size,
                        x => CarpetSpriteRender.size = x,
                    new Vector2(StartEndDistX, StartEndDistY),
                    1
                        ).SetEase(Ease.InOutBack)));
                Seqs[index] = seq;
                index++;
            }
            return Seqs;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridworldsize.x, gridworldsize.y, 1));
            if (grid != null && drawgrid)
                foreach (Room.Roomtile n in grid)
                {

                    Gizmos.color = Color.black;

                    if (n.insideRoom)
                        Gizmos.color = Color.blue;

                    if (!n.empty)
                        Gizmos.color = Color.white;
                    if (n.onEntrance)
                        Gizmos.color = Color.red;
                    Gizmos.DrawCube(n.point, Vector3.one * (nodediameter - .1f));
                }

            Gizmos.color = Color.blue;
            //if (startwall != null && endwall != null)
            //Gizmos.DrawLine(startwall, endwall);
            if (rooms != null)
                foreach (Room room in rooms)
                {
                    if (room.startwall != null && room.corner != null)
                    {
                        Gizmos.DrawLine(new Vector3(room.startwall.x, room.startwall.y), room.corner );
                        Gizmos.DrawLine(room.corner, new Vector3(room.lastwall.x , room.lastwall.y ));
                    }

                }



            foreach (Vector2 point in RightEntrancepoints)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position + (Vector3)point, .3f);
            }
            foreach (Vector2 point in LeftEntrancepoints)
            {
                Gizmos.DrawWireSphere(transform.position + (Vector3)point, .3f);
            }
            foreach (Vector2 point in TopEntrancepoints)
            {
                Gizmos.DrawWireSphere(transform.position + (Vector3)point, .3f);
            }
            foreach (Vector2 point in BottomEntrancepoints)
            {
                Gizmos.DrawWireSphere(transform.position + (Vector3)point, .3f);
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                print("player touching entrance");
                //right
                if (collision.IsTouching(transform.GetChild(9).GetComponent<Collider2D>()))
                {
                    print("Player touching right entrance");
                    moveToNextRoom(WestNeighborPoint);

                    OnDoor(WestNeighborPoint.position, Vector2.right);
                }
                //left
                if (collision.IsTouching(transform.GetChild(8).GetComponent<Collider2D>()))
                {
                    print("Player touching left entrance");
                    OnDoor(EastNeighborPoint.position, Vector2.left);
                    moveToNextRoom(EastNeighborPoint);
                }
                //top
                if (collision.IsTouching(transform.GetChild(10).GetComponent<Collider2D>()))
                {
                    print("Player touching top entrance");
                    OnDoor(SouthNeighborPoint.position, Vector2.up);
                    moveToNextRoom(SouthNeighborPoint);
                }
                //bottom
                if (collision.IsTouching(transform.GetChild(11).GetComponent<Collider2D>()))
                {
                    print("Player touching bottom entrance");
                    if (NorthNeighborPoint == null) return;
                    OnDoor(NorthNeighborPoint.position, Vector2.down);
                    moveToNextRoom(NorthNeighborPoint);
                }
            }
        }
    }
    
}
