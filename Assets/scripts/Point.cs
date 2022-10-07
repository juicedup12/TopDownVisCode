using UnityEngine;

public class Point : MonoBehaviour
{

    public Trail RightDirectionTrail;
    public Trail LeftDirectionTrail;
    public Vector2Int currentPointpos;
    bool FollowingTrail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void assignTrail(Vector2Int dir, Trail trailtoassign)
    {
        if (dir == Vector2Int.left)
            LeftDirectionTrail = trailtoassign;
        if (dir == Vector2Int.right)
            RightDirectionTrail = trailtoassign;
    }

}
