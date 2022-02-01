using UnityEngine;
using UnityEngine.Rendering;

public class CaptureScreen : MonoBehaviour
{
    public bool grab;
    public Material ColorChangemat;
    public Texture2D tex;

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }
    

    private void OnPostRender()
    {
        if (grab == true)
        {
            print("grabbing");
            tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            print("screen width and height is " + Screen.width + " " + Screen.height);
            //Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
            tex.ReadPixels(new Rect(1, 0, Screen.width, Screen.height), 0, 1, false);
            tex.Apply();
            

            //ColorChangemat.SetTexture("_MainTex", tex);
            grab = false;
            
        }
    }

}
