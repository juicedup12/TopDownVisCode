using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace topdown {

    //class that the game object uses
    public class Rusher : Enemy
    {

        Vector3 dashpos;
        private bool rushtoplayer = false;
        public Transform attackLocation;
        public float attackRange;

        enum state
        {
            normal, charging,
        }
        float dashspeed = 2.7f;
        state mState = state.normal;
        private bool IsATK;
        public float atktime;


        public float atkactive { get; private set; }

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            SceneLinkedSMB<Rusher>.Initialise(anim, this);

        }

        // Update is called once per frame
        protected override void Update()
        {
            if (mState == state.normal)
            {
                base.Update();
            }
            if (mState == state.charging)
            {
                AtkUpdate();

                StopAllCoroutines();
                if (!rushtoplayer)
                {
                    float step = dashspeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, dashpos, step);
                    dashspeed -= dashspeed * 2 * Time.deltaTime;
                }

                float dist = Vector3.Distance( transform.position, dashpos);
                if (dist < .36f && !rushtoplayer)
                {
                    dashspeed = 2.5f;
                    dashpos = playertransform.position;
                    rushtoplayer = true;
                }
                //if transform is at dashpos then dahs to player

                if (rushtoplayer)
                {
                    float step = dashspeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, dashpos, step);
                    dashspeed -= dashspeed * 2f * Time.deltaTime;
                }


                
                if (dashspeed < .2f && rushtoplayer)
                {
                    mState = state.normal;
                    anim.SetTrigger("charge");
                    mState = state.normal;
                }
            }
        }

        void meleeAtk()
        {
            IsATK = true;
            atkactive = atktime;
        }

        public void AtkUpdate()
        {
            if (atkactive > 0)
            {
                Collider2D[] damage = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, playermask);

                for (int i = 0; i < damage.Length; i++)
                {

                    Debug.Log("hit " + damage[i].name);

                    if (damage[i].tag == "Player") 
                    damage[i].GetComponent<player>().death();
                }

                atkactive -= Time.deltaTime;
            }
            else
            {
                IsATK = false;
            }
        }

        public void charge()
        {
            if (mState == state.normal)
            {
                CodeMonkey.CMDebug.TextPopup("charge", Vector3.zero);
                int rndm = Random.Range(0, 100);

                Vector3 SideVector = -transform.up;
                //if(rndm > 50)
                //{
                //    SideVector *= -1;
                //}

                

                //will add a while loop later to see if addtion value to transform.right hits a wall
                dashpos = playertransform.position - (SideVector * .5f) + (transform.right * .5f);

                Vector3 dirtodash =   dashpos - transform.position ;
                //do raycast to see if there's a wallbehind player
                RaycastHit2D hit =
                Physics2D.Raycast(transform.position, dirtodash, distance: dirtodash.magnitude, layerMask: wallcheck);

                if (hit.collider != null)
                {
                    Debug.Log("dash hit a all");
                    dashpos = hit.point;


                //    SideVector *= -1;
                //    dashpos = playertransform.position - (SideVector * .5f) + (transform.right * .5f);
                //    RaycastHit2D hit2 =
                //Physics2D.Raycast(transform.position, dirtodash, distance: dirtodash.magnitude, layerMask: wallcheck);

                //    //if both sides hit a wall, then go straight to player
                //    if (hit.collider != null)
                //    {
                //        dashpos = playertransform.position +  (transform.right * .5f);
                //    }
                }


                mState = state.charging;
                dashspeed = 2.2f;
            }
        }
    }
}
