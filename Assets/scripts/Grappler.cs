using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class Grappler : Enemy
    {
        public Transform bullet;
        public Transform bulletspawn;
        public Vector3 playerpos;
        public Vector3 playerposneg;
        EdgeCollider2D ropecol;
        public Vector3 playerposplus;
        public LayerMask ropelayer;
        LineRenderer l;
        public float angle;
        public float lerpspeed;
        public float anglelerpseed;
        bool Isrestraining = false;
        public Vector3 RopeEnd;
        player playref;

        bool hastouched;
        List<Vector3> points = new List<Vector3>();
        enum state
        {
            normal, throwing,
        }
        state mState = state.normal;

        enum ropestate
        {
            traveling, retracting,
        }
        ropestate mRopeState = ropestate.traveling;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            SceneLinkedSMB<Grappler>.Initialise(anim, this);
            l = GetComponentInChildren<LineRenderer>();
            ropecol = l.GetComponent<EdgeCollider2D>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (!Isrestraining)
            {
                if (mState == state.normal)
                {
                    base.Update();
                }
                else
                {
                    AddColliderToLine(transform.position, RopeEnd);
                    ropecol.enabled = true;
                    l.useWorldSpace = true;
                    l.startWidth = 0.06f;
                    l.endWidth = 0.06f;
                    l.SetPosition(1, RopeEnd);
                    l.SetPosition(0, transform.position);
                    //playerpos = Quaternion.AngleAxis(angle, Vector3.forward) * playerpos;

                    if (mRopeState == ropestate.traveling)
                    {
                        //rope end lerps to player pos and player pos moves in an angle
                        RopeEnd = Vector3.Lerp(RopeEnd, playerposplus, lerpspeed * Time.deltaTime);
                        playerposplus = Vector3.MoveTowards(playerposplus, playerposneg, anglelerpseed * Time.deltaTime);

                        float distancefromendpoint = Vector3.Distance(RopeEnd, playerposneg);
                        if (distancefromendpoint < .2f)
                        {
                            mRopeState = ropestate.retracting;
                            Debug.Log("retracting rope");
                        }
                    }
                    else
                    {
                        //make rope go back to enemy
                        RopeEnd = Vector3.Lerp(RopeEnd, transform.position, 2 * lerpspeed * Time.deltaTime);

                        float distancefromendpoint = Vector3.Distance(RopeEnd, transform.position);
                        if (distancefromendpoint < .1f)
                        {
                            //Debug.Log("going to normal");
                            mState = state.normal;
                            anim.SetTrigger("throwing");
                            ropecol.enabled = false;
                        }

                    }
                }
            }
            else
            {
               if(!playref.restrained)
                {
                    Isrestraining = false;

                }
            }
        }


        public bool canShoot()
        {
            float distance = Vector3.Distance(transform.position, OrientTo);
            if (isFacingplayer() && distance < 1)
            {
                return true;
            }

            return false;
        }

        public void ThrowGrapple()
        {
            if (mState == state.normal)
            {
                hastouched = false;
                l.useWorldSpace = true;
                RopeEnd = transform.position;
                l.positionCount = 2;

                l.SetPosition(0, transform.position);
                l.SetPosition(1, RopeEnd);
                mRopeState = ropestate.traveling;
                anim.SetTrigger("throwing");
                playerpos = playertransform.position;
                playerposneg = Quaternion.AngleAxis(-angle  , Vector3.forward) * playerpos ;
                playerposplus = Quaternion.AngleAxis(angle  , Vector3.forward)  * (playerpos * 2);
                
                mState = state.throwing;
                
            }
        }

        private void AddColliderToLine(Vector3 startPoint, Vector3 endPoint)
        {
            Vector2[] points = new Vector2[2];
            points[0] = Vector2.zero;
            Vector2 dir = endPoint - transform.position ;
            dir = Quaternion.AngleAxis(-transform.eulerAngles.z, transform.forward) * dir;
            points[1] = dir ;
            ropecol.points = points;

        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("wall"))
            {
                Debug.Log("rope hit wall", this);
                return;
            }

            if (collision.IsTouching(ropecol) && collision.CompareTag("Player"))
            {
                

                if (!Isrestraining && !hastouched )
                {
                    Isrestraining = true;
                    hastouched = true;
                    Debug.Log("rope collided with " + collision.name);
                    playref = collision.GetComponent<player>();
                    playref.restrain();
                }
            }
        }

    }
}
