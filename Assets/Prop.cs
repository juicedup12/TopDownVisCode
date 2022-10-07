using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public abstract class Prop : MonoBehaviour, Ikillable
    {
        public Sprite[] pieces;
        protected Vector2 HitPos;
        protected bool Broken = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public abstract void Break();



        private void OnTriggerEnter2D(Collider2D collision)
        {
            print("prop collider was triggered");
            if (collision.CompareTag("Hitbox") && collision.gameObject.layer == 19)
            {
                Break();
            }
        }

        public void Die(Vector2 HitPos)
        {
            this.HitPos = HitPos;
            Break();
        }
    }
}
