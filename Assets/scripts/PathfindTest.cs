using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindTest : MonoBehaviour
{
    [SerializeField] CopyRequestManager requestManager;
    [SerializeField] Transform Target;
    [SerializeField] float PathCooldown;
    [SerializeField] float speed;
    float timer;
    public Vector3[] path;
    int targetIndex;
    Rigidbody2D rb2d;
    Vector2 velocity;
    [SerializeField] Vector2[] Patrolpoints;
    [SerializeField] LayerMask wallmask;
    int PatrolIndex = 0;
    Vector2 target;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //StartCoroutine(GetPath());
        ChoosePatrolPoints();
    }

    IEnumerator GetPath()
    {
        yield return new WaitForSeconds(.5f);
        print("requesting path");
        requestManager.RequestPath(transform.position, target, OnPathFound,  0);
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //print("path successful");
            //AtWaypoint = 0;
            path = newPath;
            //targetIndex = 0;
            //StopCoroutine("FollowPath");
            //StartCoroutine("FollowPath");
        }
    }

    // Update is called once per frame
    void Update()
    {
        patrolupdate();
        if(timer <= 0)
        {
            timer = PathCooldown;
            print("requestion path to " + target);
            requestManager.RequestPath(transform.position, target, OnPathFound, 0);
        }
        if(path == null || path.Length < 1)
        {
            print("no path");
            return;
        }
        //transform.position = Vector3.MoveTowards(transform.position, path[0], speed * Time.deltaTime);
        velocity = (path[0] - transform.position).normalized;
        timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //rb2d.velocity = velocity * speed;
        rb2d.MovePosition(rb2d.position + velocity * Time.fixedDeltaTime);
        //rb2d.AddForce(velocity * speed);
    }

    public virtual void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one / 7);

                if (i == targetIndex)
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

    public void ChoosePatrolPoints()
    {
        float PatrolDirChance = Random.value;
        Patrolpoints = PatrolDirChance < .5 ? setPatrolPoints(transform.position, Vector2.right) : setPatrolPoints(transform.position, Vector2.up);
        //patrolPointDist = Vector2.Distance(Patrolpoints[0], Patrolpoints[1]);


        Debug.Log("Patrol points are " + Patrolpoints[0] + " " + Patrolpoints[1]);
        target = Patrolpoints[0];
    }

    Vector2[] setPatrolPoints(Vector2 Point, Vector2 Direction)
    {
        Vector2[] patrolpoints = new Vector2[2];
        RaycastHit2D FirstHit;
        RaycastHit2D Second;

        FirstHit = Physics2D.Raycast(Point, Direction, 20, wallmask);
        print("getting first raycast from " + Point + " and " + (Point + Direction * 20));
        if (FirstHit.collider != null)
            Debug.Log("First hit " + FirstHit.collider.name + " patrol point 0 is " + FirstHit.point, FirstHit.collider.gameObject);
        else
            Debug.Log("first patrol raycast didn't hit anything");
        //might have to use transform.position.y + Vector3.up * gridworldsize.y / 2
        Second = Physics2D.Raycast(Point, Direction * -1, 20, wallmask);
        Debug.Log("getting Second raycass starting from" + Point + " and " + (Point + Direction * -1 * 20) + " bitmask is " + LayerMask.NameToLayer("wall"));
        Debug.Log(Second.collider == null ? "second raycast hit nothing" : "right raycast hit " + Second.collider.name + " patrol point 1 is " + Second.point);
        Debug.DrawRay(Point, Direction * 20, Color.green, 20);
        Debug.DrawRay(Point, Direction * -1 * 20, Color.red, 20);



        //float PointPadding = .25f;
        //patrolpoints[0] = FirstHit.collider == null ? new Vector2(Point.x, transform.position.y + (gridworldsize.y - .3f) / 2) : FirstHit.point + new Vector2(PointPadding, 0);
        patrolpoints[0] = FirstHit.point;
        //patrolpoints[1] = Second.collider == null ? new Vector2(Point.x, transform.position.y - (gridworldsize.y + .3f) / 2) : Second.point + new Vector2(-PointPadding, 0);
        patrolpoints[1] = Second.point;
        return patrolpoints;
    }

    public void patrolupdate()
    {
        if (Patrolpoints == null || Patrolpoints.Length < 1)
            return;

        if (PatrolIndex >= Patrolpoints.Length)
        {
            target = Patrolpoints[0];
            PatrolIndex = 0;
        }


        //if near patrol point, then iterate point to target
        if (Vector2.Distance(transform.position, target) < 1f)
        {
            //when reached a patrol point resets orientation to starting path
            //AtWaypoint = 0;
            //orientToWaypoint();

            //resets patrol target if it goes over
            PatrolIndex++;
            if (PatrolIndex >= Patrolpoints.Length)
            {
                target = Patrolpoints[0];
                PatrolIndex = 0;
            }
            target = Patrolpoints[PatrolIndex];
        }

        //if ((Vector2)target != Patrolpoints[AtTarget - 1])
        //{
        //    target = Patrolpoints[AtTarget - 1];
        //}
    }

}
