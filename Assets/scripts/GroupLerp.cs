using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupLerp : MonoBehaviour
{
    [SerializeField]
    Transform[] objects;
    [SerializeField] float LerpOffset, LerpDur;
    public void LerpObjeccts(float duration, float time)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            float startTime = i * (duration / objects.Length);
            if (time > startTime)
            {
                float localTime = time - startTime;
                objects[i].localScale = Vector2.Lerp(Vector2.one, Vector2.one * 2, localTime / (duration / objects.Length));
            }
        }
    }

    public void LerpObjecctsWithOffset(float duration, float time)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            float offset = (duration - LerpDur) / (objects.Length - 1) * .60f;
            float startTime = offset * i;
            if (time > startTime/.60)
            {
                float localTime = time - startTime/.60f;
                objects[i].localScale = Vector2.Lerp(Vector2.one, Vector2.one * 2, localTime / LerpDur);
            }
        }
    }

    private void Start()
    {
        float offset = (5 - 1.5f) / (6 - 1) * .60f;
        print("offset is " + offset);
        print("last start: " + offset * 5);
    }

    //what is the offset 
    //6 total objects, 5 second timer, 3 second duration
    ////object 0: starts at 00:00
    ///object 1 starts at 0:24
    ///object 2 starts at 00:48
    ///object 3 starts at 01:12
    ///object 4 starts at 01:36
    ///object 5 starts at 02:00
    //5-3 = 2 = 2/5 = .5 * 60 = 24 miliseconds each offset


    //what is the offset 
    //6 total objects, 5 second timer, 1.5 second duration
    ////object 0: starts at 00:00
    ///object 1 starts at 0:42
    ///object 2 starts at 01:24
    ///object 3 starts at 02:06
    ///object 4 starts at 02:48
    ///object 5 starts at 03:30
    //5-1.5 = 3.5 = 3.5/5 = .7 * 60 = 42 miliseconds each offset
    //need to convert solution to seconds instead of decimal
    //42*2 mod it by 1 and convert the solution
    //solution time 1/60
}
