using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown {
    public class flipevent : MonoBehaviour
    {
        public Transform attackLocation;
        public float attackRange;
        public LayerMask enemies;

        //property for keeping track of atk frame data
        public float atkactive;
        List<GameObject> hitobjects = new List<GameObject>();

        //the amount of time atk fram is active
        float atktime = .1f;
        public bool IsATK;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            atkUpdate();
        }

        void flip()
        {
            transform.localScale = new Vector3(1, transform.localScale.y * -1, 1);
            Debug.Log("called flip");
        }

        void atkUpdate()
        {

            if (atkactive > 0)
            {
                Collider2D damage = Physics2D.OverlapBox(attackLocation.position, Vector2.one/attackRange, layerMask: enemies, angle: transform.rotation.z);
                atkactive -= Time.deltaTime;

                if (damage != null)
                {
                    if (!hitobjects.Contains(damage.gameObject))
                    {
                        hitobjects.Add(damage.gameObject);
                        if (damage.CompareTag("bullet"))
                        {
                            Debug.Log("hit bullet");
                            damage.GetComponent<bullet>().directiontogo *= -1;
                            return;
                        }

                        Debug.Log("hit " + damage.name);

                        damage.GetComponent<Enemy>().die();
                        
                    }

                }
                
            }
            else
            {
                hitobjects.Clear();
                IsATK = false;
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (atkactive >0)
            {

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(attackLocation.position, Vector2.one / attackRange);
            }
        }


        

        void meleeAtk()
        {
            IsATK = true;
            atkactive = atktime;
        }
    }
}
