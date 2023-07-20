using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace topdown {
    public class ledgeinteract : MonoBehaviour
    {
        player playref;

        public List<Transform> Enemylist;

        int targetindex = 0;

        public Transform currentEnemy;
        public void Start()
        {
            prompt.enabled = false;
        }

        public Image prompt;
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                prompt.enabled = true;
                //enable player to move to ledge
                Debug.Log("ledge collided with " + collision.name);
                playref = collision.GetComponent<player>();
                playref.IsUnderledge = true;
                playref.ledgepos = transform;
                playref.ledgeref = this;
            }
            if(collision.CompareTag("enemy"))
            {
                if (!Enemylist.Contains(collision.transform))
                {
                    Enemylist.Add(collision.transform);

                    currentEnemy = collision.transform;
                    Debug.Log("enemy found");

                    //listen when enemy dies
                    collision.GetComponent<Enemy>().OnDeath += removeEnemy;
                }
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playref = collision.GetComponent<player>();
                prompt.enabled = false;
                playref.IsUnderledge = false;
                playref.ledgepos = null;
                playref = null;
                
            }
        }

        public void removeEnemy(Transform enemy)
        {
            if(enemy != null)
            {
                Enemylist.Remove(enemy);
                if (Enemylist.Count > 0)
                    currentEnemy = Enemylist[0];
            }
        }

        public void switchtarget()
        {
            if (Enemylist.Count > 1)
            {
                targetindex++;
                if (targetindex > Enemylist.Count - 1)
                    targetindex = 0;
                currentEnemy = Enemylist[targetindex];
            }
        }


    }
}
