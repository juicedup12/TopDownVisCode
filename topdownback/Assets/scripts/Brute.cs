using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace topdown {
    public class Brute : Enemy
    {
        Vector3 playerpos;
        enum state
        {
            normal, charging,
        }
        public float dashspeed = 1.8f;

        state mState = state.normal;
        public Transform attackLocation;
        public float attackRange;

        public bool atkactive = false;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            SceneLinkedSMB<Brute>.Initialise(anim, this);

        }

        // Update is called once per frame
        protected override void Update()
        {
            if (mState == state.normal)
            {
                base.Update();
                atkactive = false;
            }
            if(mState == state.charging)
            {
                attack();
                atkactive = true;
                StopAllCoroutines();
                float step = dashspeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, playerpos, step);

                dashspeed -= dashspeed * .8f * Time.deltaTime;


                //when it's slowing down
                if (dashspeed < .3f)
                {
                    //go back to pursue state
                    mState = state.normal;
                    anim.SetTrigger("charge");
                    mState = state.normal;
                    dashspeed = 1.8f;
                }
            }


        }

        


        //activates hurtbox
        public void attack()
        {
            if (!dead)
            {
                Collider2D damage = Physics2D.OverlapBox(attackLocation.position, Vector2.one / attackRange, playermask);
                atkactive = true;
                Debug.Log("hit " + damage.name);

                if (damage.tag == "Player")
                    damage.GetComponent<player>().death();
            }   
        }


        public void swing()
        {
            if (mState == state.normal)
            {
                playerpos = playertransform.position + (transform.right * .1f);
                mState = state.charging;


            }

        }

        public void charge()
        {
            if (mState == state.normal)
            {
                Vector3 playerposadded = playertransform.position + (transform.right * .2f);
                Vector3 dirtoplayer = playertransform.position - transform.position;
                //do raycast to see if there's a wallbehind player
                RaycastHit2D hit = 
                Physics2D.Raycast(transform.position, dirtoplayer, distance: playerposadded.magnitude, layerMask: wallcheck);

                // If it hits something...
                if (hit.collider != null)
                {
                    Debug.Log("wall was hit");
                    playerpos = hit.point;
                    mState = state.charging;
                }
                else
                {
                    playerpos = playerposadded;
                    mState = state.charging;
                }

            }
            
        }


        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (atkactive)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(attackLocation.position, Vector3.one / attackRange);
                
            }
        }

    }
}
