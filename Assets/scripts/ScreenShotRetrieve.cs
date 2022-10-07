using System.Collections;
using System;
using UnityEngine;

//change class to store a texture 
//slicer class will check if there's a texture
//and if there is it will empty it
public class ScreenShotRetrieve : MonoBehaviour, ISliceTextureRetriever
{

    private Action onCapture;
    Texture2D Screenshot;
    public Action OnCapture { set { onCapture = value; } }

    public void PerformCapture()
    {
        StartCoroutine(Capture());

    }

    public Texture2D RetrieveTex
    {
        get
        {
            return Screenshot;
        }
    }
    IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();
        Screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        onCapture?.Invoke();
    }



}