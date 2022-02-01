using UnityEngine;


//steps through the path defined by the nodegrid
public class enemypathfind : MonoBehaviour
{
    public NodeGrid gridref;
    public float speed = .05f;
    int nodestep = 0;
    public GameObject player;
    Vector3 postogoto;
    bool arrivedatpos = false;
    public bool chasingplayer;
    Node nodetogoto;
    int xpos, ypos;
    bool isgoingtosound;
    Node nodepos;
    Node secondnode;
    public bool isidle = true;
    bool gotnodepos = false;
    public float damping;
    Vector3 secondnodepos;
    Vector3 secondndnodedir;
    public GameObject feet;
    Animator feetanim;
    public float MaxDistToPlayer = .3f;
    public float forceadd;
    Rigidbody2D rgb2d;
    public pathfinding pathcreate;
    public int dist;

    // Start is called before the first frame update
    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();

        feetanim = feet.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isidle )
        {
            if (arrivedatpos)
            {
                //nodetogoto = gridref.path[nodestep];
                Debug.Log("node to go to pos is" + nodetogoto.worlppos.ToString());
            }

            //retrieves the node closest to the enemy everytime the previous node was reached
            //if (!gotnodepos && gridref.path.Count > 2)
            //{
            //    nodetogoto = gridref.path[nodestep];

            //    postogoto = nodetogoto.worlppos - transform.position;
            //    xpos = nodetogoto.gridx; ypos = nodetogoto.gridy;
            //    //Debug.Log("nodepos is" + nodetogoto.worlppos + "x and y is " + xpos + " " + ypos);
            //    gotnodepos = true;


            //}
            
            nodepos = gridref.NodeFromWorldPoint(transform.position);


            


            feetanim.SetFloat("forward", postogoto.magnitude);
            //Vector3 vectorToTarget = postogoto - transform.position;

            if (!chasingplayer)
            {
                float angle = Mathf.Atan2(postogoto.y, postogoto.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * damping);
                feet.transform.rotation = Quaternion.AngleAxis(angle, feet.transform.forward);
                //transform.t = nodetogoto.worlppos;
            }
            else
            {
                //rotate feet to movedir
                float angle = Mathf.Atan2(postogoto.y, postogoto.x) * Mathf.Rad2Deg;
                Quaternion qfeet = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = Quaternion.Slerp(feet.transform.rotation, qfeet, Time.deltaTime * damping);
                feet.transform.rotation = Quaternion.AngleAxis(angle, feet.transform.forward);
                //face player
                Vector3 toplayerdir =   player.transform.position - transform.position;
                float topangle = Mathf.Atan2(toplayerdir.y, toplayerdir.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(topangle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * damping);
                //transform.rotation = Quaternion.AngleAxis(topangle, transform.forward);

            }
            

            if (nodepos.gridx == xpos && nodepos.gridy == ypos)
            {
                gotnodepos = false;
                //Debug.Log("arrived at node");
                //nodestep++;
            }


            
        }


    }

    private void FixedUpdate()
    {
        //if (gridref.path != null)
        //{
        //    if (gridref.path.Count >= 2)
        //    {
        //        //transform.Translate(postogoto.normalized * speed * Time.deltaTime, Space.World);
        //        rgb2d.velocity = postogoto.normalized * speed * Time.deltaTime;
        //        pathcreate.movetowardtarget = true;
        //    }
        //    else
        //    {
        //        rgb2d.velocity = Vector2.zero;
        //    }
        //}
        //else
        //{
        //    rgb2d.velocity = Vector2.zero;
        //}

        //if(gridref.path.Count <= 6)
        //{
        //    rgb2d.velocity = postogoto.normalized * speed * Time.deltaTime;
        //    pathcreate.movetowardtarget = false;
        //    pathcreate.IsFarEnough(dist);
            
        //}
    }
}
