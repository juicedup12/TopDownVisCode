using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{

    public Parralax[] parralaxObjs;
    Vector2 parralaxeffectMultiplier;

    public void FollowPlayerCam(GameObject cam)
    {
        foreach(Parralax parobj in parralaxObjs)
        {
            parobj.AssignFollow(cam);
            parobj.ParallaxEffectMultiplier *= .2f;
        }
    }

    public void FollowRoomFocusCam(GameObject cam)
    {
        foreach (Parralax parobj in parralaxObjs)
        {
            parobj.AssignFollow(cam);
            parobj.ParallaxEffectMultiplier = parobj.OriginalParMultiplier;
        }
    }


}
