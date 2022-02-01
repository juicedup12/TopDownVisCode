using UnityEngine;

public class patrol : MonoBehaviour
{

    public Transform[] Patrolpoints;
    public pathfinding pathfinding;
    public int AtTarget = 1;
    public float distancetopoint;
    public bool ischasingplayer = false;
    public bool isgoingtosound = false;
    enemypathfind enemypathfind;
    public bool timeron = false;
    public float waittime = 2;
    public float timer;

    void Start()
    {
        //pathfinding.target = Patrolpoints[0];
        enemypathfind = GetComponent<enemypathfind>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!ischasingplayer && !isgoingtosound)
        {
            //after arriving at first node point increment 
            if (nearobj(Patrolpoints[AtTarget - 1]))
            {

                AtTarget++;
                if (AtTarget > Patrolpoints.Length)
                {
                    AtTarget = 1;

                }
                //pathfinding.target = Patrolpoints[AtTarget - 1];

            }
            distancetopoint = Vector3.Distance(Patrolpoints[AtTarget - 1].position, transform.position);
        }




        //if the enemy is going to sound check if it's near target from pathfinding
        //after arriving at target wait for a certain amount of time and turn going to sound false
        //if (isgoingtosound && nearobj(pathfinding.target) && timeron == false)
        //{
        //    Debug.Log("is near sound");
        //    //wait a few seconds
        //    //go back to patrol
        //    timer = waittime + Time.time;
        //    enemypathfind.isidle = true;
        //    timeron = true;
        //}

        if(timer <= Time.time && timer != 0 && timeron)
        {
            Debug.Log("stopped looking for sound");
            enemypathfind.isidle = false;
            ischasingplayer = false;
            isgoingtosound = false;
            //pathfinding.target = Patrolpoints[AtTarget - 1];
            timeron = false;
        }



    }


    public bool nearobj( Transform obj)
    {
        float dist = Vector3.Distance( obj.position , transform.position);

        if (dist < .2)
            return true;


        return false;
    }
}
