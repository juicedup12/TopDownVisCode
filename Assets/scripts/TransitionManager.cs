using System;
using UnityEngine;
using DG.Tweening;

class TransitionManager: MonoBehaviour
{
    [SerializeField]
    IRoomTransitioner transitioner;
    [SerializeField]
    Transform[] floorObjects, wallobjects, roomobjects;
    public enum Floortransition { spin, flip }
    Floortransition floorTransitionType;
    public static TransitionManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void SetTransition(Floortransition floortransition, Transform[] TransitionObjects, Action OnTransitionFinished)
    {
        switch(floortransition)
        {
            case Floortransition.flip:

                return;


            case Floortransition.spin:

                return;
        }
    }


    void FlipFloorTiles(Transform[,] objs)
    {
        Sequence TileRotSeq = DOTween.Sequence();
        //Transform[,] TileHolder = GenerateWholeRoomFloor();
        int TileGridsizeX = objs.GetLength(0);
        int TileGridSizeY = objs.GetLength(1);
        int xpos;
        for (int x = 0; x < TileGridsizeX; x++)
        {
            for (int y = 0; y <= x; y++)
            {
                if (y > TileGridSizeY - 1)
                { break; }
                //Tween thisTween = TileHolder[x, y].DOPunchRotation(new Vector3(360, 0, 0), 3.5f - (x * .3f), 3, 1).OnStart(Setactive(TileHolder[x, y].gameObject));
                Tween thisTween = objs[x, y].DOLocalRotate(new Vector3(450, 0), Mathf.Clamp(2.5f + (x * .4f), .5f, 2), RotateMode.WorldAxisAdd).SetEase(Ease.InQuad);
                thisTween.OnStart(() => objs[x, y].gameObject.SetActive(true));
                TileRotSeq.Insert((x * .3f) + 2f, thisTween);
                xpos = x;
                if (y == x)
                {
                    do
                    {
                        x--;
                        if (x < 0) break;
                        Tween ThisTween = objs[x, y].DOLocalRotate(new Vector3(450, 0), Mathf.Clamp(2.5f + (xpos * .4f), .5f, 2), RotateMode.WorldAxisAdd).SetEase(Ease.InQuad)
                        .OnStart(() => objs[x, y].gameObject.SetActive(true));
                        TileRotSeq.Insert((xpos * .3f) + 2f,
                    ThisTween);
                    }
                    while (x != 0);
                }
                x = xpos;
            }
        }
    }

}

