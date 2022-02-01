using UnityEngine;

public class Trail : MonoBehaviour
{
    LineRenderer TrailLine;
    public Transform[] TrailPoints;
    public int TrailLength;
    public Point StartPoint;
    public Point EndPoint;

    // Start is called before the first frame update
    void Start()
    {
        TrailLine = GetComponent<LineRenderer>();
        TrailLength = transform.childCount + 2;
        TrailPoints = new Transform[transform.childCount + 2];
        TrailPoints[0] = StartPoint.transform;
        print("start point is " + StartPoint.transform.position);
        for (int i = 1; i < transform.childCount +1; i++)
        {
            TrailPoints[i] = transform.GetChild(i -1);
        }
        TrailPoints[TrailLength - 1] = EndPoint.transform;
        print("endpoint is " + EndPoint.transform.position);
        AsignTrailPoints();

    }


    void AsignTrailPoints()
    {

        for (int i = 0; i < TrailPoints.Length; i++)
        {
            TrailLine.SetPosition(i, TrailPoints[i].position);
            print("linepoint " + i + "assigning to position " + TrailPoints[i].position);
        }
    }

    public Vector3 GetTrailPoint(int trailpoint)
    {
        //print("returning trail point " + (trailpoint - 1) + "from input " + trailpoint);
        return TrailPoints[trailpoint - 1].position;
    }
    
    
}
