using UnityEngine;

public class Fly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float xdir = Random.Range(0f, 1f );
        float ydir = Random.Range(0f, 1f);
        GetComponent<Rigidbody2D>().velocity = new Vector3(xdir, ydir) * 2 ;
        Debug.Log("velocity is " + xdir + " " + ydir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
