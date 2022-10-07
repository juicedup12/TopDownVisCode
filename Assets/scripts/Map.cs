using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public Point[] points;
    Point currentPoint;
    public Transform PlayerIcon;
    bool FollowingTrail;
    Dictionary<Vector2Int, Point> PointsInGrid = new Dictionary<Vector2Int, Point>();
    public Grid gridref;
    public GameObject pointPrefab;
    public GameObject trail;

    // Start is called before the first frame update
    void Start()
    {
        currentPoint = points[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!FollowingTrail)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentPoint.RightDirectionTrail != null)
                {
                    StartCoroutine(FollowTrail(currentPoint.RightDirectionTrail));
                }
                else
                {
                    print("no trail on right direction of current point");
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentPoint.LeftDirectionTrail != null)
                {
                    StartCoroutine(FollowTrail(currentPoint.LeftDirectionTrail));
                }
                else
                {
                    print("no trail on left direction of current point");
                }
            }
        }
    }


    IEnumerator FollowTrail(Trail TrailTofollow)
    {
        int PointtoReach;
        int lastpoint;
        int direction;
        Point endpoint;
        if (isNearPointOne(TrailTofollow))
        {
            PointtoReach = 1;
            lastpoint = TrailTofollow.TrailLength;
            endpoint = TrailTofollow.EndPoint;
            direction = 1;
        }
        else
        {
            PointtoReach = TrailTofollow.TrailLength;
            lastpoint = 1;
            endpoint = TrailTofollow.StartPoint;
            direction = -1;
        }

        FollowingTrail = true;
        print("following " + TrailTofollow.ToString());

        print("last point is " + lastpoint);
        while (!IsAtPoint(lastpoint, TrailTofollow))
        {
            if (IsAtPoint(PointtoReach, TrailTofollow))
            {

                print("player reached point " + PointtoReach);
                PointtoReach+= direction;
            }
            else
            {
                PlayerIcon.position =
                Vector3.Lerp(PlayerIcon.position, TrailTofollow.GetTrailPoint(PointtoReach), .3f);
            }
            yield return null;
        }

        currentPoint = endpoint;
        print("follow trail finished");
        FollowingTrail = false;
    }

    private bool isNearPointOne(Trail trailtofollow)
    {
        float PointOneDist = Vector3.Distance(PlayerIcon.position, trailtofollow.GetTrailPoint(1));
        float LastPointDist = Vector3.Distance(PlayerIcon.position, trailtofollow.GetTrailPoint(trailtofollow.TrailLength));

        if(PointOneDist < LastPointDist)
        {
            return true;
        }
        return false;
    }

    bool IsAtPoint(int point, Trail CurrentTrail)
    {
        float distance = Vector3.Distance(PlayerIcon.position, CurrentTrail.GetTrailPoint(point));

        if (distance <= .1)
        {
            return true;
        }
        return false;
    }


    public void createPoint(Vector2Int _point)
    {
        //instantiates points at world positions based on grid's position
        Point instancedpoint = Instantiate(pointPrefab, gridref.GetCellCenterLocal((Vector3Int)_point), Quaternion.identity).GetComponent<Point>();
        instancedpoint.gameObject.name = _point.ToString();
        instancedpoint.currentPointpos = _point;
        print("point is " + _point);
        
        PointsInGrid.Add(_point, instancedpoint);
        if(currentPoint == null)
        {
            currentPoint = instancedpoint;
        }
        ConnectPoints(instancedpoint);
    }

    public void ConnectPoints(Point point1)
    {
        //performs a check in the dictionary to see if there's a there's a point 
        //if they do then create a trail and assign them to both points


        Vector2Int currentpos = point1.currentPointpos;
        print("point " + point1.gameObject + " current pos is" + currentpos);
        //perform a check to the left
        Vector2Int leftcheck = currentpos + Vector2Int.left;
        print("left check is " + leftcheck);
        if(PointsInGrid.ContainsKey(leftcheck))
        {
            print(point1.gameObject + " has left neighbor");
            Point leftpoint = PointsInGrid[leftcheck];
            //Trail lefttrail = Instantiate(trail).GetComponent<Trail>();
            //lefttrail.StartPoint = leftpoint;
            createAndAssignTrail(point1, leftpoint, Vector2Int.left);
            print("connecting " + point1 + " to " + leftpoint);
        }

        Vector2Int rightcheck = currentpos + Vector2Int.right;
        if (PointsInGrid.ContainsKey(rightcheck))
        {
            print(point1.gameObject + " has right neighbor");

            Point rightpoint = PointsInGrid[rightcheck];
            print("right point is " + rightpoint + " at position " + rightpoint.currentPointpos);
            //Trail righttrail = Instantiate(trail).GetComponent<Trail>();
            //righttrail.StartPoint = point1;
            //righttrail.EndPoint = rightpoint;
            //point1.RightDirectionTrail = righttrail;
            //rightpoint.LeftDirectionTrail = righttrail;

            createAndAssignTrail(point1, rightpoint, Vector2Int.right);

            print("connecting " + point1 + " to " + rightcheck);
        }
        
    }

    void createAndAssignTrail(Point point1, Point point2, Vector2Int dir)
    {
        
        Trail newtrail = Instantiate(trail).GetComponent<Trail>();
        newtrail.StartPoint = point1;
        print("trail " + newtrail + " start point is " + point1);
        newtrail.EndPoint = point2;
        print(" trail " + newtrail + "endpoint is " + point2);
        point1.assignTrail(dir, newtrail);
        point2.assignTrail(dir * -1, newtrail);
    }

}
