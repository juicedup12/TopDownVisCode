using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


namespace topdown
{
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
        public GameObject TileHolder;
        public static event DoorAction OnDoor;
        Tween walltween;
        public bool SpawnOnKey = false;
        public LayerMask wallmask;
        Transform SptriteMskTransofrm;
        bool CompletedTransition = false;
        Transform[,] Tiles;
        [SerializeField]
        TransitionManager.Floortransition floor;

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
        GameObject[] enemyPrefabs;
        public player player;
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
            TileHolder = GameObject.Find("TileParent");
            enemyPrefabs = roomdata.enemies;
            nodegrid = GetComponentInChildren<CopyNodeGrid>();
            creategrid();
            createTilegridPositions();
            EntrancePointsToGridCoordinates();

            CameraManager.instance.TransferCam(gameObject);
            //put walls in a sequence
            WallsToSeq();

            MakeRandomRooms();
            ChangeGridEmpty();
            ChangeGridInRoom();
            //StartCoroutine( StartTweening());
            StartCoroutine(BeginTween());

            //SpawnItem();


            //makewall();
            //SpawnEnemy();

            

            //PlaceFloorInRoom();

            //LightsForLoop();
            //OuterWallsFallInTween();
            //RotateRooms();
            //RotateFloorTiles();
        }

        private IEnumerator BeginTween()
        {
            
            IRoomTransitioner[] transitions = GetComponents<IRoomTransitioner>();
            foreach(IRoomTransitioner t in transitions)
            {
                t.DoRoomTransition();
            }
            yield return new WaitForSeconds(3);
            CameraManager.instance.TransferCamToPlayer();
            player.SequenceDone = true;
            //EnterRoomTxt.enabled = true;
            UITextNotificationController.instance.UpdateText("press " + UITextNotificationController.instance.currentAcceptButton + " to enter");
            print("current room is " + gameObject.name);
        }

        private void OnEnable()
        {
            if(CompletedTransition)
            {
                print("Tween already complete Player is in control ");
                player.SequenceDone = true;
                Gmanager.instance.ReturnToPlayer();
            }
        }

        //creates room tiles used in room creation for placing objects and walls
        //room tiles hold data like vector3s and data about occupied spaces and special points
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


        //creates a vector3 grid for tile sprites to populate
        //maybe make an interface that uses a method like this and each class that inherits uses it's own way of implementing effects
        void createTilegridPositions()
        {
            float GridSizeScaler = 1f;
            float TileRadius =  TileMultiplier;
            float TileDiameter = TileRadius * 2 ;
            int TileGridSizeX = Mathf.RoundToInt(gridworldsize.x * GridSizeScaler / TileDiameter );
            int TileGridSizeY = Mathf.RoundToInt(gridworldsize.y * GridSizeScaler / TileDiameter );
            TileGrid = new Vector3[TileGridSizeX, TileGridSizeY];
            Vector3 worldbottomleft = transform.position - Vector3.right * (gridworldsize.x * GridSizeScaler) / 2 - Vector3.up * (gridworldsize.y * GridSizeScaler) / 2;
            //print("world bottom left is " + worldbottomleft);
            //print("first world point is  " +  (worldbottomleft + Vector3.right * (0 * TileDiameter + TileRadius)
                        //+ Vector3.up * (0 * TileDiameter + TileRadius)));

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
                    Room.Roomtile tile = NodeFromWorldPoint(BottomEntrancepoints[i]);
                    GridEntrancePoints.Add(tile);

                    // Debug.Log("adding Bottom entrance points to grid" + BottomEntrancepoints[i]);
                    GridEntrancePoints.Add(NodeFromWorldPoint(BottomEntrancepoints[i]));
                }
        }


        void AddLightsToHallway()
        {

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
            for (int x = 0; x < gridsizeX; x += 5)
            {
                for (int y = 0; y < gridsizeY; y+= 5)
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
                        seq.Insert((x * .15f) + y * .3f,(DOTween.To(() => light.intensity, l => light.intensity = l, (float).6, .8f).OnStart ( () => light.gameObject.SetActive(true)).SetEase(lightease))); 
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
        
        //move to a seperate class
        public Sequence SpawnEnemy()
        {
            if (enemyPrefabs.Length < 1) return null;
            int enemycount = 0;

            Sequence SpawnSeq = DOTween.Sequence();

            //Debug.Log("this room is number " + LevelCreator.GetRoomInOrder(transform) + " in the list", gameObject);
            int ListPos = ProcedualLevelCreator.GetRoomInOrder(transform);
            float IndexFraction = (float)ListPos / (float)8;
            //Debug.Log("index fraction is " + IndexFraction + " in list pos " + ListPos);
            float MaxEnemySpawn =  (Random.Range(10, 20)  * IndexFraction) + 4;
            //Debug.Log("Max enemy spawn is " + MaxEnemySpawn, gameObject);
            int EnemySpawnAmount =  Random.Range(2,(int) MaxEnemySpawn);
            //Debug.Log(ListPos + " Room spawning " + EnemySpawnAmount);
            if(EnemySpawnAmount >0)
            do
            {
                
                Room.Roomtile TileToSpawn = GetRandomRoomTile();
                if (TileToSpawn.empty == true)
                {
                        Debug.Log("Enemy tiles spawn point" + TileToSpawn.point + " is " + TileToSpawn.empty);
                    int enemynum = Random.Range(0, enemyPrefabs.Length);
                    Enemy enemy = Instantiate(enemyPrefabs[enemynum], TileToSpawn.point, Quaternion.identity).GetComponent<Enemy>();
                        enemy.player = player;
                    enemy.transform.parent = gameObject.transform;
                    SpawnSeq.Insert(enemycount, enemy.transform.DOPunchScale(enemy.transform.localScale + new Vector3(5,-.6f,0), .8f, 1, 1f).SetEase(roomdata.enemyScaleEase).OnStart(() => enemy.gameObject.SetActive(true)));
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


        //this logic should be handled by enemy scripts
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
                    //print("breaking patrol assign while loop");
                    break;
                }
            } while (patrolPointDist < 1);

            enemy.Patrolpoints = patrolpoints;
            //Debug.Log("Patrol points are " + patrolpoints[0] + " " + patrolpoints[1]);
            yield return new WaitForSeconds(2);
            enemy.CantMove = false;
            yield return null;
        }

        //move to a procedual subtype
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

        //redundant, make a new method in enemy class that handles patrol logic better
        Vector2[] setVerticalPatrolPoints(Enemy enemy, Vector2 TilePoint)
        {
            Vector2[] patrolpoints = new Vector2[2];
            RaycastHit2D TopHit;
            RaycastHit2D BottomHit;

            TopHit = Physics2D.Raycast(enemy.transform.position, Vector2.up, 20, wallmask);
            //Debug.Log("getting raycast from " + TilePoint + " and " + (TilePoint + Vector2.up * 20), enemy);
            //if (TopHit.collider != null)
            //    //Debug.Log("tophit hit " + TopHit.collider.name + " patrol point 0 is " + TopHit.point, TopHit.collider.gameObject);
            //else
            //    //Debug.Log("didn't hit anything, patrol point is " + new Vector2(TilePoint.x, transform.position.y + gridworldsize.y / 2), enemy);
            ////might have to use transform.position.y + Vector3.up * gridworldsize.y / 2
            BottomHit = Physics2D.Raycast(TilePoint, Vector2.down, 20, wallmask);
            //Debug.Log("getting bottom raycass from " + TilePoint + " and " + (TilePoint + Vector2.down * 20) + " bitmask is " + LayerMask.NameToLayer("wall"), enemy);
            //Debug.Log(BottomHit.collider == null ? "Bottom raycast hit nothing, patrol point is " + new Vector2(TilePoint.x, transform.position.y - (gridworldsize.y + .3f) / 2) : "bottom raycast hit " + BottomHit.collider.name + " patrol point 1 is " + BottomHit.point);

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

        public void MakeRandomRooms()
        {
            int rooms = Random.Range(0, 4);
            if (rooms != 0)
                for (int i = 0; i < rooms; i++)
                {
                    makewall();
                }
        }


        //move to a subtype class
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

        //public void ActivateSpears(GameObject obj)
        //{
        //    StartCoroutine(Spears(obj));
        //}

        //public void ActivateCoins(GameObject obj)
        //{
        //    StartCoroutine(Coins(obj));
        //}


        

            //places obj on a random tile that is empty
        public IEnumerator CreateOnEmptyTilesRoutine(GameObject obj)
        {
            int xpos;
            for (int x = 0; x < grid.GetLength(0); x+= 3)
            {
                for (int y = 0; y <= x; y += 3)
                {
                    if (y > grid.GetLength(1) - 1)
                    { break; }
                    //Tween thisTween = TileHolder[x, y].DOPunchRotation(new Vector3(360, 0, 0), 3.5f - (x * .3f), 3, 1).OnStart(Setactive(TileHolder[x, y].gameObject));
                    if (grid[x, y].empty)
                    {
                        Instantiate(obj, position: grid[x, y].point, Quaternion.identity);
                        yield return new WaitForSeconds(.1f);
                        xpos = x;
                        if (y == x)
                        {
                            do
                            {
                                x -= 3;
                                if (x < 0) break;
                                Instantiate(obj, position: grid[x, y].point, Quaternion.identity);
                                yield return new WaitForSeconds(.1f);

                            }
                            while (x > 0);
                        }
                        x = xpos;
                    }
                }
            }

            yield return null;
        }


        //creates objects and rotates them depending on the axis of the grid position
        public IEnumerator CreateOnEmptyWallsRoutine(GameObject obj )
        {

            for (int x = 0; x < gridsizeX; x++)
            {

                if(grid[x, 1].empty)
                {
                    Vector2 posToInstantiate = grid[x, 0].point;
                    Instantiate(obj, position: posToInstantiate, Quaternion.Euler(new Vector3(0,0, 90)));
                    yield return new WaitForSeconds(.5f);
                }
            }

            for (int y = 0; y < gridsizeY; y++)
            {
                if(grid[gridsizeX -1, y].empty)
                {
                    Vector2 posToInstantiate = grid[gridsizeX -1, y].point;
                    Instantiate(obj, position: posToInstantiate, Quaternion.Euler(new Vector3(0, 0, 180)));
                    yield return new WaitForSeconds(.5f);
                }
            }
            

        }

        private IEnumerator StartTweening()
        {
            //yield return new WaitForSeconds(.5f);


            if (!CompletedTransition)
            {
                Sequence FullSeq = DOTween.Sequence();
                FullSeq.Join(OuterWallsFallInTween());
                FullSeq.Join(RotateRooms());
                //FullSeq.Join(RotateFloorTiles());
                float i = 3.5f;
                foreach (Sequence seq in PlaceFloorInRoom())
                {

                    FullSeq.Insert(i, seq);
                    i += i * .3f;
                }
                LightsForLoop();
                FullSeq.Insert(4, PlaceLightsInRooms());
                //On complete code is inside spawn enemy
                FullSeq.Append(SpawnEnemy());
                FullSeq.onKill = () =>
                {
                //print("spawn enemy seq complete");
                player.SequenceDone = true;
                    if (Gmanager.instance != null)
                        Gmanager.instance.ReturnToPlayer();
                };
                CompletedTransition = true;
            }
            else
            {
                player.SequenceDone = true;
                //Gmanager.instance.ReturnToPlayer();
            }
            yield return null;
        }


        //change code as stated below 
        Sequence  OuterWallsFallInTween()
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < TweenObjects.Count; i++)
            {
                Vector2 originalpos = TweenObjects[i].position;
                //moves objects out of view
                TweenObjects[i].position += new Vector3(0, 15);
                //place tween further in the timeline
                walltween = seq.Insert(i * .5f - i * .08f, TweenObjects[i].DOMove(originalpos, 2).SetEase(Ease.OutBounce));
                if (i == TweenObjects.Count - 1)
                {
                    seq.AppendCallback(() => { Debug.Log("First callback!"); });
                }
                
                //seq.Append(TweenObjects[i].transform.DOMove(originalpos, 2).SetEase(Ease.OutBounce));
            }
            return seq;
        }

        
        //this method should be an interface method
        //where all effects are handled 
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
                    Tween thisTween = TileHolder[x, y].DOLocalRotate(new Vector3(450, 0), Mathf.Clamp(2.5f + (x * .4f), .5f, 2), RotateMode.WorldAxisAdd).SetEase(roomdata.tileEase).OnStart(Setactive(TileHolder[x, y].gameObject));
                    TileRotSeq.Insert((x * .3f) + 2f, thisTween);
                    xpos = x;
                    if(y == x)
                    {
                        do
                        {
                            x--;
                            if (x < 0) break;
                            Tween ThisTween = TileHolder[x, y].DOLocalRotate(new Vector3(450, 0), Mathf.Clamp( 2.5f + (xpos * .4f),.5f, 2 ), RotateMode.WorldAxisAdd).SetEase(roomdata.tileEase) 
                            .OnStart(() => TileHolder[x, y].gameObject.SetActive(true));
                            TileRotSeq.Insert((xpos * .3f) + 2f,
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
        
        //should be part of an interface method as stated above
        Sequence RotateRooms()
        {
            Sequence RotSeq = DOTween.Sequence();
            for (int i = 0; i < roomgameobjects.Count; i++)
            {
                Vector2 originalrot = roomgameobjects[i].transform.rotation.eulerAngles;
                Room room = rooms[i];
                bool Bottom = room.firstwall == 0;
                if (Bottom)
                {
                    roomgameobjects[i].transform.eulerAngles += new Vector3(0, 0, -90);
                    //Debug.Log("room game object is on botton rotating -90", roomgameobjects[i]);
                }
                else
                {
                    roomgameobjects[i].transform.eulerAngles += new Vector3(0, 0, 180);
                    //Debug.Log("room game object is on top rotating 90", roomgameobjects[i]);
                }
                RotSeq.Insert((i *1) + 2.5f, roomgameobjects[i].transform.DORotate(originalrot, 1.5f).SetEase(roomdata.RotEase));
                GameObject HorizontalWall = roomgameobjects[i].transform.GetChild(1).gameObject;
                Vector2 HoriOriginalRot = HorizontalWall.transform.rotation.eulerAngles;
                if(Bottom)
                HorizontalWall.transform.eulerAngles += new Vector3(0, 0, -90);
                else
                    HorizontalWall.transform.eulerAngles += new Vector3(0, 0, 90);
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


        //should change method name to "ApplyTileValues" and edit to apply other values
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

        //should change method name to "ApplyTileValues" and edit to apply other values
        //changes every tile that's within a small room to inside of room
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


        //returns a roomtile from a world pos
        //useful for placing objects and checking where that tile is
        //maybe a different kind method of checking tiles could be used insted of using worldspace
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


        //this method will be moved to a door class
        public IEnumerator moveToNextRoom(Transform nextRoom)
        {
            yield return new WaitForEndOfFrame();
            print("disabling room " + gameObject);
            gameObject.SetActive(false);

            print("activating next room " + nextRoom.parent);
            nextRoom.parent.gameObject.SetActive(true);
            yield return null;

        }



        //move to a room effect class
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

        //rework to only access variables once
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

        Transform GetRandomFloorTile()
        {
            int randomTileX;
            int randomTileY;
            Room.Roomtile tile;
            do
            {
                randomTileX = Random.Range(0, Tiles.GetLength(0));
                randomTileY = Random.Range(0, Tiles.GetLength(1));
                tile = NodeFromWorldPoint(Tiles[randomTileX, randomTileY].position);
            } while (!tile.empty);

            return Tiles[randomTileX, randomTileY];
        }

        void SpawnItem()
        {
            int RanditemIndex = Random.Range(0, roomdata.ItemsToSpawn.Length);
            Item item = Instantiate( roomdata.ItemsToSpawn[RanditemIndex]).GetComponent<Item>();
            Debug.Log("spawned item", item);
            item.transform.SetParent(GetRandomFloorTile(), false);
        }


        //need to populate the gameobject with tile objects
        //new rooms need to grab from parent game object
        Transform[,] GenerateWholeRoomFloor()
        {
            
            //if tile holder hasn't already been populated
            if (TileHolderData.tilepool == null)
            {
                TileHolderData.tilepool = new Transform[TileGrid.GetLength(0), TileGrid.GetLength(1)];
                Tiles = new Transform[TileGrid.GetLength(0), TileGrid.GetLength(1)];
                for (int x = 0; x < TileGrid.GetLength(0); x++)
                {
                    for (int y = 0; y < TileGrid.GetLength(1); y++)
                    {
                        GameObject Thistile = Instantiate(roomdata.FloorTile, TileGrid[x, y], Quaternion.Euler(new Vector3(90, 0)));
                        TileHolderData.tilepool[x, y] = Thistile.transform;
                        Thistile.transform.SetParent(TileHolder.transform);
                        Thistile.SetActive(false);
                        Tiles[x, y] = Thistile.transform;
                    }
                }
            }
            else
            {
                Tiles = new Transform[TileGrid.GetLength(0), TileGrid.GetLength(1)];
                for (int x = 0; x < TileGrid.GetLength(0); x++)
                {
                    for (int y = 0; y < TileGrid.GetLength(1); y++)
                    {
                        GameObject Thistile = TileHolderData.tilepool[x, y].gameObject;
                        TileHolderData.tilepool[x, y].position = TileGrid[x, y];
                        TileHolderData.tilepool[x, y].rotation = Quaternion.Euler(new Vector3(90, 0));
                        
                        Thistile.SetActive(false);
                        Tiles[x, y] = Thistile.transform;
                    }
                }
            }
            //TileHolder.transform.position += new Vector3(0, 0.7f);
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

                float RoomWidth =  room.lastwall.x - room.startwall.x ;
                float roomheight = room.lastwall.y - room.startwall.y;
                Debug.Log("room width is " + Mathf.Abs(RoomWidth), room.roombase);
                List<Light2D> lights = new List<Light2D>();
                if (Mathf.Abs(RoomWidth) > 5 || Mathf.Abs(roomheight) > 3)
                {
                    if (Mathf.Abs(RoomWidth) > 5)
                    {
                        float QuartWidth = RoomWidth * .35f;
                        Vector3 LightPos = room.startwall + new Vector2(QuartWidth, 0);
                        Debug.Log("quart width is " + QuartWidth + "light pos is " + LightPos, room.roombase);
                        Vector3 midpoint = (room.startwall + room.lastwall) * .5f;
                        Color lightcolor = Random.ColorHSV(saturationMin: .3f, saturationMax: .8f, hueMax: 1, hueMin: 0, alphaMin: 100, alphaMax: 100, valueMin: .3f, valueMax: .6f);
                        Light2D light1 = Instantiate(InnerRoomLight, midpoint + new Vector3(QuartWidth, 0), Quaternion.identity, gameObject.transform).GetComponent<Light2D>();
                        Light2D light2 = Instantiate(InnerRoomLight, midpoint - new Vector3(QuartWidth, 0), Quaternion.identity, gameObject.transform).GetComponent<Light2D>();
                        light1.color = lightcolor;
                        light2.color = lightcolor;
                        lights.Add(light1);
                        lights.Add(light2);


                    }
                    if (Mathf.Abs(roomheight) > 3)
                    {
                        float QuartHeight = roomheight * .25f;
                        Vector3 LightPos = room.startwall + new Vector2(QuartHeight, 0);
                        Debug.Log("quart height is " + QuartHeight + "light pos is " + LightPos, room.roombase);
                        Vector3 midpoint = (room.startwall + room.lastwall) * .5f;
                        Color lightcolor = Random.ColorHSV(saturationMin: .3f, saturationMax: .8f, hueMax: 1, hueMin: 0, alphaMin: 100, alphaMax: 100, valueMin: .3f, valueMax: .6f);
                        Light2D light1 = Instantiate(InnerRoomLight, midpoint + new Vector3(0, QuartHeight), Quaternion.identity, gameObject.transform).GetComponent<Light2D>();
                        Light2D light2 = Instantiate(InnerRoomLight, midpoint - new Vector3(0, QuartHeight), Quaternion.identity, gameObject.transform).GetComponent<Light2D>();
                        light1.color = lightcolor;
                        light2.color = lightcolor;
                        lights.Add(light1);
                        lights.Add(light2);
                    }
                }
                else
                {
                    Vector3 midpoint = (room.startwall + room.lastwall) * .5f;
                    lights.Add(Instantiate(InnerRoomLight, midpoint, Quaternion.identity, gameObject.transform).GetComponent<Light2D>());
                }
                foreach (Light2D light in lights)
                {
                    seq.Insert(i, DOTween.To(() => light.intensity, x => light.intensity = x, 8, .6f));
                    light.color = Random.ColorHSV(saturationMin: .3f, saturationMax: .8f, hueMax: 1, hueMin: 0, alphaMin: 100, alphaMax: 100, valueMin: .3f, valueMax: .6f);
                    //light.pointLightOuterRadius = Mathf.Max(room.)
                    i++;
                }
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
                print("Start wall is " + _room.startwall);
                print("endpoint is " + _room.lastwall);
                Vector3 RoomStart = new Vector2(_room.startwall.x , _room.startwall.y);
                Vector3 RoomEnd = new Vector2(_room.lastwall.x, _room.lastwall.y );
                Vector3 midpoint = (RoomStart + RoomEnd) * .5f;
                float StartEndDistX = Mathf.Abs( RoomEnd.x - RoomStart.x );
                float StartEndDistY = Mathf.Abs( RoomEnd.y  - RoomStart.y);
                print("midpoint is " + midpoint);

                GameObject carpet =  Instantiate<GameObject>(roomdata.Carpet, position: midpoint, Quaternion.identity);
                SpriteRenderer CarpetSpriteRender = carpet.GetComponent<SpriteRenderer>();
                //CarpetSpriteRender.size = new Vector2(StartEndDistX,StartEndDistY );
                Vector2 myVector = Vector2.zero;
                //carpet.transform.position = midpoint;
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
                seq.onComplete = () => { carpet.transform.SetParent(_room.InstancedVertWall.transform);
                    carpet.GetComponent<BoxCollider2D>().size = CarpetSpriteRender.size * .9f;
                };
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

        //doors should get their own monobehavior classes and handle transfer logic there
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //print(gameObject.name + "  colided with " + collision.gameObject);
            if(collision.IsTouchingLayers(1 << 17))
            if (collision.CompareTag("Player"))
            {
                System.Action roomTransition;

                
                //level transition is set to false in player.cs 
                //when player moves to next room
                print("player touching entrance");
                //right
                if (collision.IsTouching(transform.GetChild(9).GetComponent<Collider2D>()) && Gmanager.instance.CanPlayerTransition())
                {
                    if (WestNeighborPoint == null) return;
                    roomTransition = () =>
                    {
                        //if the transform tag is a shop then don't move player to room

                        Gmanager.instance.MovePlayerToRoom(WestNeighborPoint.transform, Vector2.right);
                        print("Player touching right entrance");
                        StartCoroutine( moveToNextRoom(WestNeighborPoint));

                    };
                    InteractUIBehavior.instance.gameObject.SetActive(true);
                    InteractUIBehavior.instance.DisplayUI(roomTransition, transform.GetChild(9).position, "To Enter");

                    //collision.GetComponent<player>().SetWait();

                    //OnDoor(WestNeighborPoint.position, Vector2.right);
                }
                //left
                if (collision.IsTouching(transform.GetChild(8).GetComponent<Collider2D>()) && Gmanager.instance.CanPlayerTransition())
                {
                    if (EastNeighborPoint == null) return;
                    roomTransition = () =>
                    {
                        Gmanager.instance.MovePlayerToRoom(EastNeighborPoint, Vector2.left);

                        print("Player touching left entrance");


                        //disable current room enable next room
                        StartCoroutine(moveToNextRoom(EastNeighborPoint));
                    };
                    InteractUIBehavior.instance.gameObject.SetActive(true);
                    InteractUIBehavior.instance.DisplayUI(roomTransition, transform.GetChild(8).position, "To Enter");

                }
                //top
                if (collision.IsTouching(transform.GetChild(10).GetComponent<Collider2D>()) && Gmanager.instance.CanPlayerTransition())
                {
                    if (SouthNeighborPoint == null) return;
                    roomTransition = () =>
                    {
                        print("Player touching top entrance");

                        Gmanager.instance.MovePlayerToRoom(SouthNeighborPoint, Vector2.up);

                        //OnDoor(SouthNeighborPoint.position, Vector2.up);
                        //collision.GetComponent<player>().SetWait();

                        StartCoroutine(moveToNextRoom(SouthNeighborPoint));
                    };
                    InteractUIBehavior.instance.gameObject.SetActive(true);
                    InteractUIBehavior.instance.DisplayUI(roomTransition, transform.GetChild(10).position, "To Enter");

                }
                //bottom
                if (collision.IsTouching(transform.GetChild(11).GetComponent<Collider2D>()) && Gmanager.instance.CanPlayerTransition())
                {
                    if (NorthNeighborPoint == null) return;
                    roomTransition = () =>
                    {
                        if (NorthNeighborPoint.CompareTag("shop"))
                        {
                            NorthNeighborPoint.gameObject.SetActive(true);
                            Time.timeScale = 0;
                            return;
                        }

                        print("Player touching bottom entrance");
                        Gmanager.instance.MovePlayerToRoom(NorthNeighborPoint, Vector2.down);

                        //OnDoor(NorthNeighborPoint.position, Vector2.down);
                        //collision.GetComponent<player>().SetWait();

                        StartCoroutine(moveToNextRoom(NorthNeighborPoint));
                    };
                    InteractUIBehavior.instance.gameObject.SetActive(true);
                    InteractUIBehavior.instance.DisplayUI(roomTransition, transform.GetChild(11).position, "To Enter");

                }
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //if the player exited AND the level transition is true
                if (Gmanager.instance.LevelTransition && Gmanager.instance.LevelStarted)
                {
                    //did the player walk out of the warp zone
                    Gmanager.instance.LevelTransition = false;
                    Debug.Log("Player exited door warp. Level transition ended", gameObject);
                }

                InteractUIBehavior.instance.HideUI();
            }

        }
    }


    public enum SpawnEvents
    {
        AllEmptyTiles, TilesOnWall,  
    }
}
