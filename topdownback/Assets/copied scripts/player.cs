using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace topdown{
    public class player : MonoBehaviour
    {
        public Transform feettransform;
        public Vector3 move;
        public float speed = 2;
        Vector3 movedir;
        public float distToMove = .1f;
        Rigidbody2D rb2d;
        bool attack = false;
        bool isdash = true;
        bool dead = false;
        public Image stambar;
        public LayerMask enemies;

        int shake;
        public bool restrained = false;
        bool shakeright = false;
        public GameObject TargetIndicator;
        TrailRenderer Trail;

        public float dashdist;
        public float soundradius = 1.3f;
        public float maxstam = 100;
        public float currentstam = 100;
        public float stamdecrease = 10;
        public float dashtime;
        public float dashspeed;
        public float distancebetweenimg;
        public float dashcooldown;
        public float dashcontrol = .5f;
        public LayerMask soundlayer;
        public bool IsUnderledge = false;
        public Transform ledgepos;
        public ledgeinteract ledgeref;
        bool enemyledgeatk = false;
        public Gmanager manager;
        Vector3 dashvector;
        public bool canmove = true;
        bool onledge = false;
        public LayerMask hitboxlayer;

        float dashtimeleft;
        float lastimgxpos;
        float lastdash = -100f;

        Vector3 lastpointeddir;
        Vector2 LastDirection;

        public float magclamp = 20;

        public bool isdashing = false;
        bool hasknife = true;

        Animator[] anim;
        Animator upper, feet;
        Animator playeranim;
        public Transform upperbod;

        int knifehash = Animator.StringToHash("knife_swing");

        // Start is called before the first frame update
        void Start()
        {
            Trail = GetComponent<TrailRenderer>();
            anim = GetComponentsInChildren<Animator>();
            playeranim = GetComponent<Animator>();
            SceneLinkedSMB<player>.Initialise(playeranim, this);
            for (int i = 0; i < anim.Length; i++)
            {
                if (anim[i].name == "upper")
                {
                    upper = anim[i];
                }
                else
                {
                    feet = anim[i];
                }
            }
            rb2d = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

            if(stambar != null)
            stambar.fillAmount = currentstam / 100;
                

                

            if (currentstam < 100)
            {
                currentstam += Time.deltaTime * (stamdecrease / 2);
            }
                
            
                

        }


        private void FixedUpdate()
        {
            if (canmove && Mathf.Abs(movedir.magnitude) > .5)
            {
                rb2d.velocity = movedir * speed * Time.deltaTime;
                //Debug.Log("canmove");
            }
        }

        //for SMB
        public void IsPressingDash()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playeranim.SetTrigger("dash");
            }

            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                playeranim.SetTrigger("dashatk");
            }
        }
        

        public void handledash()
        {
            
            lastpointeddir += move * dashcontrol;
            

            checkdash();
        }
        
        public void Dashinput()
        {
            
            lastpointeddir = move;
            //isdashing = true;
            if (Time.time >= (lastdash + dashcooldown))
                attemptTodash();
            



            if (Input.GetKeyUp(KeyCode.Space))
            {

                isdashing = false;
                //rb2d.velocity -= 0.5f * rb2d.velocity;
                canmove = true;
            }
        }

        void attemptTodash()
        {
            if (currentstam > 0)
            {
                Debug.Log("attempt to dash");
                isdashing = true;
                dashtimeleft = dashtime;
                lastdash = Time.time;
                afterImgPool.instance.getfrompool();
                lastimgxpos = transform.position.x;
            }
        }

        void checkdash()
        {
                canmove = false;
                Vector3 dashdir = dashspeed * lastpointeddir ;
                rb2d.velocity = Vector3.ClampMagnitude(dashdir, magclamp) ;
                dashtimeleft -= Time.deltaTime;
                if (Mathf.Abs(transform.position.x - lastimgxpos) > distancebetweenimg)
                {
                    afterImgPool.instance.getfrompool();
                    lastimgxpos = transform.position.x;
                }

                currentstam -= Time.deltaTime * stamdecrease;
            
        }


        public void dashATK()
        {
            dashvector = movedir * dashdist;

            //add box cast and trail renderer
            RaycastHit2D[] hitlinecol = Physics2D.LinecastAll(transform.position, transform.position + dashvector, enemies);
            foreach(RaycastHit2D ray in hitlinecol)
            if (ray.collider != null)
            {
                    StartCoroutine(slowmoATK());
                    ray.collider.GetComponent<Enemy>().dieSlice();
            }
            transform.position += dashvector;
        }

        IEnumerator slowmoATK()
        {
            Time.timeScale = .2f;
            yield return new WaitForSeconds(.3f);
            Time.timeScale = 1;
        }

        public void activatetrail()
        {
            Trail.enabled = true;
            StartCoroutine("deactivatetrail");
        }

        IEnumerator deactivatetrail()
        {
            yield return new WaitForSeconds(.3f);
            Trail.enabled = false;
        }


        public bool ledgeATK()
        {
            Debug.Log("ledgeatk");
            

            transform.position = Vector3.MoveTowards(transform.position, ledgeref.currentEnemy.position, .6f * Time.deltaTime);
            float dist = Vector2.Distance(transform.position, ledgeref.currentEnemy.position);
            if (dist <= .1f)
            {
                ledgeref.currentEnemy.GetComponent<Enemy>().die();
                
                onledge = false;
                enemyledgeatk = false;
                rb2d.bodyType = RigidbodyType2D.Dynamic;
                return true;
            }
            return false;
        }

        //moves player to ledge(glitchy)
        public void GoToLedge()
        {
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            movedir = Vector3.zero;
            float dist = Vector2.Distance(transform.position, ledgepos.position);

            //keep moving until position is close enough
            if (dist > .05f)
                transform.position = Vector3.MoveTowards(transform.position, ledgepos.position, 1.5f * Time.deltaTime);
        }

        //true if shift pressed under ledge
        public bool ledgecheck()
        {
            if (IsUnderledge)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    onledge = true;
                    Debug.Log("is on ledge");
                    return true;
                }
            }
            return false;
        }


        //logic while player is on ledge
        public void LedgeUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //get off ledge check for collision 
                playeranim.SetTrigger("movement");
                rb2d.bodyType = RigidbodyType2D.Dynamic;
            }

            if(ledgeref.currentEnemy != null)
            {
                TargetIndicator.SetActive(true);
                TargetIndicator.transform.position = ledgeref.currentEnemy.position;
            }

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                //switch target
                ledgeref.switchtarget();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (ledgeref.Enemylist != null)
                {
                    float dist = Vector2.Distance(transform.position, ledgeref.transform.position);

                    //check if player is actually on the ledge before attacking
                    if (dist <= .1f)
                    {

                        playeranim.SetTrigger("ledgeATK");
                    }
                }
            }
        }

        public void deactivateIndicator()
        {
            TargetIndicator.SetActive(false);
        }

        public void UpdateRotation()
        {
            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (move.sqrMagnitude > 0)
            {
                float moveangle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
                feettransform.rotation = Quaternion.AngleAxis(moveangle, Vector3.forward);
            }
            else
            {
                feettransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            }
        }


        public bool IsAnamationFinished()
        {
            AnimatorStateInfo upanim = upper.GetCurrentAnimatorStateInfo(0);
            if(upanim.IsName("knife_swing"))
            {
                Debug.Log("swinging knife");
                Debug.Log(upper.GetCurrentAnimatorStateInfo(0).length);
            }

            if (upanim.IsName("knife_idle"))
            {
                Debug.Log("idling");
                return true;
            }

                return false;
        }

        public bool Isattacking()
        {
            bool isatk = Input.GetKeyDown(KeyCode.Mouse0);
            if (upper != null)
            {
                upper.SetBool("hasknife", hasknife);

                if (isatk)
                {
                    upper.SetTrigger("attack");
                    //makesound();
                }
            }
            return isatk;


        }

        public void SlowmodeInput()
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                Time.timeScale = .5f;
            }
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                Time.timeScale = 1f;
            }
        }

        public void MovementInput()
        {
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");
            playeranim.SetFloat("DirX", move.x);
            playeranim.SetFloat("DirY", move.y);

            if(move.magnitude > .1)
            {
                playeranim.SetFloat("LastDirX", move.x * 10);
                playeranim.SetFloat("LastDirY", move.y * 10);
            }

            movedir = move.normalized;
            //feet.SetFloat("forward", move.normalized.magnitude);

        }

        public void restrain()
        {
            movedir = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            Debug.Log("restraining");
            playeranim.SetTrigger("restrain");
            shake = 0;
            restrained = true;
        }

        public void Breakfree()
        {
            rb2d.velocity = Vector2.zero;
            if (shake > 10)
            {
                restrained = false;
                playeranim.SetTrigger("restrain");
            }

            float shakedir = Input.GetAxis("Horizontal");

            if(shakeright)
            {
                if(shakedir > 0)
                {
                    shake++;
                    Debug.Log("shook right");
                    shakeright = false;
                }
            }
            else
            {
                if(shakedir < 0)
                {
                    shake++;
                    Debug.Log("shook left");
                    shakeright = true;
                }
            }
        }

        

        public void death()
        {
            upper.SetTrigger("death");
            dead = true;
        }

        public void ResetPlayer()
        {
            dead = false;
            upper.SetTrigger("idle");
        }

        public void makesound()
        {
            Debug.Log("make sound");
            GameObject soundpos = new GameObject("sound origin");
            soundpos.layer = 11;
            soundpos.transform.position = transform.position;
            Collider2D enemycol = Physics2D.OverlapCircle(transform.position, soundradius, enemies);
            if (enemycol != null)
            {
                if (enemycol.CompareTag("enemy"))
                {
                    Debug.Log("col is " + enemycol.name);
                    //tell the enemy that it's checking sound
                    Enemy enemyunit = enemycol.GetComponent<Enemy>();
                    enemyunit.GoToSound(soundpos.transform);
                    //enemypatrol.isgoingtosound = true;
                    //enemypatrol.timer = Time.time + 2.0f;
                }
            }
            else
            {
                Debug.Log("no colliders");
            }
        }


        public void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + dashvector);
        }


        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("enemy"))
            {
                if (collision.IsTouchingLayers(hitboxlayer))
                {
                    Debug.Log("back attack hit" + collision.name);
                    collision.GetComponent<Enemy>().die();
                }
            }
            if(collision == null)
            {
                Debug.Log("hit nothing");
            }
        }

    }

}