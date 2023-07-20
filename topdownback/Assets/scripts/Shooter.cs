using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }


        public bool canShoot()
        {
            float distance = Vector3.Distance(transform.position, OrientTo);
            if (isFacingplayer() && distance < 1)
            {
                return true;
            }

            return false;
        }

        public void shoot()
        {
            Instantiate(bullet, bulletspawn.position, transform.rotation);
        }
    }
}
