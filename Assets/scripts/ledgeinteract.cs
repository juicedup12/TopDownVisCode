using System.Collections.Generic;
using UnityEngine;
using System;


namespace topdown
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ledgeinteract : MonoBehaviour
    {
        player playref;
        Action LedgeAction;
        public Action LedgeAttackAction;
        public float searchradius;
        public Collider2D[] enemcols;
        int targetindex = 0;
        public LayerMask walllayer;
        List<Transform> enemies = new List<Transform>();

        public Transform currentEnemy;


        private void Start()
        {
            LedgeAction = () =>
            {

                Debug.Log("player going to ledge " + playref.name);
                InteractUIBehavior.instance.HideUI();
                ShortSceneController.instance.playscene(ShortSceneController.ShortScene.ledge);
            };
        }

        //update enemies array
        public void EnemySearch()
        {
            
            enemcols = Physics2D.OverlapCircleAll(transform.position, searchradius, LayerMask.GetMask("Enemy"));
            //print("layermask is " + LayerMask.GetMask("Enemy"));
            if (enemcols == null || enemcols.Length == 0)
            {
                enemies.Clear();
                InteractUIBehavior.instance.HideUI();
                currentEnemy = null;
                //print("no contacts");
            }
            else
            {

                //print("contacts within radius of ledge, count " + enemcols.Length);
                foreach (Collider2D enemy in enemcols)
                {

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        //remove enemies not within the radius
                        if(!enemies.Contains(enemy.transform))
                        {
                            enemies.Remove(enemy.transform);
                        }
                    }

                    

                        Debug.DrawLine(transform.position, enemy.transform.position, Color.magenta);
                    if (!enemies.Contains(enemy.transform))
                    {
                        if(CheckEnemyDead(enemy.transform))
                        {
                            return;
                        }
                        Vector3 dirtoEnemy = enemy.transform.position - transform.position;
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirtoEnemy, dirtoEnemy.magnitude, LayerMask.GetMask("wall"));
                        if (hit.collider == null )
                        {
                            print("enemy " + enemy + " is attackable");
                            enemies.Add(enemy.transform);

                            //if there isn't already a current enemy, then assign first one
                            if (currentEnemy == null)
                            {
                                currentEnemy = enemies[0];
                            }
                        }
                        else
                        {
                            print("enemy " + enemy + " is not attackable" + "being blocked by " + hit.collider);
                        }
                    }
                }


            }



            //col.GetContacts(searchcontacts);
            //foreach(Collider2D enemy in searchcontacts)
            //{
            //    //try a raycast to see if there's something blocking sight to enemy
            //}
        }

        bool CheckEnemyDead(Transform enemy)
        {
            Debug.Log("enemy is dead: " + enemy.GetComponent<Enemy>().IsDead, enemy);
            return enemy.GetComponent<Enemy>().IsDead;

        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //prompt.enabled = true;

                Vector3 dirtoplayer =  collision.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirtoplayer,  dirtoplayer.magnitude, LayerMask.GetMask("wall"));
                Debug.DrawRay(transform.position, dirtoplayer, Color.red, 5f);
                print("layermask is " + LayerMask.GetMask("wall"));
                if (hit.collider == null)
                {
                    print("hit collider is null");
                    print("player touching ledge radius");

                    playref = collision.GetComponent<player>();
                    playref.ledgeref = this;
                    InteractUIBehavior.instance.DisplayUI(LedgeAction, transform.position, "To Perch");
                }
                else
                {
                    print("hit collider is " + hit.collider);
                }
                //enable player to move to ledge
            }
            //if (collision.CompareTag("enemy"))
            //{
            //    if (!Enemylist.Contains(collision.transform))
            //    {
            //        Enemylist.Add(collision.transform);

            //        currentEnemy = collision.transform;
            //        Debug.Log("enemy found");

            //        //listen when enemy dies
            //        collision.GetComponent<Enemy>().OnDeath += removeEnemy;
            //    }
            //}
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //playref.ledgeref = null;

                playref = null;
                InteractUIBehavior.instance.HideUI();

            }
        }
        


        //update to use enemies list instead
        public void switchtarget( float Dir)
        {
            if(enemies == null || enemies.Count == 0)
            {
                print("no enemies to target on ledge");
                InteractUIBehavior.instance.HideUI();
                return;
            }

            //update display position
            if(currentEnemy != null)
            InteractUIBehavior.instance.DisplayUI(LedgeAttackAction, currentEnemy.transform.position, "to attack");
            //keep current enemy within array bounds
            if (targetindex > enemcols.Length - 1)
            {
                targetindex = enemies.Count - 1;
            }
            if (targetindex < 0)
            {
                targetindex = 0;
            }
            if(enemies.Count >0)
            currentEnemy = enemies[targetindex];


            //print("switch target dir is " + Dir);

            if (enemies.Count > 1)
            {
                if (Dir > 0.2)
                {
                    print("target index is " + targetindex);

                    targetindex++;
                    if (targetindex > enemies.Count - 1)
                        targetindex = enemies.Count - 1;
                    currentEnemy = enemies[targetindex];
                    Debug.Log("current enemy is " + currentEnemy.transform, currentEnemy.gameObject);

                }
                else if(Dir < -.2)
                {
                    print("target index is " + targetindex);
                    targetindex--;
                    if (targetindex < 0)
                        targetindex = 0;
                    currentEnemy = enemies[targetindex];
                    Debug.Log("current enemy is " + currentEnemy.transform, currentEnemy.gameObject);

                }

                

            }



        }


        public Transform RetrieveCurrentEnemy()
        {
                return currentEnemy ;
            
        }


        

        

    }
}
