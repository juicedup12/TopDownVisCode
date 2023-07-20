using InteractableSelect;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Yarn.Unity;


namespace topdown
{
    public class player : MonoBehaviour, IUseItem
    {

        #region ints
        //ints

        int PlayerHealth = 200;
        [SerializeField]
        private int MaxHealth;
        int wait;
        int shake;
        public int Money;
        #endregion


        #region floats
        //floats
        public float Knockbackdist;
        public float DashPressDuration;
        public float InventoryDir;
        float WalkLookAheadLerpSpeed = .5f;
        public float magclamp = 20;
        public float LookAheadSpeed = 1.5f;
        public float speed = 2;
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
        float dashtimeleft;
        float lastimgxpos;
        float lastdash = -100f;
        public float aimfloat;
        public float AimdotSpeed;
        #endregion

        #region vectors
        //vectors

        private Vector2 lookaheadDir;

        Vector2 WalkIntoLevelDir;
        Vector2 mousepos;
        Vector3 movedir;
        public Vector3 move;
        Vector3 dashvector;
        public Vector2 InputMove { get; private set; }
        Vector2 InputLook { get; set; }
        Vector2 InputMousePos { get; set; }
        Vector3 lastpointeddir;
        Vector2 LastDirection;
        Vector3 lastpos;
        Vector3 newpos;
        #endregion

        #region bools
        //bools

        [HideInInspector]
        public bool Parry { get; private set; }
        public bool CanRegainStamina = false;
        public bool PressedAccept { get; private set; }
        bool canbeDamaged = true;
        bool OnMouseControl { get; set; }
        public bool OpenInventory;
        bool hasknife = true;
        bool onledge = false;
        bool enemyledgeatk = false;
        bool attack = false;
        bool isdash = true;
        bool dead = false;
        public bool SequenceDone;
        public bool restrained = false;
        public bool CloseInventory;
        //public bool ReleaseAttack;
        bool shakeright = false;
        public bool InDialogue;
        public bool TransitioningToLevel = false;
        #endregion


        #region components/ class references
        //components
        [Header("component references")]
        public StaminaController stambar;
        public MoneyUIBehavior moneyUI;
        public PostProcessController postProcessCtrl;
        public GameObject HoldDashIndicator;
        public GameObject swoosh;
        [SerializeReference] Slicer sliceObj;
        CapsuleCollider2D Capcol;
        private ItemSelectionManager _itemSelectionManager;
        public GameObject SlashIndicator;
        PressBehavior dashpress;
        public BoxCollider2D Parrybox;
        public Transform LookAheadTransform;
        public GameObject sparkanimation;
        public Transform upperbod;
        public LayerMask hitboxlayer;
        public HealthUIController healthUI;
        PlayerInput playerInput;
        Animator[] anim;
        Animator upper, feet;
        Animator playeranim;
        public PlayerInventoryController inventory;
        Controlls _inputActions;
        public GameObject hurtboxobj;
        public BoxCollider2D Hurtbox;
        BoxCollider2D HitBox;
        //public DialogueUI uidialogue;

        Rigidbody2D rb2d;
        public LayerMask enemies;
        public GameObject TargetIndicator;
        TrailRenderer Trail;
        public GameObject SlashAnimation;
        public LayerMask soundlayer;
        public LayerMask EnemyHit;
        public bool IsUnderledge = false;
        public Transform ledgepos;
        public ledgeinteract ledgeref;
        public Transform EnemyLedgeAtk;
        LineRenderer AimLine;
        PlayerInteractable PlayerInteractableTarget;
        Material AimMat;
        #endregion

        #region Callbacks
        public event Action ParryAction;
        public event Action AttackAction;
        public event Action DashCompleteAction;
        Action AcceptAction;
        #endregion

        string device;
        public Vector3 descenddir;

        private void Awake()
        {

            rb2d = GetComponent<Rigidbody2D>();
            _inputActions = new Controlls();
            playeranim = GetComponent<Animator>();
        }

        private void OnEnable()
        {

            InputSystem.onActionChange += CheckForInputControlChange;

            _inputActions.Enable();
            _inputActions.UI.Disable();

            _inputActions.Player.Interact.performed += OnInteract;
            //_inputActions.Player.Accept.canceled += ctx => PressedAccept = false;
            _inputActions.Player.Walking.performed += onMoveInput;
            _inputActions.Player.Walking.canceled += onMoveInput;
            _inputActions.Player.Look.performed += onLookInput;
            _inputActions.Player.Look.canceled += onLookInput;
            _inputActions.Player.Attack.performed += AttackInput;
            _inputActions.Player.Dash.performed += DashInput;
            _inputActions.Player.MouseLook.performed += onmouseLook;
            _inputActions.Player.OpenInventory.performed += InventoryInput;
            _inputActions.Player.Pause.performed += PauseInput;
            _inputActions.Player.SlowMo.performed += SlowmodeInput;
            _inputActions.Player.SlowMo.canceled += SlowmodeInput;
            _inputActions.UI.Accept.performed += UIAcceptInput;
            SceneLinkedSMB<player>.Initialise(playeranim, this);
        }

        private void OnDisable()
        {
            InputSystem.onActionChange -= CheckForInputControlChange;

            _inputActions.Disable();

            _inputActions.Player.Interact.performed -= OnInteract;
            //_inputActions.Player.Accept.canceled += ctx => PressedAccept = false;
            _inputActions.Player.Walking.performed -= onMoveInput;
            _inputActions.Player.Walking.canceled -= onMoveInput;
            _inputActions.Player.Look.performed -= onLookInput;
            _inputActions.Player.Look.canceled -= onLookInput;
            _inputActions.Player.Attack.performed -= AttackInput;
            _inputActions.Player.Dash.performed -= DashInput;
            _inputActions.Player.MouseLook.performed -= onmouseLook;
            _inputActions.Player.OpenInventory.performed -= InventoryInput;
            _inputActions.Player.Pause.performed -= PauseInput;
            _inputActions.Player.SlowMo.performed -= SlowmodeInput;
            _inputActions.Player.SlowMo.canceled -= SlowmodeInput;
            _inputActions.UI.Accept.performed -= UIAcceptInput;

        }

        // Start is called before the first frame update
        void Start()
        {

            HitBox = SlashIndicator.GetComponent<BoxCollider2D>();
            _itemSelectionManager = GetComponentInChildren<ItemSelectionManager>();
            inventory = GetComponent<PlayerInventoryController>();
            Trail = GetComponent<TrailRenderer>();
            anim = GetComponentsInChildren<Animator>();
            AimLine = GetComponent<LineRenderer>();
            if (AimMat)
                AimMat = AimLine.material;

            SceneLinkedSMB<PlayerInventoryController>.Initialise(playeranim, inventory);
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

            wait = Animator.StringToHash("Wait");
            Capcol = GetComponent<CapsuleCollider2D>();

            //AddMoney(10);


            if (healthUI != null)
            {
                //healthUI.setCurrentHealth(PlayerHealth);
                //healthUI.ChangePlayerHealth(PlayerHealth);

            }

        }

        // Update is called once per frame
        void Update()
        {

            if (stambar != null)
                stambar.SetFillAmount(currentstam / 100);

            if (currentstam < 100 && CanRegainStamina)
            {
                currentstam += Time.deltaTime * (stamdecrease / 2);
            }


        }



        private void FixedUpdate()
        {
            if (playeranim.GetCurrentAnimatorStateInfo(0).IsName("WalkTree"))
            {
                //print("player in walk tree");
                if (InputMove.magnitude > 0)
                {

                    rb2d.velocity = InputMove * speed * Time.fixedDeltaTime;
                }
            }
            


            if (lookaheadDir.magnitude > 1.5)
                LookAheadTransform.position = transform.position + new Vector3(Mathf.Clamp(lookaheadDir.x, -4, 4), Mathf.Clamp(lookaheadDir.y, -4, 4)) / 2f;
            else LookAheadTransform.position = transform.position;
        }

        private void CheckForInputControlChange(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                //Debug.Log($"device is gamepad: {lastDevice is Gamepad}");



                ControllerType TypeofController = lastDevice is Keyboard || lastDevice is Mouse ? ControllerType.Keyboard : ControllerType.GamePad;

                //Debug.Log($"device is :{TypeofController.ToString()} ");

                if (UITextNotificationController.instance != null)
                {
                    UITextNotificationController.instance.CheckControllerChange(TypeofController);
                }
            }
        }

        void UIAcceptInput(InputAction.CallbackContext ctx)
        {
            AcceptAction?.Invoke();
        }

        void InventoryInput(InputAction.CallbackContext ctx)
        {
            playeranim.SetTrigger("Inventory");
        }

        void AttackInput(InputAction.CallbackContext ctx)
        {
            if (ctx.interaction is HoldInteraction) playeranim.SetTrigger("HoldPunch");
            else
            {
                playeranim.SetTrigger("ATK"); 
            }
        }

        void PauseInput(InputAction.CallbackContext ctx)
        {
            PauseUI.instantce.ShowPauseMenu();
        }


        void MovementInput()
        {
            if (!TransitioningToLevel && InputMove.magnitude > .2f)
            {
                //print("moving input move is " + InputMove);
                move.x = InputMove.x;
                move.y = InputMove.y;
            }

            if (move.magnitude < 0)
            {
                print("Move is zero");
                move = Vector3.zero;
            }


            playeranim.SetFloat("DirX", rb2d.velocity.x);
            playeranim.SetFloat("DirY", rb2d.velocity.y);

            //if(move.magnitude > .1)
            //{
            //    playeranim.SetFloat("LastDirX", move.x * 10);
            //    playeranim.SetFloat("LastDirY", move.y * 10);
            //}
            //else

            movedir = move.normalized;

            if (movedir.magnitude < 0)
            {
                print("Movedir is zero");
                movedir = Vector3.zero;
            }
            //feet.SetFloat("forward", move.normalized.magnitude);

        }

        void DashInput(InputAction.CallbackContext ctx)
        {
            if( move.magnitude > .2 && currentstam > 30)
            playeranim.SetTrigger("dash");
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            _itemSelectionManager.InteractWithObject();
        }

        void onMoveInput(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            InputMove = value.normalized;
            //print("input move is " + InputMove);
        }

        public void AddMoney(int amount)
        {
            int currentmoney = Money;
            moneyUI.ReflectMoneyChange(currentmoney, amount);
            Money += amount;
        }

        void onLookInput(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            InputLook = value;
            //Debug.Log("input look is " + value);
        }


        //public void ControlSlicer()
        //{
        //    if (!sliceObj) return;
        //    if (!sliceObj.isActiveAndEnabled) return;
        //    sliceObj.MouseInputPos = InputMousePos;
        //    //print("slice mouse input is " + InputMousePos);
        //    if(Attacking)
        //    {
        //        print("slicer attack");
        //        Attacking = false;
        //    }
        //    if( _inputActions.Player.Attack.WasPerformedThisFrame())
        //        {
        //        print("attack performed this frame");

        //        sliceObj.MouseClick();
        //    }
        //}

        public void ActivateParry(bool activate)
        {
            if (activate)
                Parrybox.enabled = true;
            else
                Parrybox.enabled = false;
        }



        //move this code to a seperate class that checks for animation states
        public void FinishedAction(int tutnum)
        {

            //if a tutorial wants to know if an attack happened then fire event
            switch (tutnum)
            {
                case 0:
                    AttackAction?.Invoke();
                    break;
                case 1:
                    print("invoking dashcomplete action");
                    print("dash complete has a method?: " + DashCompleteAction == null);
                    DashCompleteAction?.Invoke();
                    break;
            }

        }

        void onmouseLook(InputAction.CallbackContext context)
        {
            if (Mouse.current.delta.ReadValue().magnitude > 5)
            {
                if (!OnMouseControl)
                {
                    //print("starting mouse look");
                    StopCoroutine("MouseLookReset");
                }
                var val = context.ReadValue<Vector2>();
                OnMouseControl = true;
                InputMousePos = val;
                //Vector2.Lerp(LookAheadTransform.position, InputMousePos, .5f);
                //inventory.PointDir = val;
            }
            else if (Mouse.current.delta.ReadValue().magnitude < 5 && OnMouseControl)
            {
                //print("stopped moving mouse");
                StartCoroutine("MouseLookReset"); //print("stopped moving mouse");
            }
        }

        public Vector2 getmousepos()
        {
            return InputMousePos;
        }

        IEnumerator MouseLookReset()
        {
            yield return new WaitForSecondsRealtime(.8f);
            WalkLookAheadLerpSpeed = .2f;
            OnMouseControl = false;
            //print("reseting mouse control");
            yield return new WaitForSeconds(.2f);
            WalkLookAheadLerpSpeed = .5f;


        }

        public void UsePotion(int amount)
        {
            //print("healed player by " + amount + " with item " + item.name);
            ChangeHealth(50);
        }


        public void UsePoison(int dmg)
        {
            print("player drank poison");
            TakeDamage(transform.position);
        }

        void ChangeHealth(int healamnt)
        {

            PlayerHealth = PlayerHealth + healamnt > MaxHealth ? MaxHealth : PlayerHealth + healamnt;
            int heal = PlayerHealth;

            print("healing player by  " + heal);
            healthUI.ChangePlayerHealth(heal);
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

        }

      

        //public bool CanHoldDash()
        //{
        //    if (Dashing && move.magnitude < .2  && currentstam > 50)
        //    {
        //        currentstam -= 50;
        //        return true;
        //    }
        //    return false;
        //}

        public void ReduceStamina(int stam)
        {
            currentstam -= stam;
        }


        IEnumerator RegainStamina()
        {
            yield return new WaitForSeconds(.5f);
            CanRegainStamina = true;

        }

        public void EnableHoldDashIndicator(bool Activate)
        {
            if (Activate)
                HoldDashIndicator.SetActive(true);
            else
                HoldDashIndicator.SetActive(false);

        }

        public bool AddToInventory(ItemClass item)
        {
            if (inventory)
            {
                if (inventory.AddItem(item))
                {

                    //show item confirmation
                    ItemConfirmation.instance.ShowItemConfirmation(true, item);
                    return true;
                }
            }


            //Activate a UI to tell player that inventory is full
            ItemConfirmation.instance.ShowItemConfirmation(false);

            return false;


        }

        public void InventoryClose(bool ThrowItem = false)
        {
            if (!ThrowItem)
            {
                print("not trowing, closing inventory");
                playeranim.SetTrigger("movement");
                OpenInventory = false;
            }
            else
            {
                print("inventory close setting trigger to throw");
                playeranim.SetTrigger("Throw");
                OpenInventory = false;
            }

        }

        public void EnableAttackHitbox()
        {
            //HitBox.enabled = true;
        }

        public void DisableAttackHitbox()
        {
            //HitBox.enabled = false;
        }

        void attemptTodash()
        {
            Debug.Log("attempt to dash");
            dashtimeleft = dashtime;
            lastdash = Time.time;
            if (afterImgPool.instance == null)
                return;
            afterImgPool.instance.getfrompool();
            lastimgxpos = transform.position.x;
            handledash();

        }


        public void DoStamRegain(bool start)
        {
            if (start)
                StartCoroutine(RegainStamina());
            else
            {
                CanRegainStamina = false;
                StopCoroutine(RegainStamina());
            }
        }

        public void setVelocity(bool SleepOn)
        {
            if (SleepOn)
            {
                rb2d.velocity = Vector2.zero;
                print("rigidbody velocity set to " + rb2d.velocity);
            }
            else
            {
                rb2d.WakeUp();
            }

        }

        //changes rb2d velocity
        void checkdash()
        {
            Vector3 dashdir = dashspeed * lastpointeddir;
            rb2d.velocity = Vector3.ClampMagnitude(dashdir, magclamp);
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
            dashvector = lookaheadDir * dashdist;

            //add box cast and trail renderer
            RaycastHit2D[] hitlinecol = Physics2D.LinecastAll(transform.position, transform.position + dashvector, enemies);
            foreach (RaycastHit2D ray in hitlinecol)
                if (ray.collider != null)
                {

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
            //Trail.enabled = true;
            SlashAnimation.SetActive(true);
            float angle = Mathf.Atan2(lookaheadDir.y, lookaheadDir.x) * Mathf.Rad2Deg;
            SlashAnimation.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            SlashAnimation.transform.position = transform.position;
            StartCoroutine("deactivatetrail");
        }

        IEnumerator deactivatetrail()
        {
            yield return new WaitForSeconds(.6f);
            SlashAnimation.SetActive(false);
            //Trail.enabled = false;
        }

        public void OnLedgeEnter()
        {
            ledgeref.LedgeAttackAction = () =>
            {
                if (ledgeref.currentEnemy != null)
                {
                    playeranim.SetTrigger("ledgeATK");
                    print("attacking ledge enmy");
                    InteractUIBehavior.instance.HideUI();
                }
                else
                {
                    print("no ledge enemy to attack");
                }
            };
        }

        public void LedgeAttack()
        {

        }

        public void LedgeMove()
        {
            Debug.Log("Moving away from ledge");


            transform.position = Vector3.MoveTowards(transform.position, -descenddir, .6f * Time.deltaTime);
            //float dist = Vector2.Distance(transform.position, ledgeref.currentEnemy.position);
            //if (dist <= .1f)
            //{
            //    //ledgeref.currentEnemy.GetComponent<Enemy>().Die();

            //    onledge = false;
            //    enemyledgeatk = false;
            //    rb2d.bodyType = RigidbodyType2D.Dynamic;
            //    return true;
            //}
            //return false;
        }

        public void SetLedge(ledgeinteract ledge)
        {
            playeranim.SetTrigger("ledge");
            ledgeref = ledge;
            ledgepos = ledgeref.transform;
            EnemyLedgeAtk = ledgeref.RetrieveCurrentEnemy();

        }

        public void OnLedge()
        {
            Vector2 ledgedir = ledgepos.position - transform.position;
            descenddir = ledgedir.x < 0 ? transform.position - transform.right : transform.position + transform.right;
            print("descend direction is " + descenddir);

            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            movedir = Vector3.zero;
        }

        public void Offledge()
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
        }


        //transfer code to onledge enter or set ledge
        //moves player to ledge(glitchy)
        public void GoToLedge()
        {

            float dist = Vector2.Distance(transform.position, ledgepos.position);

            //keep moving until position is close enough
            if (dist > .05f)
                transform.position = Vector3.MoveTowards(transform.position, ledgepos.position, 1.5f * Time.deltaTime);

        }

        //performs an update on ledge code and handles player input
        public void LedgeInput()
        {
            ledgeref.switchtarget(InputMove.x);

            ledgeref.EnemySearch();

            //player choses to descend
            if (Parry)
            {
                //move in the direction player got on to the ledge
                print("getting off ledge");
                playeranim.SetTrigger("movement");
            }

            //if(Attacking )
            //{
            //    EnemyLedgeAtk = ledgeref.RetrieveCurrentEnemy();
            //    if(EnemyLedgeAtk != null)
            //    {
            //        print("player attacking enemy from ledge");
            //        playeranim.SetTrigger("ledgeATK");
            //    }
            //}

        }



        public void MoveInSeconds(ref float timer, Vector3 startpos, Vector3 endPos, float time)
        {

            timer += Time.deltaTime / time;

            transform.position = Vector3.Lerp(startpos, endPos, timer);
            //yield return null;

            //playeranim.SetTrigger("moving");
            //print("done moving player to position");
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
            if (upanim.IsName("knife_swing"))
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

        public void SlowmodeInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {

                if (currentstam > 1)
                {
                    //activate slowmode effects on slowmode manager
                    postProcessCtrl.SetBlurActive(true);
                    if (Time.timeScale > .51)
                    {
                        Time.timeScale = .5f;
                    }
                }
                else
                {
                    postProcessCtrl.SetBlurActive(false);
                    Time.timeScale = 1;
                }
            }
            else if(ctx.canceled)
            {
                print("canceled slowmo"); 
                postProcessCtrl.SetBlurActive(false); 
                Time.timeScale = 1;
            }
        }

        public void MoveLookAhead()
        {

            if (Mouse.current.delta.ReadValue().magnitude > .2)
            {

                mousepos = Camera.main.ScreenToWorldPoint(InputMousePos);
                //Vector3.Normalize(InputLook);
                //print("cam to screen mouse is " + Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                //print("mouse pos is " + Mouse.current.position.ReadValue());
                //print("normalized input look is " + mousepos.normalized);


                lookaheadDir = mousepos - (Vector2)transform.position;

                inventory.PointDir = lookaheadDir;
                //print("mouse being moved mouse dif is " + lookaheadDir);
                //if (lookaheadDir.magnitude > 3)
                //    LookAheadTransform.position = transform.position + new Vector3(Mathf.Clamp(lookaheadDir.x, -4, 4), Mathf.Clamp(lookaheadDir.y, -4, 4)) / 2f;
                //else LookAheadTransform.position = transform.position;
                //LookAheadTransform.position = Vector2.Lerp(LookAheadTransform.position, InputMousePos, .5f);

                    //Vector2 AimDir = LookAheadTransform.position - transform.position;
                float angle = Mathf.Atan2(lookaheadDir.y, lookaheadDir.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.Euler(0f, 0f, angle - 90);
                SlashIndicator.transform.rotation = q;
                //print("slash indicator angle is " + angle);
                //LookAheadTransform.position = transform.position + new Vector3(Mathf.Clamp(lookaheadDir.x, -2, 2), Mathf.Clamp(lookaheadDir.y, -2, 2));

            }
            else if (InputLook.magnitude > .1)
            {//if the analog is being moved
                lookaheadDir = InputLook * 2;
                print("input look mag more than .2");
                LookAheadTransform.position = transform.position + (Vector3)lookaheadDir;
                //InputLook = Vector2.zero;

                float angle = Mathf.Atan2(lookaheadDir.y, lookaheadDir.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.Euler(0f, 0f, angle - 90);
                SlashIndicator.transform.rotation = q;
                print("slash indicator angle is " + angle);


            }
            //else if (move.magnitude > .1f && InputLook.magnitude < .3 && !OnMouseControl)
            //{
            //    //InputLook = Vector2.zero;
            //    //lookaheadDir = new Vector2(Mathf.Round(move.x), Mathf.Round(move.y));
            //    lookaheadDir = move  * 2;
            //    inventory.PointDir = lookaheadDir;

            //    //print("look ahead dir while movings is " + move);
            //    //lookaheadDir = (transform.position + move) * 2;
            //    //float t = (int)(((LookAheadSpeed * Time.deltaTime) / (1/32)) + 0.5f) * (1/32);

            //    //LookAheadTransform.position = Vector3.Lerp(LookAheadTransform.position, lookaheadDir, LookAheadSpeed);


            //    //only do this if mouse looking bool is set to false
            //    LookAheadTransform.position = Vector3.Slerp(LookAheadTransform.transform.position, transform.position + (Vector3)lookaheadDir, .3f);
            //    float angle = Mathf.Atan2(lookaheadDir.y, lookaheadDir.x) * Mathf.Rad2Deg;
            //    Quaternion q = Quaternion.Euler(0f, 0f, angle - 90);
            //    SlashIndicator.transform.rotation = q;
            //    //print("slash indicator angle is " + angle);



            //}





            if (lookaheadDir.magnitude > .1)
            {
                playeranim.SetFloat("LastDirX", lookaheadDir.x * 10);
                playeranim.SetFloat("LastDirY", lookaheadDir.y * 10);
            }
            else
            {
                if (move.magnitude > .1)
                {
                    playeranim.SetFloat("LastDirX", move.x * 10);
                    playeranim.SetFloat("LastDirY", move.y * 10);
                }
            }

        }


        //points a line in direction of trajectory
        public void AimThrow()
        {
            if (AimLine.enabled == false)
            {
                AimLine.enabled = true;
            }
            AimLine.SetPosition(0, transform.position);
            AimLine.SetPosition(1, LookAheadTransform.position);
            aimfloat = Time.time * AimdotSpeed;
            AimMat.SetTextureOffset("_BaseMap", new Vector2(-aimfloat, 0));

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

            float shakedir = InputMove.x;

            if (shakeright)
            {
                if (shakedir > 0)
                {
                    shake++;
                    Debug.Log("shook right");
                    shakeright = false;
                }
            }
            else
            {
                if (shakedir < 0)
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

        public void TakeDamage(Vector3 damagepos)
        {
            if (canbeDamaged)
            {
                //DisableHurtBox();
                //Hurtbox.enabled = false;
                //Debug.Log("hurtbox game obj is " + Hurtbox.gameObject, Hurtbox.gameObject);
                //hurtboxobj.GetComponent<BoxCollider2D>().enabled = false;

                canbeDamaged = false;
                print("player was attacked");
                Vector3 damagedir = damagepos - transform.position;
                playeranim.SetTrigger("Hurt");
                print("current health is " + PlayerHealth);
                rb2d.AddForce(damagedir.normalized * Knockbackdist);
                PlayerHealth -= 50;
                print("health after damage is " + PlayerHealth);
                healthUI.ChangePlayerHealth(PlayerHealth);
                if (PlayerHealth <= 0)
                {
                    GameOverController.instance.GameOverScreen();
                    playeranim.SetTrigger("dead");
                }
                StartCoroutine(RemoveInvincibility());
            }
            //healthUI.CutHeart(false);
        }

        IEnumerator RemoveInvincibility()
        {
            yield return new WaitForSeconds(1.5f);
            canbeDamaged = true;
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

            Capcol.enabled = false;
        }

        public void EnableCollider()
        {
            Capcol.enabled = true;
            print("enabling hurtbox");
        }



        public void DisableHurtBox()
        {
            Hurtbox.enabled = false;
            print("disabling hurtbox");

        }


        public void EnableHurtBox()
        {
            Hurtbox.enabled = true;
            print("enabling hurtbox of player");
        }

        //for punching
        public void InchForward()
        {
            AnimatorClipInfo[] m_CurrentClipInfo;
            m_CurrentClipInfo = playeranim.GetCurrentAnimatorClipInfo(0);
            string m_ClipName = m_CurrentClipInfo[0].clip.name;
            Debug.Log("clib length is " + m_CurrentClipInfo[0].clip.length);
            if (m_ClipName == "BackATK")
            {
                transform.position += new Vector3(0, .3f, 0);
            }
        }


        //seperating code to use in SetDialogue() and SetTransition()
        //public void SetWait()
        //{
        //    if (!InDialogue)
        //    {
        //        canmove = false;
        //        move = Vector2.zero;
        //        InDialogue = true;
        //        _inputActions.Player.Disable();
        //        _inputActions.UI.Enable();
        //        print("Setting player to wait");
        //        //only change view direction
        //        //playeranim.SetTrigger("Wait");
        //    }
        //    else
        //    {
        //        canmove = true;
        //        InDialogue = false;
        //        _inputActions.Player.Enable();
        //        print("Setting player to wait");
        //        //only change view direction
        //        //playeranim.SetTrigger("Wait");
        //    }
        //}

        
        public void SetUIControls(bool EnableUIControls)
        {

            if (EnableUIControls)
            {
                print("enabling Ui controls");
                _inputActions.Player.Disable();
                _inputActions.UI.Enable();
                //dialogue will advance with the event system
            }
            else
            {
                print("enabling player controls");
                _inputActions.Player.Enable();
                _inputActions.UI.Disable();
            }

        }


        public void EnableReadyPhase(bool Enable)
        {
            if(Enable)
            {
                SetUIControls(true);
                AcceptAction = WalkIntoLevel;
            }
        }


        //first places player in a door of room 
        public void SetPosAndWalkDir(Vector2 pos, Vector2 Dir)
        {
            //canmove = false;
            WalkIntoLevelDir = Dir;
            //DisablePlayerCol();
            //SequenceDone = false;
            transform.position = pos;
            //TransitioningToLevel = true;
            move = Vector2.zero;
            setVelocity(true);
            print("Setting walk dir");
            //playeranim.SetTrigger("Wait");
            //StartCoroutine(StartWalking(Dir));
        }


        public float GetAngleToMouse()
        {

            Vector3 relative = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(InputMousePos));
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
            //lookaheadDir = mousepos - (Vector2)transform.position;
            //lookaheadDir = Vector3.Normalize(lookaheadDir);
            //print("direction to mouse is " + relative);
            //print("mouse being moved mouse dif is " + lookaheadDir);
            //LookAheadTransform.position = transform.position + new Vector3(Mathf.Clamp(lookaheadDir.x, -4, 4), Mathf.Clamp(lookaheadDir.y, -4, 4)) / 2f;
            //LookAheadTransform.position = Vector2.Lerp(LookAheadTransform.position, InputMousePos, .5f);

            //Vector2 AimDir = LookAheadTransform.position - transform.position;
            //float ToMouseAngle = Mathf.Atan2(lookaheadDir.y, lookaheadDir.x) * Mathf.Rad2Deg;
            //print("mouse to angle is " + ToMouseAngle);
            return angle;
            //Quaternion q = Quaternion.Euler(0f, 0f, angle - 90);
            //return q;
        }

        //makes the player walk in directon room after button input
        public void WalkIntoLevel()
        {
            playeranim.SetTrigger("RoomEnter");
            CodeMonkey.CMDebug.TextPopup("Entering room", transform.position);
            //Gmanager.instance.LevelStarted = true;
            print("Level started set to true");
            //DisablePlayerCol();
            move = Vector3.zero;
            AcceptAction -= WalkIntoLevel;
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
            print("player started walking coroutine started. level transition set to true");
            //Gmanager.instance.LevelTransition = true;
            print("started walking co routine towards dir" + Dir);

            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && SequenceDone);
            //UITextNotificationController.instance.ShowNotification(false);
            float secondstoReach = 1.5f;
            TransitioningToLevel = true;
            while (secondstoReach > 0)
            {
                InputMove = Dir.normalized;
                secondstoReach -= Time.deltaTime;
                
                yield return null;
            }
            move = Vector3.zero;
            EnableCollider();
            TransitioningToLevel = false;

            GetComponent<SpriteRenderer>().enabled = true;
            EnableCollider();
            print("finished walking co routine");
        }





        public void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + dashvector);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
        }


        public void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.IsTouching(Parrybox))
            {
                if (collision.gameObject.layer == 15)
                {
                    Debug.Log("enemy attack parried, from :" + collision.gameObject, collision.gameObject);
                    Instantiate(sparkanimation, Parrybox.ClosestPoint(collision.transform.position), Quaternion.identity);
                    IparryInterface parryenemy = collision.GetComponentInParent<IparryInterface>();
                    if (parryenemy != null)
                    {
                        parryenemy.Stunned();
                        HitEffectManager.instance.TimeStop();
                    }
                    else
                    {
                        print("no enemy to parry");
                    }

                    //for tutorial
                    ParryAction?.Invoke();
                }
            }


            //print("enemy touched");
            if (collision.IsTouchingLayers(1 << 12))
            {
                print("enemy is touching layer " + hitboxlayer);
                //playeranim.SetTrigger("Hurt");
                Debug.Log("attack hit" + collision.name, collision.gameObject);

                collision.GetComponent<Ikillable>().Die(transform.position);

            }

           


        }

    }

}