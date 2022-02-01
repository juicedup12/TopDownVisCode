using UnityEngine;

namespace topdown
{
    public class Shooter : Enemy
    {
        LineRenderer line;
        public GameObject bullet;
        public Transform bulletspawn;
        public float soundradius;
        public LayerMask enemies;
        Vector3[] linepositions = new Vector3[2];
        public float currentLineWidth;
        public float minLineStart;
        public float minLineEnd;
        public float widthIncreaseRate;
        public float maxLineWidth;


        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            SceneLinkedSMB<Shooter>.Initialise(anim, this);

        }

        protected override void Start()
        {
            base.Start();
            line = GetComponent<LineRenderer>();
            minLineStart = line.startWidth;
            print("minline start is " + minLineStart);

            minLineEnd = line.endWidth;
            print("minline end is  " + minLineEnd);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            Vector3 dir = playertransform.position - transform.position;
            Debug.DrawRay(WeaponTransform.position, dir, Color.green);
            Debug.DrawLine(WeaponTransform.position, WeaponTransform.right * 3, Color.red);
        }

        public void makesound()
        {
            //Debug.Log("make sound");
            GameObject soundpos = new GameObject("sound origin");
            soundpos.layer = 11;
            soundpos.transform.position = transform.position;
            Collider2D enemycol = Physics2D.OverlapCircle(transform.position, soundradius, enemies);
            if (enemycol != null)
            {
                if (enemycol.CompareTag("enemy"))
                {
                    //Debug.Log( enemycol.name + "heard a sound", enemycol.gameObject);
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


        //is true if enemy is facing the player and within distance
        public bool canShoot(float dist)
        {
            float distance = Vector3.Distance(transform.position, OrientTo);
            if (isFacingplayer() && distance < dist)
            {
                
                //anim.SetTrigger("shoot");
                //CMDebug.TextPopupMouse("can shoot");

                return true;
            }
            if(line.enabled)
            resetAimLine();
            return false;
        }

        public void shoot()
        {
            Instantiate(bullet, bulletspawn.position, WeaponTransform.rotation);
            resetAimLine();
            anim.SetTrigger("shoot");
            makesound();
            
        }

        public void StartAiming(float width)
        {

            if (!line.enabled) { line.enabled = true;
                print("enabling line renderer");
            }
            linepositions[0] = bulletspawn.position;
            linepositions[1] = bulletspawn.right * 5;
            line.SetPositions(linepositions);
            //line.widthMultiplier = width; 
        }

        public void resetAimLine()
        {
            print("reseting aimline");
            line.startWidth = minLineStart;
            line.endWidth = minLineEnd;
            currentLineWidth = 0;
            line.enabled = false;
        }

        public void UpdateAimLaser(float distToShoot)
        {
            if(canShoot(distToShoot) )
            {
                StartAiming(currentLineWidth);

                currentLineWidth +=  widthIncreaseRate;
                line.startWidth = currentLineWidth;
                line.endWidth = currentLineWidth * 1.5f;
                if (currentLineWidth >= maxLineWidth)
                {
                    print("calling shoot");
                    shoot();
                }
            }
        }

    }
}
