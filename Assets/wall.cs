using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public float offset;
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
        
        if (firstwall == 0)
        {
            transform.position = new Vector3(startpos.x, startpos.y );
            transform.localScale = new Vector3(transform.localScale.x, (endpos.y - startpos.y) , 1);
        }
        else
        {
            
            transform.position = new Vector3(startpos.x, startpos.y );
            transform.localScale = new Vector3(transform.localScale.x, (endpos.y - startpos.y), 1);
        }
    }
}
