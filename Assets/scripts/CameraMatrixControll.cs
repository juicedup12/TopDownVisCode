using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraMatrixControll : MonoBehaviour
{
    public float TimeToTransition;
    private Matrix4x4 ortho, perspective;
    public float fov = 60f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 50f;
    private float aspect;
    private bool orthoOn;
    private bool Transitioning;
    Camera m_camera;
    public CinemachineVirtualCamera cmCam;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
        aspect = (float)Screen.width / (float)Screen.height;
        ortho = m_camera.projectionMatrix;
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        m_camera = GetComponent<Camera>();
        m_camera.projectionMatrix = ortho;
        orthoOn = true;
    }


    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }

    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration, float ease, bool reverse)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float step;
            if (reverse) step = 1 - Mathf.Pow(1 - (Time.time - startTime) / duration, ease);
            else step = Mathf.Pow((Time.time - startTime) / duration, ease);
            m_camera.projectionMatrix = MatrixLerp(src, dest, step);
            yield return 1;
        }
        m_camera.projectionMatrix = dest;
    }

    void TweenCMcam()
    {
        cmCam.m_Lens.ModeOverride = LensSettings.OverrideModes.Perspective;
        DOTween.To(() => cmCam.m_Lens.FieldOfView, x => cmCam.m_Lens.FieldOfView = x, 1, 3).onComplete = () => { print("complete"); Transitioning = false; } ;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.isPressed && !Transitioning)
        {
            if (orthoOn)
            {
                //StartCoroutine(LerpFromTo(m_camera.projectionMatrix, perspective, TimeToTransition, 8, true));
                TweenCMcam();
                Transitioning = true;
            }
            else
            {

            }
        }
    }
}
