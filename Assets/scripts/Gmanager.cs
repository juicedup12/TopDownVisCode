using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

namespace topdown
{
    //disables and enables rooms as the player moves from them
    public class Gmanager : MonoBehaviour
    {
        GameObject[,] RoomGrid;
        public GameObject TestingRoom;
        public LevelCreator LevelGenRef;
        public player _player;
        public static Gmanager instance;
        public CaptureScreen capscreenobj;
        public TextMeshProUGUI EnterRoomTxt;
        public CinemachineVirtualCamera Playercam;
        public CinemachineVirtualCamera RoomCam;
        RectangleShaderActivate RectShader;
        public GameObject RectShaderGameObject;
        public bool LevelStarted = false;
        

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
                RoomGrid = LevelGenRef.GetComponent<LevelCreator>().roomGrid;
                ActivateStartRoom();
            }
            else
            {
                RectShaderGameObject.SetActive(true);
                RectShader = RectShaderGameObject.GetComponent<RectangleShaderActivate>();
                RoomGen roomgen = TestingRoom.GetComponent<RoomGen>();
                roomgen.TweenWait = 0;
                roomgen.OpenDoor('d');
                Transform BottomEntrance = TestingRoom.transform.GetChild(11);
                BottomEntrance.gameObject.GetComponent<SpriteRenderer>().enabled = false;

                _player.WalkInDir(BottomEntrance.position, Vector2.up);
                TransferCam(TestingRoom);
            }
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

                            _player.WalkInDir(BottomEntrance.position, Vector2.up);
                            TransferCam(room);
                            //play level transition animation
                        }
                    }
                }
            }
        }

        public void TransferCam(GameObject room)
        {
            print("start transfer cam");
            RoomCam.gameObject.SetActive(true);
            RoomCam.Follow = room.transform;
            StartCoroutine(StartDissolve(room.transform.position));
            
        }

        IEnumerator StartDissolve(Vector2 RoomPos)
        {
            print("start dissolve");
            yield return new WaitForSeconds(1);
            RectShader.SetRectRegion(RoomPos);
            print("room pos is " + RoomPos);
            RectShader.activateDissolve = true;
            yield return null;
            
        }

        public void ReturnToPlayer()
        {
            RoomCam.gameObject.SetActive(false);

            EnterRoomTxt.enabled = true;
        }
        

        public void HideLevelStartUI()
        {
            EnterRoomTxt.enabled = false;
        }



        public void StartRoomEffect(GameObject room)
        {
            capscreenobj.grab = true;
            TransferCam(room);
            LevelStarted = false;
        }

    }
}
