using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Playables;

namespace topdown
{
    public class PostLevelBehavior : MonoBehaviour
    {
        public Vector3Int FromGrid;
        public Vector3Int ToGrid;
        public Vector3Int GridSize;
        public GameObject LevelIndicatorPrefab;
        public GameObject GridPrefab;
        public GameObject FollowObj;
        public ParallaxManager ParMan;
        public RectangleShaderActivate BuildingShader;
        public CameraController camController;
        //starting and dest are the game objects shown in 3D scene
        public Transform Starting, Dest;
        public Transform BackDrop;
        public player PlayerTransform;
        public float FovDecreaseDuration, ZoomOutDist, ZoomOutDur;
        [SerializeField] ScreenShotRetrieve screenShot;
        [SerializeField] RectangleShaderActivate RectShader;

        public PlayableDirector Director;
        public float CameraOffsetY;
        Grid currentGrid;
        float rotation = 0;
        [SerializeField]
        Ease ease;

        // Start is called before the first frame update
        void Start()
        {

            //StartCoroutine(PostLevelSequenceRoutine());
            CreateLevelGrid();
            ShowLevelIndicator(FromGrid, Starting);
            ShowLevelIndicator(ToGrid, Dest);
            //make a set level position method?
            BackDrop.position = new Vector3(Dest.position.x, Dest.position.y, BackDrop.position.z);
            PlayerTransform.transform.position = BackDrop.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CreateLevelGrid()
        {
            Vector3 gridPos = new Vector3(FollowObj.transform.position.x, Camera.main.transform.position.y + CameraOffsetY, FollowObj.transform.position.z);

            currentGrid = Instantiate(GridPrefab, gridPos, Quaternion.Euler(new Vector3(90, 0, 0))).GetComponent<Grid>();
            //do a for loop in order to visualize positions
            //for (int i = 0; i < GridSize.x; i++)
            //{
            //    for (int j = 0; j < GridSize.z; j++)
            //    {
            //        Instantiate(LevelIndicatorPrefab, currentGrid.GetCellCenterWorld(new Vector3Int(i, 0, j)), Quaternion.Euler(new Vector3(90, 0, 0)), currentGrid.transform);

            //    }
            //}
        }

        void ShowLevelIndicator(Vector3Int GridPos, Transform t)
        {
            //Instantiate(LevelIndicatorPrefab, currentGrid.CellToWorld(GridPos), Quaternion.Euler(new Vector3(90, 0, 0)), currentGrid.transform);
            t.position = currentGrid.CellToWorld(GridPos);
            //t.gameObject.SetActive(true);

        }

        void startPlayableDirector()
        {
            Director.Play(Director.playableAsset);
        }


        //called when door is interacted with
        //puase time and activate the mesh slicer
        //must move to perspective cam before slicing starts

        public void MoveDirection(Vector3 dir)
        {
            screenShot.PerformCapture();
            screenShot.OnCapture = () =>
            {
                RectShader.RemoveDissolve();
                startPlayableDirector();
                FromGrid = ToGrid;
                ToGrid += Vector3Int.RoundToInt(dir);
                ShowLevelIndicator(FromGrid, Starting);
                ShowLevelIndicator(ToGrid, Dest);
                BackDrop.position = new Vector3(Dest.position.x, Dest.position.y, BackDrop.position.z);
                PlayerTransform.SetPosAndWalkDir(BackDrop.position, dir);
                //PlayerTransform.transform.position = BackDrop.position;
            };
        }


        IEnumerator PostLevelSequenceRoutine()
        {
            //ParMan.TweenParalax(360, 4.5f);
            //yield return new WaitForSeconds(5);
            //displaySection(Vector3.zero);
            //yield return new WaitForSeconds(.5f);
            //displaySection(from - to);
            //ParMan.TweenParalax(-90, 1);
            //yield return null;
            yield return new WaitForSeconds(1);
            //TweenValue(180, 5f, () => SetRotationValues(rotation));
            DOTween.To(() => rotation, x => rotation = x, rotation + 180, 1).SetEase(ease).onUpdate = () => SetRotationValues(rotation);
            yield return new WaitForSeconds(1);
            print("changing fov");
            DOTween.To(() => camController.Fov, x => camController.Fov = x, 1, FovDecreaseDuration);
            DOTween.To(() => camController.orbit.m_FollowOffset.z, x => camController.orbit.m_FollowOffset.z = x, ZoomOutDist, ZoomOutDur);
            yield return new WaitForSeconds(ZoomOutDur);
            BuildingShader.MaskMat.DOFloat(-.3f, "Noise_Strength", 5);
            yield return new WaitForSeconds(FovDecreaseDuration);

            camController.vcam.gameObject.SetActive(false);
        }

        //void TweenValue(float rotationAmount, float duration, Delegate Set)
        //{
        //    print("starting tween");
        //    Tween rotateTween = DOTween.To(() => rotation, x => rotation = x, rotation + rotationAmount, duration);
        //    rotateTween.SetEase(ease);

        //    rotateTween.onUpdate = Set;

        //}

        void SetRotationValues(float value)
        {
            ParMan.AssignParalaxValue(value);
            camController.AssignVcamRotation(value);
        }


        public void SetCoordinate(Vector2 from, Vector2 to)
        {

        }

        void displaySection(Vector3 direction)
        {
            Instantiate(LevelIndicatorPrefab, new Vector3(direction.x * -4, Camera.main.transform.position.y - 3, -2), Quaternion.Euler(new Vector3(90, 0, 0)));
            print("direction is " + direction);
        }

    }
}
