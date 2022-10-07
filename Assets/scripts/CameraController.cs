using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public PixelPerfectCamera pixelcam;
    public float StartingPPU, FinalPPU;
    public Cinemachine.CinemachineVirtualCamera vcam;
    public Cinemachine.CinemachineOrbitalTransposer orbit;
    public float Fov { get { return vcam.m_Lens.FieldOfView; } set { vcam.m_Lens.FieldOfView = value; } }

    // Start is called before the first frame update
    void Start()
    {
        AssignOrbit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator LerpPixelDensity(float currentppu, float targetppu)
    {
        print("started pixel lerping");
        float currentdensity = currentppu;
        while (Mathf.Ceil(currentdensity + .05f) < targetppu)
        {
            print("current ppu is " + currentdensity);
            currentdensity = Mathf.Lerp(currentdensity, targetppu, .1f);
            pixelcam.assetsPPU = (int)Mathf.Ceil(currentdensity);
            yield return null;
        }
        pixelcam.assetsPPU = (int)targetppu;
        yield return null;
    }

    //public void FollowRoomFocusCam(GameObject cam)
    //{
    //    pixelcam.assetsPPU = Mathf.RoundToInt(StartingPPU);
    //    print("parallax objs folloowing room focus cam");
    //    foreach (Parralax parobj in parralaxObjs)
    //    {
    //        parobj.ParallaxEffectMultiplier = parobj.OriginalParMultiplier;
    //    }
    //}
    //public void FollowPlayerCam(GameObject cam)
    //{
    //    StartCoroutine(LerpPixelDensity(StartingPPU, FinalPPU));
    //    foreach (Parralax parobj in parralaxObjs)
    //    {
    //        parobj.ParallaxEffectMultiplier *= .2f;
    //    }
    //}

    public void AssignVcamRotation(float f)
    {
        orbit.m_XAxis.Value = f;
    }

    public void assignVcamFov(float f)
    {
        vcam.m_Lens.FieldOfView = f;
    }

    void AssignOrbit()
    {
        orbit = vcam.GetCinemachineComponent<Cinemachine.CinemachineOrbitalTransposer>();
    }

}
