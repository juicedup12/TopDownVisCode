using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{

    bool stopping;
    public float stoptime, slowtime;
    public static HitStop instance;

    public void TimeStop()
    {
        if (!stopping)
        {
            stopping = true;
            Time.timeScale = 0;
            StartCoroutine(stop());
        }
        
    }


    IEnumerator stop()
    {
        yield return new WaitForSecondsRealtime(stoptime);
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(slowtime);
        Time.timeScale = 1;
        stopping = false;
            
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
