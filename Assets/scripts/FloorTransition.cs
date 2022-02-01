using DG.Tweening;
using UnityEngine;

class FloorTransition : MonoBehaviour, IRoomTransitioner
{
    public Transform[,] FloorTiles;
    public enum TransitionType { flip, spin}
    public TransitionType Transition;
    [SerializeField]
    private Vector2 gridworldsize;
    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private float TileDiameter, GridScalar;

    //tileparent will hold tile game objects for organization
    private GameObject TileParent;


    //might need a tile radius or diameter var
    public void DoRoomTransition()
    {
        createTilegridPositions();
        switch(Transition)
        {
            case TransitionType.flip:
                FlipFloorTiles(450);
                
                return;

            case TransitionType.spin:
                spinFloorTiles(450);
                return;
        }
    }

    void PopTiles()
    {
        print("popping tiles");
        foreach(Transform t in FloorTiles)
        {
            t.DOPunchScale(Vector3.one, 3, 3, 1).OnStart(() => t.gameObject.SetActive(true));
        }
    }

    void FlipFloorTiles(float Xrotation)
    {
        foreach (Transform t in FloorTiles)
        {   
            t.rotation = Quaternion.Euler(90,0,0);
            
        }
        Sequence TileRotSeq = DOTween.Sequence();
        int TileGridsizeX = FloorTiles.GetLength(0);
        int TileGridSizeY = FloorTiles.GetLength(1);

            int xpos;
            for (int x = 0; x < TileGridsizeX; x++)
            {
                for (int y = 0; y <= x ; y++)
                {
                    if(y > TileGridSizeY -1)
                    { break; }
                    //Tween thisTween = TileHolder[x, y].DOPunchRotation(new Vector3(360, 0, 0), 3.5f - (x * .3f), 3, 1).OnStart(Setactive(TileHolder[x, y].gameObject));
                    Tween thisTween = FloorTiles[x, y].DOLocalRotate(new Vector3(Xrotation, 0), Mathf.Clamp(2.5f + (x * .4f), .5f, 2), RotateMode.WorldAxisAdd).SetEase(Ease.InSine);
                   
                    TileRotSeq.Insert((x * .3f) + 2f, thisTween);
                    xpos = x;
                    if(y == x)
                    {
                        do
                        {
                            x--;
                            if (x < 0) break;
                            Tween ThisTween = FloorTiles[x, y].DOLocalRotate(new Vector3(Xrotation, 0), Mathf.Clamp( 2.5f + (xpos * .4f),.5f, 2 ), RotateMode.WorldAxisAdd).SetEase(Ease.InSine); 
                            
                            TileRotSeq.Insert((xpos * .3f) + 2f,
                        ThisTween);
                        }
                        while (x != 0);
                    }
                    x = xpos;
                }
            }
    }

   

    void spinFloorTiles(float YRotation)
    {
       Sequence TileRotSeq = DOTween.Sequence();
       //Transform[,] TileHolder = GenerateWholeRoomFloor();
       int TileGridsizeX = FloorTiles.GetLength(0);
       int TileGridSizeY = FloorTiles.GetLength(1);
       int xpos;
       for (int x = 0; x < TileGridsizeX; x++)
       {
           for (int y = 0; y <= x; y++)
           {
               if (y > TileGridSizeY - 1)
               { break; }
               //Tween thisTween = TileHolder[x, y].DOPunchRotation(new Vector3(360, 0, 0), 3.5f - (x * .3f), 3, 1).OnStart(Setactive(TileHolder[x, y].gameObject));
               Tween thisTween = FloorTiles[x, y].DOLocalRotate(new Vector3(0,YRotation), Mathf.Clamp(2.5f + (x * .4f), .5f, 2), RotateMode.WorldAxisAdd).SetEase(Ease.InQuad);
                
                //set object active and rotate to make it invisible
                thisTween.OnStart(() => {FloorTiles[x, y].gameObject.SetActive(true); FloorTiles[x, y].rotation  = Quaternion.Euler(0, 90,0); });

               TileRotSeq.Insert((x * .3f) + 2f, thisTween);
               xpos = x;
               if (y == x)
               {
                   do
                   {
                       x--;
                       if (x < 0) break;
                       Tween ThisTween = FloorTiles[x, y].DOLocalRotate(new Vector3(450, 0), Mathf.Clamp(2.5f + (xpos * .4f), .5f, 2), RotateMode.WorldAxisAdd).SetEase(Ease.InQuad)
                       .OnStart(() => FloorTiles[x, y].gameObject.SetActive(true));
                       TileRotSeq.Insert((xpos * .3f) + 2f,
                   ThisTween);
                   }
                   while (x != 0);
               }
               x = xpos;
           }
       }
    }



    



    void createTilegridPositions()
    {
        //set size of grid
        int TileGridSizeX = Mathf.RoundToInt(gridworldsize.x * GridScalar / TileDiameter);
        int TileGridSizeY = Mathf.RoundToInt(gridworldsize.y * GridScalar / TileDiameter);
        //makes transform reference values for the tile objects
        FloorTiles = new Transform[TileGridSizeX, TileGridSizeY];
        //sets the position of the start of the grid relative to this object's pos
        Vector3 worldbottomleft = transform.position - Vector3.right * (gridworldsize.x * GridScalar) / 2 - Vector3.up * (gridworldsize.y * GridScalar) / 2;
        //print("world bottom left is " + worldbottomleft);
        //print("first world point is  " +  (worldbottomleft + Vector3.right * (0 * TileDiameter + TileRadius)
        //+ Vector3.up * (0 * TileDiameter + TileRadius)));

        TileParent = new GameObject();
        TileParent.name = "tile holder";

        for (int x = 0; x < TileGridSizeX; x++)
        {
            for (int y = 0; y < TileGridSizeY; y++)
            {
                //the world point of x and y
                Vector3 worldpoint = worldbottomleft + Vector3.right * (x * TileDiameter + 1)
                    + Vector3.up * (y * TileDiameter + 1);



                //this is where we'll check if the game object is instantiated
                //if it isn't instantiated, then we will set it's transform to the newly created one
                if (FloorTiles[x,y] == null)
                {
                    //make a folder for tiles
                    

                    //instantiate tiles
                    GameObject Thistile = Instantiate(TilePrefab, worldpoint, Quaternion.identity);
                    FloorTiles[x, y] = Thistile.transform;

                    
                    Thistile.transform.SetParent(TileParent.transform);
                    //Thistile.SetActive(false);

                    
                    
                    
                }
                else
                {
                    //change positions of floortile transform positions to worldpoint
                    FloorTiles[x, y].position = worldpoint;
                }
                    
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridworldsize.x, gridworldsize.y, 1));
        Gizmos.color = Color.red;

        //set size of grid
        int TileGridSizeX = Mathf.RoundToInt(gridworldsize.x * GridScalar / TileDiameter);
        int TileGridSizeY = Mathf.RoundToInt(gridworldsize.y * GridScalar / TileDiameter);

        //sets the position of the start of the grid relative to this object's pos
        Vector3 worldbottomleft = transform.position - Vector3.right * (gridworldsize.x * GridScalar) / 2 - Vector3.up * (gridworldsize.y * GridScalar) / 2;
        for (int x = 0; x < TileGridSizeX; x++)
        {
            for (int y = 0; y < TileGridSizeY; y++)
            {
                //the world point of x and y
                Vector3 worldpoint = worldbottomleft + Vector3.right * (x * TileDiameter + 1)
                    + Vector3.up * (y * TileDiameter + 1);

                Gizmos.DrawWireCube(worldpoint, Vector3.one);
            }
        }
    }

}

