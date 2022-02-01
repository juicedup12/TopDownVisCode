using UnityEngine;


namespace topdown
{

    //class that the game object uses
    public class Rusher : Enemy, IparryInterface
    {

        Vector3 dashpos;
        private bool rushtoplayer = false;
        public Transform attackLocation;
        public float attackRange;
        public Animator WeaponAnim;
        public float sideDashDistance;
        bool finishedAtk = false;
        public bool IsStunned;

        enum state
        {
            normal, charging,
        }
        public float dashspeed = 2.7f;
        state mState = state.normal;
        public float atktime;


        public float atkactive { get; private set; }

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            SceneLinkedSMB<Rusher>.Initialise(anim, this);
        }

        protected override void Start()
        {
            WeaponAnim = WeaponTransform.gameObject.GetComponent<Animator>();
            base.Start();

        }

        // Update is called once per frame
        protected override void Update()
        {
            if (dead) return;
            if (mState == state.normal)
            {
                base.Update();
            }
            if (mState == state.charging)
            {
                //AtkUpdate();

                StopAllCoroutines();

                //move to side dash position
                if (!rushtoplayer)
                {
                    float step = dashspeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, dashpos, step);
                    dashspeed -= dashspeed * 1.3f * Time.deltaTime;
                }

                //check distance
                float dist = Vector3.Distance( transform.position, dashpos);
                if (dist < .36f || dashspeed < .1 && !rushtoplayer)
                {
                    dashspeed = 2.5f;
                    dashpos = playertransform.position;
                    rushtoplayer = true;
                }
                //if transform is at dashpos then dash to player

                if (rushtoplayer)
                {
                    float step = dashspeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, dashpos, step);
                    dashspeed -= dashspeed * 1.3f * Time.deltaTime;


                }

                if (!finishedAtk)
                {
                    if (rushtoplayer && dist < .3f || dashspeed < .2)
                    {
                        //play weapon attack animation

                        WeaponAnim.SetTrigger("atk");
                        print("starting weapon atk");
                        finishedAtk = true;
                    }
                }

                float distToPlayer = Vector3.Distance(transform.position, playertransform.position);

                if (dashspeed < .1f || distToPlayer < .1 && finishedAtk)
                {
                    print("going back to pursuing player");
                    mState = state.normal;
                    //anim.SetTrigger("charge");
                    mState = state.normal;
                }
            }
        }

        

        //public void AtkUpdate()
        //{
            
        //        Collider2D[] damage = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, playermask);

        //        for (int i = 0; i < damage.Length; i++)
        //        {

        //            Debug.Log("hit " + damage[i].name);

        //            if (damage[i].tag == "Player") 
        //            damage[i].GetComponent<player>().TakeDamage(transform.position);
        //        }
                
            
        //}


        public void Stunned()
        {
            anim.SetTrigger("stun");
            WeaponAnim.SetTrigger("stun");
            IsStunned = true;
        }

        public override void Die()
        {

            if (IsStunned && !dead)
            {
                print("enemy was attacked while stunned");
                Gmanager.instance.OnParry();
            }
            base.Die();
        }

        public void charge()
        {
            if (mState == state.normal)
            {
                CodeMonkey.CMDebug.TextPopup("charge", Vector3.zero);
                float rndm = Random.value;
                RaycastHit2D hit;
                Vector3 forward = -transform.up;
                print(rndm + " is the random value");


                if (rndm < .33)
                {
                    dashpos = playertransform.position - (forward * .5f) + (transform.right * sideDashDistance);
                    print("rusher dashing to its right");
                }
                else if( rndm >.33 && rndm < .66)
                {
                    dashpos = playertransform.position - (forward * .5f) + (-transform.right *  sideDashDistance);
                    print("rusher dashing to left");
                }
                else if(rndm > .66)
                {
                    dashspeed = 2.5f;
                    dashpos = playertransform.position;
                    rushtoplayer = true;
                    print("rusher dashing straight to player");
                }

                Vector3 dirtodash =   dashpos - transform.position ;

                //do raycast to see if there's a wallbehind player
                hit = Physics2D.Raycast(transform.position, dirtodash, distance: dirtodash.magnitude, layerMask: wallcheck);

                //if there's a wall, make the wall the limit of dash
                if (hit.collider != null)
                {
                    Debug.Log("dash hit a wall");
                    dashpos = hit.point;
                    
                    #region
                    //    SideVector *= -1;
                    //    dashpos = playertransform.position - (SideVector * .5f) + (transform.right * .5f);
                    //    RaycastHit2D hit2 =
                    //Physics2D.Raycast(transform.position, dirtodash, distance: dirtodash.magnitude, layerMask: wallcheck);

                    //    //if both sides hit a wall, then go straight to player
                    //    if (hit.collider != null)
                    //    {
                    //        dashpos = playertransform.position +  (transform.right * .5f);
                    //    }'
                    #endregion
                }


                mState = state.charging;
                finishedAtk = false;
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
           
            if (collision.tag == "Hurtbox" && collision.transform.parent.tag == "Player")
            {

                if (collision.IsTouchingLayers(LayerMask.GetMask("EnemyHitbox")))
                {

                    print("rusher attack collided with " + collision.name);

                    Debug.Log("doing damage to player", gameObject);
                    Gmanager.instance._player.TakeDamage(transform.position);
                }
            }
        }
    }
}
