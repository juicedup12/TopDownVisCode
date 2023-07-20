using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown {
    public class bullet : MonoBehaviour
    {
        public float speed;
        public Vector3 directiontogo;

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
            if (collision.tag == "Player")
            {
                Debug.Log("collided with player");
                collision.transform.GetComponent<player>().death();
            }
        }

    }
}
