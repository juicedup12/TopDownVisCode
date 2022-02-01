using System.Collections;
using UnityEngine;
using TMPro;
using Cinemachine;

namespace topdown
{
    //disables and enables rooms as the player moves from them
    public class Gmanager : MonoBehaviour
    {
        GameObject[,] RoomGrid;
        public GameObject TestingRoom;
        public GameObject ParryObject;
        public ProcedualLevelCreator LevelGenRef;
        public player _player;
        public static Gmanager instance;
        public CaptureScreen capscreenobj;
        public TextMeshProUGUI EnterRoomTxt;
        public CinemachineVirtualCamera Playercam;
        public CinemachineVirtualCamera RoomCam;
        RectangleShaderActivate RectShader;
        public GameObject RectShaderGameObject;
        /// <summary>
        /// level started is turned true when player presses button to enter level
        /// </summary>
        public bool LevelStarted = false;
        public bool GamePaused = false;
        //level transition is true when next level is loaded and player is entering
        public bool LevelTransition = true;
        public UIMinimapGird minimap;
        RoomGen CurrentRoom;
        public Slicer sliceObj;

        private void Awake()
        {
            instance = this;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            if (LevelGenRef.isActiveAndEnabled)
            {
                RectShaderGameObject.SetActive(true);
                RectShader = RectShaderGameObject.GetComponent<RectangleShaderActivate>();
                print("levelgen is enabled");
                RoomGrid = LevelGenRef.GetComponent<ProcedualLevelCreator>().roomGrid;
                ActivateStartRoom();
            }
            else
            {
                RectShaderGameObject.SetActive(true);
                RectShader = RectShaderGameObject.GetComponent<RectangleShaderActivate>();
                RoomGen roomgen = TestingRoom.GetComponent<RoomGen>();
                CurrentRoom = roomgen;
                roomgen.TweenWait = 0;
                roomgen.OpenDoor('d');
                Transform BottomEntrance = TestingRoom.transform.GetChild(11);
                BottomEntrance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                
                _player.WalkInDir(BottomEntrance.position, Vector2.up);
                //TransferCam(TestingRoom);
                CameraManager.instance.TransferCam(TestingRoom);
                StartDissolve(TestingRoom.transform.position);
            }
        }


        //this should be moved to the player class
        //Player can transition when they pressed button to enter
        //and they have exited the entrance collision zone for the first time
        public bool CanPlayerTransition()
        {
            if (LevelStarted && !LevelTransition)
                return true;
            return false;
        }

        //make a new class that's responsible for moving player
        //other objects like items and predetermined rooms should be able to accesss this easily
        //to acheive this implementation create a subtype of iMovePlayer that does all these things
        //other subtypes perform the move action in different ways
        //method that puts player in next room
        //and other actions...
        public void MovePlayerToRoom(Transform RoomDoor, Vector3 direction)
        {
            print("Moving player into room");


            //start slicing the screen
            //StartRoomEffect(RoomDoor, direction);
            LevelStarted = false;

            
            
            
            print("player walk in dir callled. Level Transition is true");
            LevelTransition = true;

            minimap.AddTile(RoomDoor.position, direction);
            CurrentRoom = RoomDoor.transform.parent.GetComponent<RoomGen>();

            
        }



        //activates the first room in the bottom center of the grid
        //get reference to direction
        void ActivateStartRoom()
        {
            if (RoomGrid != null && LevelGenRef != null)
            {
                for (int x = 0; x < RoomGrid.GetLength(0); x++)
                {
                    for (int y = 0; y < RoomGrid.GetLength(1); y++)
                    {
                        if (x == Mathf.RoundToInt(RoomGrid.GetLength(0) / 2.0f) && y == 0)
                        {
                            GameObject room = RoomGrid[x, y];
                            //start screen effect
                            room.SetActive(true);
                            room.GetComponent<RoomGen>().TweenWait = 0;
                            Transform BottomEntrance = room.transform.GetChild(11);
                            BottomEntrance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            room.GetComponent<RoomGen>().OpenDoor('d');

                            //there probably shouldn't be two calls to player
                            
                            _player.WalkInDir(BottomEntrance.position, Vector2.up);
                            //********************************************************

                            //TransferCam(room);
                            StartCoroutine(StartDissolve(room.transform.position));
                            CameraManager.instance.TransferCam(room);
                            CurrentRoom = room.GetComponent<RoomGen>();
                            //play level transition animation
                        }
                    }
                }
            }
        }


        //should make another class to handle camera logic
        //public void TransferCam(GameObject room)
        //{
        //    print("start transfer cam to room " + room.name + " at " + room.transform.position);
        //    Playercam.gameObject.SetActive(false);
        //    RoomCam.gameObject.SetActive(true);
        //    RoomCam.Follow = room.transform;
            
        //    StartCoroutine(StartDissolve(room.transform.position));
        //}


        //move to camera class
        public void TransferCamToPlayer()
        {
            Playercam.gameObject.SetActive(true);
            RoomCam.gameObject.SetActive(false);
        }


        //these need to be moved to another class that handles shader logic
        IEnumerator StartDissolve(Vector2 RoomPos)
        {
            //print("start dissolve");
            yield return new WaitForSeconds(1);
            RectShader.SetRectRegion(RoomPos);
            print("room pos is " + RoomPos);
            RectShader.activateDissolve = true;
            yield return null;
            
        }

        //move to a camera class
        public void ReturnToPlayer()
        {
            TransferCamToPlayer();
            EnterRoomTxt.enabled = true;
            UITextNotificationController.instance.UpdateText("press " + UITextNotificationController.instance.currentAcceptButton + " to enter");
            print("current room is " + CurrentRoom.name);
        }
        



        //move to player class
        public void MoneyToPlayer()
        {
            _player.AddMoney(5);
            print("player money is " + _player.Money);
        }


        //move to a hit effect subtype, handles hit stop and other things related to hit effects
        //make everything black, make players red
        public void OnParry()
        {
            ParryObject.SetActive(true);
        }



        //maybe this should be in the room class?
        //begins an event where prefabs are spawned on specified ways in the current room depending on spawntype
        public void StartRoomEvent(SpawnEvents SpawnType, GameObject ObjToSpawn)
        {
            switch (SpawnType)
            {
                //all tiles on room
                case SpawnEvents.AllEmptyTiles:
                    print("spawning" + ObjToSpawn.name + "on " + SpawnType.ToString());
                    
                    StartCoroutine(CurrentRoom.CreateOnEmptyTilesRoutine(ObjToSpawn));
                    break;


                case SpawnEvents.TilesOnWall:
                    print("spawning" + ObjToSpawn.name + "on " + SpawnType.ToString());
                    StartCoroutine(CurrentRoom.CreateOnEmptyWallsRoutine(ObjToSpawn));
                    break;

            }
        }

        public void StartGlobalEvent(GameObject objToSpawn)
        {
            Instantiate(objToSpawn, _player.transform.position, Quaternion.identity);
        }
        

        public void ActivateGlobalEvent()
        {
            print("activating global event");
        }

        

        //begins the screen slicing action
        public void StartRoomEffect(Transform room, Vector3 direction)
        {
            
            sliceObj._player = _player ;
            RectShader.RemoveDesolve();
            StartCoroutine(StartDissolve(room.parent.position));
            sliceObj.SliceInitialize(room, direction);
            
        }

    }
}
