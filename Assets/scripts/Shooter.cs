using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

namespace topdown
{
    public class Shooter : Enemy
    {

        public GameObject bullet;
        public Transform bulletspawn;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            SceneLinkedSMB<Shooter>.Initialise(anim, this);

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            Vector3 dir = playertransform.position - transform.position;
            Debug.DrawRay(GunTransofrm.position, dir, Color.green);
            Debug.DrawLine(GunTransofrm.position, GunTransofrm.right * 3, Color.red);
        }


        public bool canShoot()
        {
            float distance = Vector3.Distance(transform.position, OrientTo);
            if (isFacingplayer() && distance < 2)
            {
                
                anim.SetTrigger("shoot");
                //CMDebug.TextPopupMouse("can shoot");
                return true;
            }

            return false;
        }

        public void shoot()
        {
            Instantiate(bullet, bulletspawn.position, GunTransofrm.rotation);
        }
    }
}
