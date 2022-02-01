using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ParallaxManager : MonoBehaviour
{

    public Parralax[] parralaxObjs;
    Vector2 parralaxeffectMultiplier;
    public PixelPerfectCamera pixelcam;
    public float StartingPPU, FinalPPU;

    public void FollowPlayerCam(GameObject cam)
    {
        StartCoroutine(LerpPixelDensity(StartingPPU, FinalPPU));
        foreach (Parralax parobj in parralaxObjs)
        {
            parobj.AssignFollow(cam);
            parobj.ParallaxEffectMultiplier *= .2f;
        }
    }




    public void FollowRoomFocusCam(GameObject cam)
    {
        pixelcam.assetsPPU =  Mathf.RoundToInt( StartingPPU);
        print("parallax objs folloowing room focus cam");
        foreach (Parralax parobj in parralaxObjs)
        {
            parobj.AssignFollow(cam);
            parobj.ParallaxEffectMultiplier = parobj.OriginalParMultiplier;
        }
    }

    IEnumerator LerpPixelDensity(float currentppu, float targetppu)
    {
        print("started pixel lerping");
        float currentdensity = currentppu;
        while (Mathf.Ceil( currentdensity + .05f) < targetppu  )
        {
            print("current ppu is " + currentdensity);
            currentdensity = Mathf.Lerp(currentdensity, targetppu, .1f);
            pixelcam.assetsPPU = (int)Mathf.Ceil(currentdensity);
            yield return null;
        }
        pixelcam.assetsPPU = (int)targetppu;
        yield return null;
    }
        
    


}
