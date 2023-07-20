using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CodeMonkey;

namespace topdown
{
    public class Enemy : MonoBehaviour
    {

        public LayerMask wallcheck;
        public delegate void death(Transform enemy);
        public event death OnDeath;

        public Transform target;
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

        public Transform[] Patrolpoints;
        public Animator anim;
        public Transform playertransform;
        public Transform feet;
        public float damping = 1.2f;
        public Vector3 addAngle;
        public bool pathcreated = false;
        public Vector3 OrientTo;
        [HideInInspector]
        public  bool dead;
        public int AtWaypoint = 0;
        public int AtTarget = 1;
        public Transform spawnpoint;
        float Pathfindwait = .2f;

        Vector3 targetlastpos;


        protected virtual void Awake()
        { 
        
            anim = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            //CopyRequestManager.RequestPath(transform.position, target.position, OnPathFound, offset);
            SceneLinkedSMB<Enemy>.Initialise(anim, this);
            offsetSavedValue = offset;

        }

        protected virtual void Update()
        {

            if (!dead)
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
                    if (target != null && targetlastpos != target.position)
                    {
                        //if(!checkBetween() && offset != offsetSavedValue)
                        //{
                        //    CMDebug.TextPopup("reseting offset", Vector3.zero);
                        //    offset = offsetSavedValue;
                        //}


                        CopyRequestManager.RequestPath(transform.position, target.position, OnPathFound, offset);
                        targetlastpos = target.position;
                        timer = Pathfindwait;

                    }
                }
                facetarget();
            }
        }


        



        public void die()
        {
            anim.SetTrigger("die");
            dead = true;
            OnDeath?.Invoke(transform);
            StopAllCoroutines();
        }

        public void dieSlice()
        {
            anim.SetTrigger("slicedie");
            dead = true;
            OnDeath?.Invoke(transform);
            Instantiate(torso, transform.position, Quaternion.identity);
            StopAllCoroutines();
        }

        //sets player as the target and orientation
        public void targetplayer()
        {
            if (target != playertransform)
                target = playertransform;
            OrientTo = target.position;
        }

        public void searchforPlayer()
        {
            dir = playertransform.position - transform.position;
            //if player is too far from enemy return
            if (dir.sqrMagnitude < viewDistance * viewDistance)
            {
                float angle = Vector3.Angle(transform.right, dir);

                if (angle < viewFov * 0.5f && !chasingplayer)
                {
                    // Cast a ray tor direction of player
                    RaycastHit2D hit =  Physics2D.Raycast(transform.position, dir, distance: dir.magnitude, wallcheck);

                    // If there is no wall between player and enemy
                    if (hit.collider == null)
                    {
                        //Debug.Log("player hit");
                            chasingplayer = true;
                            anim.SetTrigger("chasing");
                            timer = 0;
                            target = playertransform;
                        
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
                    OrientTo = path[0];
                }

                if (nearwaypoint(AtWaypoint))
                {

                    AtWaypoint++;
                    if (AtWaypoint > path.Length - 1)
                    {
                        return;
                    }
                    OrientTo = path[AtWaypoint];
                }
            }
        }

        //looks at the player
        public void OrientToPlayer()
        {
            OrientTo = playertransform.position;
        }

        //updates which patrol point the unit is going to
        public void patrolupdate()
        {
            if (Patrolpoints.Length < 1) return;
            if (AtTarget == 1)
            {
                target = Patrolpoints[0];
            }

            if (nearpatrolpoint(AtTarget))
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

            if (target != Patrolpoints[AtTarget - 1])
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
            Vector3 dir = OrientTo - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, damping);
            //feet.transform.rotation = Quaternion.AngleAxis(angle, feet.transform.forward);

        }

        public bool isFacingplayer()
        {
            Vector3 dir = playertransform.position - transform.position;
            float dot = Vector3.Dot(transform.right, dir);
            if (dot > .5)
                return true;
            return false;
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
            float distance = Vector3.Distance(transform.position, playertransform.position);

            return distance;
        }



        public void GoToSound(Transform soundpos)
        {
            if (!chasingplayer)
            {
                target = soundpos;
                anim.SetTrigger("soundcheck");
            }
        }

        public void ResetUnit()
        {
            offset = 0;
            anim.SetTrigger("patrol");
            dead = false;
            AtTarget = 1;
            AtWaypoint = 0;
            StopAllCoroutines();
            StopCoroutine("FollowPath");
            transform.position = spawnpoint.position;
            targetlastpos = Vector3.zero;
            timer = .2f;
            target = Patrolpoints[0];
            chasingplayer = false;
            CopyRequestManager.RequestPath(transform.position, Patrolpoints[0].position, OnPathFound, offset);


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
            float dist = Vector3.Distance(Patrolpoints[patrolpoint - 1].position, transform.position);

            if (dist < .2)
                return true;


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
                float dist = Vector3.Distance(path[AtWaypoint], transform.position);
                if (dist < .2)
                    return true;
            }

            return false;
        }


        public bool nearTarget()
        {
            float dist = Vector3.Distance(target.position, transform.position);
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
            Vector3 forward = transform.right;


            Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

            Handles.color = new Color(0, 1.0f, 0, 0.2f);
            Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

            //Draw attack range
            //Handles.color = new Color(1.0f, 0, 0, 0.1f);
            //Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
        }
#endif
    }
}

