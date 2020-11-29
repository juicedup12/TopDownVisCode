using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CodeMonkey;

namespace topdown
{
    public class Enemy : MonoBehaviour
    {
        public Transform GunTransofrm;
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
        Vector3 dir;
        public bool chasingplayer;
        public LayerMask playermask;
        public GameObject torso;
        Vector2 gridpos;
        public GameObject BloodEffect;

        public Vector2[] Patrolpoints;
        public Animator anim;
        public Transform playertransform;
        public Transform feet;
        public float damping = 1.2f;
        public Vector3 addAngle;
        public bool pathcreated = false;
        public Vector3 OrientTo;
        [HideInInspector]
        public  bool CantMove;
        public int AtWaypoint = 0;
        public int AtTarget = 1;
        public Transform spawnpoint;
        float Pathfindwait = .2f;

        public Vector3 targetlastpos;


        protected virtual void Awake()
        { 
            anim = GetComponent<Animator>();
            playertransform = GameObject.Find("Player ").transform;
        }


        protected virtual void Start()
        {
            SceneLinkedSMB<Enemy>.Initialise(anim, this);
            offsetSavedValue = offset;
            StartCoroutine(setgridpos());
            requestManager = GetComponentInParent<CopyRequestManager>();
        }

        protected virtual void Update()
        {

            if (!CantMove)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Debug.Log("distance is " + Vector3.Distance(transform.position, playertransform.position));
                }

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
                        //if(!checkBetween() && offset != offsetSavedValue)
                        //{
                        //    CMDebug.TextPopup("reseting offset", Vector3.zero);
                        //    offset = offsetSavedValue;
                        //}

                        if (requestManager != null)
                        {
                            requestManager.RequestPath((Vector2)transform.position - gridpos,(Vector2)target - gridpos, OnPathFound, offset);
                            targetlastpos = target;
                            timer = Pathfindwait;
                        }
                        else
                        {
                            Debug.Log("no request manager");
                        }

                    }
                }
                facetarget();
            }
        }

        private void OnEnable()
        {
            SceneLinkedSMB<Enemy>.Initialise(anim, this);
        }




        public void die()
        {
            anim.SetTrigger("die");
            Instantiate(BloodEffect, transform.position, Quaternion.identity);
            if(Combo.instance != null)
            {
                Combo.instance.AddToCombo();
            }
            CantMove = true;
            OnDeath?.Invoke(transform);
            StopAllCoroutines();
        }

        public void dieSlice()
        {
            anim.SetTrigger("slicedie");
            CantMove = true;
            OnDeath?.Invoke(transform);
            Instantiate(torso, transform.position, Quaternion.identity);
            StopAllCoroutines();
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
            if (!Gmanager.instance.LevelStarted) return;
            if (playertransform == null)
                return;
            dir = playertransform.position - transform.position;
            //if player is too far from enemy return
            if (dir.sqrMagnitude < viewDistance * viewDistance)
            {
                float angle = Vector3.Angle(GunTransofrm.right, dir);

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
                    else
                    {
                        Debug.Log(hit.transform.name + "between player and enemy");
                    }
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
            Vector3 dir = OrientTo - GunTransofrm.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            GunTransofrm.rotation = Quaternion.Slerp(GunTransofrm.rotation, q, damping);
            //feet.transform.rotation = Quaternion.AngleAxis(angle, feet.transform.forward);

        }

        public bool isFacingplayer()
        {
            if (playertransform == null)
                return false;
            Vector3 dir = playertransform.position - transform.position;
            float dot = Vector3.Dot(GunTransofrm.right, dir);

            if (dot > .5)
            {
                print("dot is greater than .5");
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
            float dist = Vector3.Distance(path[path.Length -1], transform.position);
            if (dist < .2)
            {
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            //draw the cone of view
            Vector3 forward = GunTransofrm.right;


            Vector3 endpoint = GunTransofrm.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

            Handles.color = new Color(0, 1.0f, 0, 0.2f);
            Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

            //Draw attack range
            //Handles.color = new Color(1.0f, 0, 0, 0.1f);
            //Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
        }
#endif
    }
}

