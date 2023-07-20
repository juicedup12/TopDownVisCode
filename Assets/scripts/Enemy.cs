using System.Collections;
using UnityEngine;
using UnityEditor;

namespace topdown
{
    //change to IUseItem later
    public class Enemy : MonoBehaviour, IUseItem, Ikillable
    {
        public Transform WeaponTransform;
        public LayerMask wallcheck;
        public delegate void death(Transform enemy);
        public event death OnDeath;
        CopyRequestManager requestManager;
        public Vector3 target;
        public float speed = .3f;
        public float offset = 2;
        public float offsetSavedValue;
        public Vector3[] path;
        int targetIndex;
        public float timer = .1f;
        public float viewFov;
        public float viewDistance;
        protected Vector3 dir;
        public bool chasingplayer;
        public LayerMask playermask;
        public GameObject torso;
        public GameObject BloodParticle;
        Vector2 gridpos;
        public GameObject BloodEffect;
        public Material glowmaterial;

        public Vector2[] Patrolpoints;
        public Animator anim;
        public Transform playertransform;
        public player player;
        public Transform feet;
        public float damping = 1.2f;
        public Vector3 addAngle;
        public bool pathcreated = false;
        public Vector3 OrientTo;
        //[HideInInspector]
        public bool CantMove;
        public int AtWaypoint = 0;
        public int AtTarget = 1;
        public Transform spawnpoint;
        float Pathfindwait = .2f;
        public GameObject coin;
        SpriteRenderer sprrend;
        public Vector3 targetlastpos;
        bool Reenabled = false;
        protected bool dead = false;
        public bool IsDead { get { return dead; } }
        AnimatorTimeController animatorTime;
        [SerializeField] LayerMask wallmask;


        protected virtual void Awake()
        { 
            anim = GetComponent<Animator>();
            animatorTime = GetComponent<AnimatorTimeController>();
            playertransform = GameObject.Find("Player").transform;
            player = playertransform.GetComponent<player>();
        }
        

        protected virtual void Start()
        {
            SceneLinkedSMB<Enemy>.Initialise(anim, this);
            offsetSavedValue = offset;
            StartCoroutine(setgridpos());
            requestManager = GetComponentInParent<CopyRequestManager>();
            sprrend = GetComponent<SpriteRenderer>();
        }

        protected virtual void Update()
        {

            if (!CantMove)
            {
                //if (Input.GetKeyDown(KeyCode.Mouse1))
                //{
                //    Debug.Log("distance is " + Vector3.Distance(transform.position, playertransform.position));
                //}



                if (timer > 0)
                {
                    //Debug.Log(timer);
                    timer -= Time.deltaTime;
                }
                else
                {
                    Pathfindwait = .2f;
                    //Debug.Log("timer done");
                    if (target != null && targetlastpos != target)
                    {
                        if (requestManager != null)
                        {
                            Vector2 Startpos = (Vector2)transform.position;
                            Vector2 Endpos = (Vector2)target;
                            print("requesting path from " + Startpos + " to " + Endpos);
                            requestManager.RequestPath(Startpos, Endpos, OnPathFound, offset);

                            targetlastpos = target;
                            timer = Pathfindwait;
                        }
                        else
                        {
                            Debug.Log("no request manager");
                        }

                    }

                    //flip sprite if dot product is behind flip sprite
                    
                    //Vector3 forward = transform.TransformDirection(Vector3.right);
                    Vector3 toOther = target - transform.position;
                    float TargetDot = Vector3.Dot(transform.right * Mathf.Sign(transform.localScale.x), toOther.normalized);
                    if (TargetDot < 0)
                    {
                        //print("The other transform is behind me!");
                        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
                        WeaponTransform.localScale = WeaponTransform.localScale * -1; 
                    }
                    
                    //print("enemy dot product to dir is " + Vector3.Dot(transform.right, toOther.normalized));
                }
                facetarget();
            }
            
        }

        private void OnEnable()
        {
            SceneLinkedSMB<Enemy>.Initialise(anim, this);

            if (Reenabled)
                StartCoroutine("FollowPath");
            Reenabled = false;
        }

        private void OnDisable()
        {
            StopCoroutine("FollowPath");
            Reenabled = true;
            chasingplayer = false;
        }


        public void ChoosePatrolPoints()
        {
                float PatrolDirChance = Random.value;
                Patrolpoints = PatrolDirChance < .5 ? setPatrolPoints(transform.position, Vector2.right) : setPatrolPoints(transform.position, Vector2.up);
                //patrolPointDist = Vector2.Distance(Patrolpoints[0], Patrolpoints[1]);


            Debug.Log("Patrol points are " + Patrolpoints[0] + " " + Patrolpoints[1]);
            CantMove = false;
        }

        Vector2[] setPatrolPoints(Vector2 Point, Vector2 Direction)
        {
            Vector2[] patrolpoints = new Vector2[2];
            RaycastHit2D FirstHit;
            RaycastHit2D Second;

            FirstHit = Physics2D.Raycast(Point, Direction, 20, wallmask);
            print("getting first raycast from " + Point + " and " + (Point + Direction * 20));
            if (FirstHit.collider != null)
                Debug.Log("First hit " + FirstHit.collider.name + " patrol point 0 is " + FirstHit.point, FirstHit.collider.gameObject);
            else
                Debug.Log("first patrol raycast didn't hit anything");
            //might have to use transform.position.y + Vector3.up * gridworldsize.y / 2
            Second = Physics2D.Raycast(Point, Direction * -1, 20, wallmask);
            Debug.Log("getting Second raycass starting from" + Point + " and " + (Point + Direction * -1 * 20) + " bitmask is " + LayerMask.NameToLayer("wall"));
            Debug.Log(Second.collider == null ? "second raycast hit nothing" : "right raycast hit " + Second.collider.name + " patrol point 1 is " + Second.point);
            Debug.DrawRay(Point, Direction * 20, Color.green, 20);
            Debug.DrawRay(Point, Direction * -1 * 20, Color.red, 20);



            //float PointPadding = .25f;
            //patrolpoints[0] = FirstHit.collider == null ? new Vector2(Point.x, transform.position.y + (gridworldsize.y - .3f) / 2) : FirstHit.point + new Vector2(PointPadding, 0);
            patrolpoints[0] = FirstHit.point;
            //patrolpoints[1] = Second.collider == null ? new Vector2(Point.x, transform.position.y - (gridworldsize.y + .3f) / 2) : Second.point + new Vector2(-PointPadding, 0);
            patrolpoints[1] =  Second.point ;
            return patrolpoints;
        }

        public void UsePotion(int amount)
        {
            Debug.Log("used potion on " + gameObject.name, gameObject);
        }


        public void UsePoison(int dmg)
        {

        }

        public virtual void Die(Vector2 HitPos)
        {
            if (!dead)
            {
                HitEffectManager.instance.TimeStop();
                anim.SetTrigger("die");
                print("enemy was killed");
                WeaponTransform.gameObject.SetActive(false);
                DropMoney();
                Instantiate(BloodEffect, transform.position, Quaternion.identity);
                if (Combo.instance != null)
                {
                    Combo.instance.ComboStart();
                }
                CantMove = true;

                OnDeath?.Invoke(transform);
                StopAllCoroutines();
                //asign blood glow material
                sprrend.material = glowmaterial;

                //Destroy(gameObject);
                dead = true;
                Instantiate(BloodParticle, transform.position, Quaternion.identity);
            }
        }

        public void dieSlice()
        {
            anim.SetTrigger("slicedie");
            CantMove = true;
            OnDeath?.Invoke(transform);
            Instantiate(torso, transform.position, Quaternion.identity);
            StopAllCoroutines();
        }

        public void DropMoney()
        {
            Instantiate(coin, transform.position, Quaternion.identity);
            //Gmanager.instance.MoneyToPlayer();
        }


        //sets player as the target and orientation
        public void targetplayer()
        {
            if (playertransform == null)
                return;
            if (target != playertransform.position)
                target = playertransform.position;
            OrientTo = target;
        }

        public void searchforPlayer()
        {
            //if (!Gmanager.instance.LevelStarted)
            //{
            //    //print("level not started");
            //    return;
            //}

            if (playertransform == null)
            {
                print("player not found");
                return;
            }
            dir = playertransform.position - transform.position;
            //if player is too far from enemy return
            if (dir.sqrMagnitude < viewDistance * viewDistance)
            {
                float angle = Vector3.Angle(WeaponTransform.right, dir);

                if (angle < viewFov * 0.5f && !chasingplayer)
                {
                    // Cast a ray tor direction of player
                    RaycastHit2D hit =  Physics2D.Raycast(transform.position, dir.normalized, distance: dir.magnitude, wallcheck);

                    // If there is no wall between player and enemy
                    if (hit.collider == null)
                    {
                        //Debug.Log("player hit");
                            chasingplayer = true;
                            anim.SetTrigger("chasing");
                            timer = 0;
                            target = playertransform.position;
                    }
                    //else
                    //{
                    //    Debug.Log(hit.transform.name + "between player and enemy");
                    //}
                }
            }
        }

        public void orientToWaypoint()
        {
            if (AtWaypoint > path.Length)
            {
                return;
            }

            if (path.Length > 0)
            {
                if (AtWaypoint == 0)
                {
                    OrientTo = path[path.Length -1];
                }

                //if (nearwaypoint(AtWaypoint))
                //{

                //    AtWaypoint++;
                //    if (AtWaypoint > path.Length - 1)
                //    {
                //        return;
                //    }
                //    OrientTo = path[AtWaypoint];
                //}
            }
        }

        public void SetSpawnState(bool IsEnabled)
        {
            if (IsEnabled)
            {
                print(gameObject + " getting animator time controller");
                //if (!animatorTime) animatorTime = GetComponent<AnimatorTimeController>();
                //animatorTime.enabled = IsEnabled;
                anim.Play("EnemySpawn");
            }
            else
            {
                print(gameObject + " disabling animator time controller");
                anim.SetTrigger("patrol");

            }
        }

        public void SpawnAnimation(float time)
        {
            //if (animatorTime)
            //    animatorTime.SetTime(time);
            //else print("No animator time controller on " + gameObject);
            anim.SetFloat("SpawnTime", time);
                                    
        }


        //looks at the player
        public void OrientToPlayer()
        {
            if (playertransform == null)
                return;
            OrientTo = playertransform.position;
        }

        //updates which patrol point the unit is going to
        public void patrolupdate()
        {
            if (Patrolpoints == null || Patrolpoints.Length < 1)
                return;

            if (AtTarget == 1)
            {
                target = Patrolpoints[0];
            }


            //if near patrol point, then iterate point to target
            if (nearpatrolpoint(AtTarget -1))
            {
                //when reached a patrol point resets orientation to starting path
                AtWaypoint = 0;
                orientToWaypoint();

                //resets patrol target if it goes over
                AtTarget++;
                if (AtTarget > Patrolpoints.Length)
                {
                    AtTarget = 1;
                }
            }

            if ((Vector2)target != Patrolpoints[AtTarget - 1])
            {
                target = Patrolpoints[AtTarget - 1];
            }
        }
        
        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                AtWaypoint = 0;
                path = newPath;
                targetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            if (path.Length > 0)
            {
                Vector3 currentWaypoint = path[0];
                while (true)
                {
                    if (transform.position == currentWaypoint)
                    {
                        targetIndex++;
                        if (targetIndex >= path.Length)
                        {
                            targetIndex = 0;
                            path = new Vector3[0];

                            yield break;
                        }
                        currentWaypoint = path[targetIndex];
                        pathcreated = true;
                    }

                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    //facetarget(currentWaypoint);
                    //Rotate Towards Next Waypoint
                    //Vector3 targetDir = currentWaypoint - this.transform.position;
                    //float step = this.damping * Time.deltaTime;
                    //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                    //transform.rotation = Quaternion.LookRotation(newDir);
                    yield return null;

                }
            }
        }

        public void facetarget()
        {
            Vector3 dir = (OrientTo + Vector3.up * .3f) - WeaponTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            WeaponTransform.rotation = Quaternion.Slerp(WeaponTransform.rotation, q, damping);
            //feet.transform.rotation = Quaternion.AngleAxis(angle, feet.transform.forward);



        }

        public bool isFacingplayer()
        {
            if (playertransform == null)
                return false;
            Vector3 dir =   playertransform.position - WeaponTransform.position;
            float dot = Vector3.Dot(WeaponTransform.right, dir);

            if (dot > .7)
            {
                //print("dot is greater than .5");
                return true;
            }

            //Debug.Log("dot of gun facing player is " + dot);
            return false;
        }
        
        IEnumerator setgridpos()
        {
            yield return new WaitForEndOfFrame();
            gridpos = transform.parent.position;

            //Debug.Log("parent pos is" + gridpos);
        }

        public bool AtDistance(float dist)
        {
            float distance = Vector3.Distance(transform.position, OrientTo);

            if (isFacingplayer() && distance < dist)
            {
                return true;
            }

            return false;
        }

        public float OnlyDistance()
        {
            if (playertransform == null)
                return 0;
            float distance = Vector3.Distance(transform.position, playertransform.position);

            return distance;
        }



        public void GoToSound(Transform soundpos)
        {
            if (!chasingplayer)
            {
                target = soundpos.position;
                anim.SetTrigger("soundcheck");
            }
        }

        public void ResetUnit()
        {
            offset = 0;
            anim.SetTrigger("patrol");
            CantMove = false;
            AtTarget = 1;
            AtWaypoint = 0;
            StopAllCoroutines();
            StopCoroutine("FollowPath");
            transform.position = spawnpoint.position;
            targetlastpos = Vector3.zero;
            timer = .2f;
            target = Patrolpoints[0];
            chasingplayer = false;
            //CopyRequestManager.RequestPath(transform.position, Patrolpoints[0].position, OnPathFound, offset);


        }

        public virtual void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one / 7);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }

        //for checking patrol object distance
        public bool nearpatrolpoint(int patrolpoint)
        {
            if(Patrolpoints[patrolpoint] == null)
            {

                Debug.Log("patrol point is null", gameObject);
                return false;
            }
            if (path.Length > 0)
            {
                float dist = Vector3.Distance(Patrolpoints[patrolpoint], transform.position);

                if (dist < 1.2f)
                {

                    Debug.Log("reached patrol point " + patrolpoint, gameObject);
                    return true;
                }
            }


            return false;
        }


        bool nearwaypoint(int waypoint)
        {

            if (AtWaypoint > path.Length)
            {
                return false;
            }



            if (path.Length > 0)
            {
                if (AtWaypoint > path.Length - 1)
                {
                    return false;
                }
                float dist = Vector3.Distance(path[path.Length -1], transform.position);
                if (dist < .2)
                {

                    print("near waypoint " + path[path.Length -1]);
                    return true;
                }
            }

            return false;
        }


        public bool nearTarget()
        {
            if (path.Length > 1)
            {
                float dist = Vector3.Distance(path[path.Length - 1], transform.position);
                if (dist < .2)
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            //draw the cone of view
            Vector3 forward = WeaponTransform.right;


            Vector3 endpoint = WeaponTransform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

            Handles.color = new Color(0, 1.0f, 0, 0.2f);
            Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

            //Draw attack range
            //Handles.color = new Color(1.0f, 0, 0, 0.1f);
            //Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
        }
#endif
    }


    public interface Ikillable
    {

        void Die(Vector2 HitPos);
    }
}

