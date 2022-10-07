using UnityEngine;
using DG.Tweening;

/// <summary>
/// handles setting size of walls aswell as becoming when player goes behind the wall
/// </summary>
public class wallDoor : MonoBehaviour
{
    public float doorwidth;
    public GameObject Pivot1;
    public GameObject Pivot2;
    public SpriteRenderer sprRndr1, sprRndr2;
    public float AssetPosScaler;
    public float assetWidthOffset;

    public GameObject Officeboard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    //scales wall to the half of the room's size with space for the door
    public void SetHorizontalWall(Vector2 startpos, Vector2 endpos, bool Enclosingleft)
    {
        float startToEndDist = startpos.x - endpos.x;
        float wallSize = Mathf.Abs(startToEndDist) / 2 - doorwidth;
        print("wall size is " + wallSize + "start to end dist is " + startToEndDist);
        //wall1.transform.position = startpos;
        //change to enclosing left
        if (true)
        {
            //print("using left");
            //wall1.transform.localScale = new Vector3((Mathf.Abs( startToEndDist) / 2 - doorwidth), wall1.transform.localScale.y, 1);
            Pivot1.transform.localScale = new Vector3(wallSize, 1);
            //test
            //Pivot1.transform.localScale = new Vector3(startToEndDist, 1);

            Pivot2.transform.position -= new Vector3(Mathf.Abs(startToEndDist) / 2 + doorwidth, 0);
            Pivot2.transform.localScale = new Vector3(wallSize, 1);
            //wall2.transform.position = new Vector3(transform.position.x - Mathf.Abs( startToEndDist), transform.position.y);
            //wall2.transform.localScale = new Vector3(-(Mathf.Abs( startToEndDist) / 2 - doorwidth), wall2.transform.localScale.y, 1);

            //if (startToEndDist < 0)
            //{
            //    print(gameObject.name + " to be flipped");
            //    Transform p = transform.parent;
            //    print("parent is " + p);
            //    if (p)
            //        p.transform.localScale = new Vector3(-1, 1, 1);
            //}
            float AssetSpawnChance = .4f;
            //spawn on right wall or left door 
            if(AssetSpawnChance <.5f)
            {
                float RandXPos = Random.Range(transform.position.x - assetWidthOffset, ((transform.position.x - (Mathf.Abs( startToEndDist) / 2 - doorwidth)) + .2f) + assetWidthOffset);
                //Debug.Log("Right wall getting a random x pos from " + transform.position.x + " and " +( transform.position.x - (startToEndDist / 2 - doorwidth)), this);
                Vector3 Boardspawn = new Vector3(RandXPos, transform.position.y + .5f);

                Instantiate(Officeboard, Boardspawn, Quaternion.identity, gameObject.transform).transform.parent = transform;
            }
            else
            {
                //float RandXPos = Random.Range(transform.position.x + assetWidthOffset, (transform.position.x + (startToEndDist / 2 - doorwidth)) - assetWidthOffset);

                float LeftEnd = (endpos.x + assetWidthOffset);
                float RightEnd = (endpos.x + (startToEndDist / 2 - doorwidth)) - .2f - assetWidthOffset;
                float RandXPos = Random.Range(LeftEnd, (endpos.x + (startToEndDist / 2 - doorwidth) - .2f) - assetWidthOffset);
                
                //Debug.Log("Left wall getting a random x pos from " + LeftEnd + " and " + RightEnd, this);
                Vector3 Boardspawn = new Vector3(RandXPos, transform.position.y + .5f);
                Instantiate(Officeboard, Boardspawn, Quaternion.identity, gameObject.transform);

            }
            if (startToEndDist < 0)
            {
                //print(gameObject.name + " to be flipped");
                //Transform p = transform.parent;
                //print("parent is " + p);
                //removed in order to scale room pivot in a sperate method
                //if (p)
                    //p.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            Pivot1.transform.localScale = new Vector3(-(startToEndDist/2 - doorwidth), Pivot1.transform.localScale.y, 1);
            
            Pivot2.transform.position = endpos;
            Pivot2.transform.localScale = new Vector3((startToEndDist/2 - doorwidth ), Pivot2.transform.localScale.y, 1);

            float AssetSpawnChance = Random.value;
            //spawn on right wall or left door 
            if (AssetSpawnChance < .5f)
            {
                float LeftEnd = transform.position.x + assetWidthOffset;
                float RightEnd = (transform.position.x + (startToEndDist / 2 - doorwidth) - .2f) - assetWidthOffset;
                float RandXPos = Random.Range(LeftEnd, RightEnd);
                //Debug.Log("Left wall wall getting a random x pos from " + LeftEnd + " and " + RightEnd, this);
                Vector3 Boardspawn = new Vector3(RandXPos, transform.position.y + .5f);

                Instantiate(Officeboard, Boardspawn, Quaternion.identity, gameObject.transform);
            }
            else
            {
                //float RandXPos = Random.Range(transform.position.x + assetWidthOffset, (transform.position.x + (startToEndDist / 2 - doorwidth)) - assetWidthOffset);

                float RightEnd = (endpos.x - assetWidthOffset);
                float LeftEnd = (endpos.x - (startToEndDist / 2 - doorwidth)) + .2f + assetWidthOffset;
                float RandXPos = Random.Range(RightEnd, LeftEnd);

                //Debug.Log("Right wall getting a random x pos from " + RightEnd + " and " + LeftEnd, this);
                Vector3 Boardspawn = new Vector3(RandXPos, transform.position.y + .5f);
                Instantiate(Officeboard, Boardspawn, Quaternion.identity, gameObject.transform);

            }
        }
    }


    public void SetInnerWallLayer(string SortlayerName)
    {
        sprRndr1.sortingLayerName = SortlayerName;
        sprRndr2.sortingLayerName = SortlayerName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            DOTweenModuleSprite.DOColor(sprRndr1, new Color(1,1,1,.5f), 1);
            DOTweenModuleSprite.DOColor(sprRndr2, new Color(1, 1, 1, .5f), 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DOTweenModuleSprite.DOColor(sprRndr1, new Color(1, 1, 1, 1), 1);
            DOTweenModuleSprite.DOColor(sprRndr2, new Color(1, 1, 1, 1), 1);
        }
    }
}
