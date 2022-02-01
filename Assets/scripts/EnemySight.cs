using UnityEngine;
using UnityEditor;

namespace topdown
{
    //logic for determining what the enemy character sees
    public class EnemySight : MonoBehaviour
    {
        public Transform[] Patrolpoints;
        public LayerMask layer;
        public LayerMask playerlayer;
        public float viewFov;
        public float viewDistance;
        //bool insight;
        enemypathfind pathfind;
        public Transform player;
        bool chasingplayer = false;
        public patrol patrolref;
        public pathfinding pathcreate;
        public Transform m_Target;
        public Transform bulletspawn;
        public GameObject bullet;
        Vector3 dir;
        Animator anim;
        
        
        public float firerate = 1;
        float timer;


        private void Awake()
        {
            anim = GetComponent<Animator>(); 
        }

        // Start is called before the first frame update
        void Start()
        {
            SceneLinkedSMB<EnemySight>.Initialise(anim, this);
            pathfind = GetComponent<enemypathfind>();

            //pathcreate.target = Patrolpoints[0];
        }

        // Update is called once per frame
        void Update()
        {
            dir = player.transform.position - transform.position;

            //if player is too far from enemy return
            if (dir.sqrMagnitude > viewDistance * viewDistance)
            {
                return;
            }


            float angle = Vector3.Angle(transform.right, dir);

            if (angle < viewFov * 0.5f && !chasingplayer)
            {
                // Cast a ray tor direction of player
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dir.magnitude, layer);

                // If it hits something...
                if (hit.collider != null)
                {
                    return;
                }


                //insight = true;
                //Debug.Log("in view");
                pathfind.isidle = false;
                pathfind.chasingplayer = true;
                //patrolref.ischasingplayer = true;
                //pathcreate.target = player;
                chasingplayer = true;
                pathfind.speed *= 2;
                shoot();
                //run shoot function
            }
            else if (angle < viewFov * 0.5f && chasingplayer)
            {
                shoot();
            }


            

            if (chasingplayer)
            {
                //start shooting at player


                //Vector3 targetdir = pathcreate.target.transform.position - transform.position;
                //float ang = Mathf.Atan2(targetdir.y, targetdir.x) * Mathf.Rad2Deg;
                ////Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                //Quaternion bulletquat = Quaternion.AngleAxis(ang, Vector3.forward);

                //timer = Time.time + firerate;
                //GameObject bulletobj = Instantiate(bullet, bulletspawn.position, transform.rotation);
                //bullet.GetComponent<bullet>().directiontogo = targetdir;

            }



        }



        public bool nearpatrolpoint(int patrolpoint)
        {
            float dist = Vector3.Distance(Patrolpoints[patrolpoint -1].position, transform.position);

            if (dist < .2)
                return true;


            return false;
        }


        public void patrolupdate(int AtTarget)
        {
            //pathcreate.target = Patrolpoints[AtTarget - 1];
        }

        public void shoot()
        {
            RaycastHit2D hit = Physics2D.Raycast(bulletspawn.position, dir, dir.magnitude);
            Debug.DrawRay(transform.position, dir);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    if (Time.time > timer)
                    {
                        Instantiate(bullet, bulletspawn.position, transform.rotation);
                        timer = Time.time + firerate;
                    }
                }
            }
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
