using UnityEngine;


//class will tell map to create points to travel through
public class PointsGrid : MonoBehaviour
{
    public Vector2Int[] Points;
    Map mapref;

    private void Start()
    {
        mapref = GetComponent<Map>();
        foreach(Vector2Int point in Points)
        {
            print("creating point at " + point);
            mapref.createPoint(point);
        }
    }
}
