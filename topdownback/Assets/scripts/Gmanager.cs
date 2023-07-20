using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace topdown
{
    public class Gmanager : MonoBehaviour
    {
        public GameObject[] npcs;
        public Transform[] spawnpoints;
        public void resetlevel()
        {
            //set npcs to their spawn points and reset their states;
            for (int i = 0; i < npcs.Length; i++)
            {
                
                //reset npc state
                if(npcs[i].CompareTag("enemy"))
                {
                    npcs[i].GetComponent<CopyUnit>().ResetUnit();
                }
                if(npcs[i].CompareTag("Player"))
                {
                    npcs[i].GetComponent<player>().ResetPlayer();
                    npcs[i].transform.position = spawnpoints[i].position;
                }
            }
        }


    }
}
