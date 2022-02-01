using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Transform target;
    public float speed = .5f;
    Vector3[] path;
    int targetindex;
    PathRequestManager pathmanager;

    // Start is called before the first frame update
    void Start()
    {
        PathRequestManager.requestpath(transform.position, target.position, Onpathfound);
    }

    

    public void Onpathfound(Vector3[] newpath, bool pathsuccessful)
    {
        if(pathsuccessful)
        {
            path = newpath;
            StopCoroutine(followpath());
            StartCoroutine(followpath());
        }
    }

    IEnumerator followpath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetindex++;
                    if (targetindex >= path.Length)
                    {
                        targetindex = 0;
                        path = new Vector3[0];
                        yield break;
                    }
                    currentWaypoint = path[targetindex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;

            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        //PathRequestManager.instance.pathrequestqueue.Clear();
        PathRequestManager.requestpath(transform.position, target.position, Onpathfound);
        
    }


    public void OnDrawGizmos()
    {

        if(path!= null)
        {
            for (int i = targetindex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one /7);

                if (i == targetindex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
                
            }
        }
    }

}
