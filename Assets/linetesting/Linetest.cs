using System.Collections.Generic;
using UnityEngine;

public class Linetest : MonoBehaviour
{
    LineRenderer lr;
    Vector2 clickpos;
    Vector2 lineend;
    public float angle;
    Vector2 posplus, posneg;
    List<Vector3> points = new List<Vector3>();
    public float linespeed;
    public float anglelerpspeed;
    bool retracting = true;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            retracting = false;
            clickpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineend = transform.position;
            points.Add(transform.position);
            points.Add(lineend);
            lr.positionCount = 2;

            lr.useWorldSpace = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, lineend);
            Debug.Log("click pos is" + clickpos);

            
            posneg = Quaternion.AngleAxis(-angle * 2, Vector3.forward) * clickpos * 1.4f;
            posplus = Quaternion.AngleAxis(angle * 2, Vector3.forward) * (clickpos * 2.3f);
            //add an angle to the rope end
        }

        if(!retracting)
        {
            //Debug.Log("not retracting");
            lineend = Vector3.MoveTowards(lineend, posplus, linespeed * Time.deltaTime);
            posplus = Vector3.MoveTowards(posplus, posneg, anglelerpspeed * Time.deltaTime);
            lr.SetPosition(1, lineend);

            float dist = Vector2.Distance(lineend, posneg);
            if (dist < 1.8)
            {
                Debug.Log("line completed");
                retracting = true;
            }
        }
        else
        {
            if (lr.positionCount > 0)
            {
                lineend = Vector3.Lerp(lineend, transform.position, linespeed * (1.2f * Time.deltaTime));
                lr.SetPosition(1, lineend);
            }
        }


    }



}
