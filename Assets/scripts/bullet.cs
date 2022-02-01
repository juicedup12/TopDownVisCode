using UnityEngine;

namespace topdown
{
    public class bullet : MonoBehaviour
    {
        public float speed;
        public Vector3 directiontogo;
        public bool AttackAll;

        // Start is called before the first frame update
        void Start()
        {
            directiontogo = transform.right;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(directiontogo * speed * Time.deltaTime, Space.World);
        }

       

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //print("bullet collided with " + collision.name);

            if (collision.tag == "Hurtbox" && collision.transform.parent.tag == "Player")
            {
                //Debug.Log("collided with player");
                Gmanager.instance._player.TakeDamage(transform.position);
                Destroy(gameObject);
                //collision.transform.GetComponent<player>().death();
            }

            if(AttackAll)
            {
                if (collision.tag == "Enemy")
                {
                    Debug.Log("collided with enemy");
                    //collision.transform.GetComponent<player>().death();
                }
            }

            if (collision.tag == "Wall") 
            {
                //print("hit wall");
                Destroy(gameObject);
            }
        }

    }
}
