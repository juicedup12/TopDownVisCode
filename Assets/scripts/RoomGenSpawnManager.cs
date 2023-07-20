using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    //holds data about enemies that are in the scene
    //should move this code to a scriptable object
    public class RoomGenSpawnManager : MonoBehaviour
    {

        List<Enemy> enemies = new List<Enemy>();
        int CurrentEnemyIndex = 0;
        public static RoomGenSpawnManager instance;



        //return the enemy at index and increase index
        public Enemy GetEnemy()
        {
            print("getting enemy, index is " + CurrentEnemyIndex + " count is " + enemies.Count);
            if(CurrentEnemyIndex < enemies.Count)
            {
                CurrentEnemyIndex++;
                return enemies[CurrentEnemyIndex -1];

            }
            print(" enemy index over enemy count");
            return null;
        }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetEnemiesToPatrol()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.SetSpawnState(false);
                enemy.ChoosePatrolPoints();

            }
        }

        /// <summary>
        /// Tells spawn manager to create and hold enemy data
        /// </summary>
        /// <param name="ListPos"></param>
        /// The index of the level
        public void CreateEnemy(Vector2 SpawnPos, GameObject EnemyPrefab, Transform room)
        {
            print("Spawn Manager creating an enemy");
            Debug.Log("Enemy tiles spawn point" + SpawnPos);
            Enemy enemy = Instantiate(EnemyPrefab, SpawnPos, Quaternion.identity).GetComponent<Enemy>();
            enemies.Add(enemy);
            enemy.transform.parent = room;
          
            //StartCoroutine(SetEnemyPatrolPoints(enemy.GetComponent<Enemy>(), TileToSpawn.point));


            //SpawnSeq.OnKill( () =>
            //{
            //    print("spawn enemy seq complete");
            //    player.SequenceDone = true;
            //    Gmanager.instance.ReturnToPlayer();
            //});
            //return SpawnSeq;
        }

        public void ResetEnemies()
        {
            enemies = new List<Enemy>();
            CurrentEnemyIndex = 0;
        }

    }
}
