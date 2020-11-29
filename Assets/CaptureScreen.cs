using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CaptureScreen : MonoBehaviour
{
    public RawImage RightImage;
    public RawImage LeftImage;
    public bool grab;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("screen width is " + Screen.width);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    grab = true;
        //}
    }
    

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
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            //Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            texture.Apply();
            //ScreenSprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
            RightImage.enabled = true;
            RightImage.texture = texture;
            LeftImage.enabled = true;
            LeftImage.texture = texture;
            grab = false;
            
            StartCoroutine(RightImage.GetComponent<MaterialEffect>().BeginEffect());
            StartCoroutine(LeftImage.GetComponent<MaterialEffect>().BeginEffect());
        }
    }

}
