using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace topdown{
    public class player : MonoBehaviour
    {
        public Transform feettransform;
        public float LookAheadSpeed = 1.5f;
        public Vector3 move;
        public float speed = 2;
        Vector3 movedir;
        public float distToMove = .1f;
        Rigidbody2D rb2d;
        bool attack = false;
        bool isdash = true;
        bool dead = false;
        public bool SequenceDone;
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
        public bool TransitioningToLevel = false;
        bool onledge = false;
        public LayerMask hitboxlayer;
        Vector2 mousepos;
        float dashtimeleft;
        float lastimgxpos;
        float lastdash = -100f;

        Vector3 lastpointeddir;
        Vector2 LastDirection;

        Vector3 lastpos;
        Vector3 newpos;

        public float magclamp = 20;

        public bool isdashing = false;
        bool hasknife = true;

        Vector2 WalkIntoLevelDir;

        Animator[] anim;
        Animator upper, feet;
        Animator playeranim;
        public Transform upperbod;

        public Transform LookAheadTransform;

        public Controlls _inputActions;
        InputActionAsset inputActions;
        PlayerInput playerInput;
        
        int knifehash = Animator.StringToHash("knife_swing");
        private Vector2 lookaheadDir;

        [HideInInspector]
        public bool PressedAccept { get;  set; }
        Vector2 InputMove { get; set; }
        Vector2 InputLook { get; set; }

        private void Awake()
        {
            _inputActions = new Controlls();

        }

        private void OnEnable()
        {
            RoomGen.OnDoor += WalkInDir;
            _inputActions.Enable();
            _inputActions.Player.Accept.performed += OnAcceptButton;
            _inputActions.Player.Accept.canceled += ctx => PressedAccept = false;
            _inputActions.Player.Walking.performed += onMoveInput;
            _inputActions.Player.Walking.canceled += ctx => InputMove = Vector3.zero;
            _inputActions.Player.Look.performed += onLookInput;
            _inputActions.Player.Look.canceled += ctx => InputLook = Vector2.zero;
        }

        private void OnDisable()
        {
            RoomGen.OnDoor += WalkInDir;
            _inputActions.Disable();
            _inputActions.Player.Accept.performed -= OnAcceptButton;
            _inputActions.Player.Accept.canceled -= ctx => PressedAccept = false;
            _inputActions.Player.Walking.performed -= onMoveInput;
            _inputActions.Player.Walking.canceled -= ctx => InputMove = Vector3.zero;
            _inputActions.Player.Look.performed -= onLookInput;
        }

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
            if (canmove && move.magnitude > .2f)
            {
                print("fixed update, movedir is " + move);

                rb2d.velocity = move * speed * Time.fixedDeltaTime;
                
            }
        }

        //private void LateUpdate()
        //{
        //    float newposdist = Vector3.Distance(lastpos, newpos);
        //    if (newposdist > .2)
        //    {
                
        //        newpos.x = Mathf.Round(newpos.x * 32 / 32f);
        //        newpos.y = Mathf.Round(newpos.y * 32 / 32f);
                
        //            transform.position = PPURounder.RoundToMultiple(transform.position);

        //            Debug.Log("new pos is " + newpos);
                
        //        lastpos = transform.position;
        //    }
        //}

        //for SMB



        private void OnAcceptButton(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            Debug.Log("pressing accept");
            PressedAccept = value >+ .1f;
        }

        public void IsPressingSpace()
        {
            //if (Input.GetKeyDown(KeyCode.Space) )
            //{
            //    playeranim.SetTrigger("dash");
            //}
            

            //if (Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.Mouse0) )
            //{
            //    playeranim.SetTrigger("dashatk");
            //}
        }
        
        void onMoveInput(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            
                InputMove = value;
            
            //else
            //{
            //    InputMove = Vector3.zero;
            //}

        }

        void onLookInput(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            InputLook = value;
            //Debug.Log("input look is " + value);
            
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
            



            //if (Input.GetKeyUp(KeyCode.Space))
            //{

            //    isdashing = false;
            //    //rb2d.velocity -= 0.5f * rb2d.velocity;
            //    canmove = true;
            //}
        }

        void attemptTodash()
        {
            if (currentstam > 0)
            {
                Debug.Log("attempt to dash");
                isdashing = true;
                dashtimeleft = dashtime;
                lastdash = Time.time;
                if (afterImgPool.instance == null)
                    return;
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
                if (afterImgPool.instance == null)
                    return;
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
                //if (Input.GetKeyDown(KeyCode.LeftShift))
                //{
                //    onledge = true;
                //    Debug.Log("is on ledge");
                //    return true;
                //}
            }
            return false;
        }


        //logic while player is on ledge
        public void LedgeUpdate()
        {
            //if (Input.GetKeyDown(KeyCode.LeftShift))
            //{
            //    //get off ledge check for collision 
            //    playeranim.SetTrigger("movement");
            //    rb2d.bodyType = RigidbodyType2D.Dynamic;
            //}

            //if(ledgeref.currentEnemy != null)
            //{
            //    TargetIndicator.SetActive(true);
            //    TargetIndicator.transform.position = ledgeref.currentEnemy.position;
            //}

            //if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            //{
            //    //switch target
            //    ledgeref.switchtarget();
            //}

            //if (Input.GetKeyDown(KeyCode.Mouse1))
            //{
            //    if (ledgeref.Enemylist != null)
            //    {
            //        float dist = Vector2.Distance(transform.position, ledgeref.transform.position);

            //        //check if player is actually on the ledge before attacking
            //        if (dist <= .1f)
            //        {

            //            playeranim.SetTrigger("ledgeATK");
            //        }
            //    }
            //}
        }

        public void deactivateIndicator()
        {
            TargetIndicator.SetActive(false);
        }

        public void UpdateRotation()
        {
            //Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //if (move.sqrMagnitude > 0)
            //{
            //    float moveangle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            //    feettransform.rotation = Quaternion.AngleAxis(moveangle, Vector3.forward);
            //}
            //else
            //{
            //    feettransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //}
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
            //bool isatk = Input.GetKeyDown(KeyCode.Mouse0);
            //if (upper != null)
            //{
            //    upper.SetBool("hasknife", hasknife);

            //    if (isatk)
            //    {
            //        upper.SetTrigger("attack");
            //        //makesound();
            //    }
            //}
            //return isatk;
            return false;


        }

        public void SlowmodeInput()
        {
            //if(Input.GetKeyDown(KeyCode.LeftShift))
            //{
            //    Time.timeScale = .5f;
            //}
            //if(Input.GetKeyUp(KeyCode.LeftShift))
            //{
            //    Time.timeScale = 1f;
            //}
        }
        


        public void MovementInput()
        {
            if (!TransitioningToLevel )
            {
                //print("moving input move is " + InputMove);
                move.x = InputMove.x;
                move.y = InputMove.y;
            }
            
            if(move.magnitude < 0)
            {
                print("Move is zero");
                move = Vector3.zero;
            }

            MoveLookAhead();

            playeranim.SetFloat("DirX", move.x);
            playeranim.SetFloat("DirY", move.y);

            if(move.magnitude > .1)
            {
                playeranim.SetFloat("LastDirX", move.x * 10);
                playeranim.SetFloat("LastDirY", move.y * 10);
            }
            else

            movedir = move.normalized;

            if(movedir.magnitude <0)
            {
                print("Movedir is zero");
                movedir = Vector3.zero;
            }
            //feet.SetFloat("forward", move.normalized.magnitude);

        }

        public void MoveLookAhead()
        {
            
            if (Mouse.current.delta.ReadValue().magnitude > .4)
            {
                
                mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                //Vector3.Normalize(InputLook);
                //print("cam to screen mouse is " + Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                //print("mouse pos is " + Mouse.current.position.ReadValue());
                //print("normalized input look is " + mousepos.normalized);

                
                lookaheadDir = mousepos - (Vector2)transform.position;
                 
                print("mouse being moved mouse dif is " + lookaheadDir);
                LookAheadTransform.position = transform.position + new Vector3(Mathf.Clamp(lookaheadDir.x, -2, 2), Mathf.Clamp(lookaheadDir.y, -2, 2)) / 5f;
                //LookAheadTransform.position = transform.position + new Vector3(Mathf.Clamp(lookaheadDir.x, -2, 2), Mathf.Clamp(lookaheadDir.y, -2, 2));
                return;
            }
            if (lookaheadDir != (Vector2)move && InputLook.magnitude < .1)
            {
                //InputLook = Vector2.zero;
                //lookaheadDir = new Vector2(Mathf.Round(move.x), Mathf.Round(move.y));
                lookaheadDir = move  * 2;
                //print("look ahead dir is " + move);
                //lookaheadDir = (transform.position + move) * 2;
                //float t = (int)(((LookAheadSpeed * Time.deltaTime) / (1/32)) + 0.5f) * (1/32);
                //LookAheadTransform.position = Vector3.Lerp(LookAheadTransform.position, lookaheadDir, LookAheadSpeed);
                

                LookAheadTransform.position = transform.position + ((Vector3)lookaheadDir);
                return;
            }
            
            if(InputLook.magnitude > .1)
            {//if the analog is being moved
                lookaheadDir = InputLook * 2;
                print("input look mag more than .2");
                LookAheadTransform.position = transform.position + (Vector3)lookaheadDir;
                //InputLook = Vector2.zero;

            }
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
            //upper.SetTrigger("death");
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

        public void DisablePlayerCol()
        {
            //Debug.Log("disable collider");
            GetComponent<CapsuleCollider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;
        }

        void EnableCollider()
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
        }

        //for punching
        public void InchForward()
        {
            AnimatorClipInfo[] m_CurrentClipInfo;
            m_CurrentClipInfo = playeranim.GetCurrentAnimatorClipInfo(0);
            string m_ClipName = m_CurrentClipInfo[0].clip.name;
            Debug.Log("clib length is " + m_CurrentClipInfo[0].clip.length);
            if ( m_ClipName == "BackATK")
            {
                transform.position += new Vector3(0, .3f, 0);
            }
        }

        
        //first places player in a door of room 
        public void WalkInDir(Vector2 pos, Vector2 Dir)
        {
            canmove = false;
            playeranim.SetTrigger("Wait");
            WalkIntoLevelDir = Dir;
            DisablePlayerCol();
            SequenceDone = false;
            transform.position = pos;
            TransitioningToLevel = true;
            move = Vector2.zero;

            //StartCoroutine(StartWalking(Dir));
        }

        //makes the plaer walk into room after button input
        public void WalkIntoLevel()
        {
            playeranim.SetTrigger("RoomEnter");
            CodeMonkey.CMDebug.TextPopup("Entering room", transform.position);
            Gmanager.instance.LevelStarted = true;
            DisablePlayerCol();
            move = Vector3.zero;
            StartCoroutine(StartWalking(WalkIntoLevelDir));

        }

        IEnumerator WaitForKeyDown()
        {
            while (!Input.GetKeyDown(KeyCode.Space))
                yield return null;
        }

        IEnumerator StartWalking(Vector2 Dir)
        {
            yield return new WaitForEndOfFrame();
            canmove = true;
            print("started walking co routine");
            
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && SequenceDone);
            Gmanager.instance.HideLevelStartUI();
            float secondstoReach = 1.5f;
            TransitioningToLevel = true;
            while (secondstoReach > 0)
            {
                move = Dir.normalized;
                secondstoReach -= Time.deltaTime;
                if(InputMove.magnitude > .1)
                {
                    secondstoReach = 0;
                }
                yield return null;
            }
            move = Vector3.zero;
            EnableCollider();
            TransitioningToLevel = false;
            
            GetComponent<SpriteRenderer>().enabled = true;
            EnableCollider();
            print("finished walking co routine");
            yield return null;
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