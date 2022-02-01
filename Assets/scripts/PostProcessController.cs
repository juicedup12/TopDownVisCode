using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessController : MonoBehaviour
{
    
    PostProcessData ppdata;
    MotionBlur blur;
    public GameObject Cam;
    public GameObject RenderTex;
    Volume vol;

    private void Start()
    {
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out blur);
        print("blur is: " + blur);
        blur.active = false;
        Cam.SetActive(false);
        RenderTex.SetActive(false);
    }


    public void SetBlurActive(bool IsActive)
    {
        print("Setting blur to " + IsActive);
        blur.active = IsActive;
        Cam.SetActive(IsActive);
        RenderTex.SetActive(IsActive);
    }

}
