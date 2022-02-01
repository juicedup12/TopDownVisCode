using UnityEngine;
using DG.Tweening;

public class wallDoor : MonoBehaviour
{
    public float doorwidth;
    public GameObject wall1;
    public GameObject wall2;
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
        float startToEndDist = Vector2.Distance(startpos, endpos);
        wall1.transform.position = startpos;
        if (Enclosingleft)
        {
            //print("using left");
            wall1.transform.localScale = new Vector3((startToEndDist / 2 - doorwidth), wall1.transform.localScale.y, 1);
            
            wall2.transform.position = endpos;
            wall2.transform.localScale = new Vector3(-(startToEndDist / 2 - doorwidth), wall2.transform.localScale.y, 1);

            float AssetSpawnChance = .4f;
            //spawn on right wall or left door 
            if(AssetSpawnChance <.5f)
            {
                float RandXPos = Random.Range(transform.position.x - assetWidthOffset, ((transform.position.x - (startToEndDist / 2 - doorwidth)) + .2f) + assetWidthOffset);
                //Debug.Log("Right wall getting a random x pos from " + transform.position.x + " and " +( transform.position.x - (startToEndDist / 2 - doorwidth)), this);
                Vector3 Boardspawn = new Vector3(RandXPos, transform.position.y + .5f);

                Instantiate(Officeboard, Boardspawn, Quaternion.identity, gameObject.transform);
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
        }
        else
        {
            wall1.transform.localScale = new Vector3(-(startToEndDist/2 - doorwidth), wall1.transform.localScale.y, 1);
            
            wall2.transform.position = endpos;
            wall2.transform.localScale = new Vector3((startToEndDist/2 - doorwidth ), wall2.transform.localScale.y, 1);

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
