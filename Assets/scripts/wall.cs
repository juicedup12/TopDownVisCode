using UnityEngine;

public class wall : MonoBehaviour
{
    public float offset;
    Vector3 Midpoint;
    public GameObject ledge;
    public bool LowerWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setwallpos(Vector2 startpos, Vector2 endpos, int firstwall)
    {

        //if (firstwall == 0)
        //{
        //    transform.position = new Vector3(startpos.x, startpos.y );
        //    transform.localScale = new Vector3(transform.localScale.x, (endpos.y - startpos.y) , 1);
        //}
        //else
        //{

        //    transform.position = new Vector3(startpos.x, startpos.y );
        //    transform.localScale = new Vector3(transform.localScale.x, (endpos.y - startpos.y), 1);
        //}

        transform.position = new Vector3(startpos.x, startpos.y);
        transform.parent.localScale = new Vector3(transform.localScale.x, Mathf.Abs(endpos.y - startpos.y), 1);

        float rndm = Random.value;
        if(rndm < .3)
        {
            ledge.SetActive(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.IsTouchingLayers(1 << 16) && LowerWall)
        {
            if (collision.CompareTag("Player") )
            {
                print("player on carpet");
                collision.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("InnerWall");
            }
        }
        else if(collision.IsTouchingLayers(1 << 16) && !LowerWall)
        {
            if (collision.CompareTag("Player"))
            {
                print("player on carpet");
                collision.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("front");
            }
        }
    }


}
