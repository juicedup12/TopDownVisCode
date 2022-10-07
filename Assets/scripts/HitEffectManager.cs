using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HitEffectManager : MonoBehaviour
{

    bool stopping;
    public float stoptime, slowtime;
    public static HitEffectManager instance;
    CinemachineVirtualCamera vcam;

    public void TimeStop()
    {
        if (!stopping)
        {
            stopping = true;
            Time.timeScale = 0;
            StartCoroutine(stop());
        }

    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        StartCoroutine(ShakeRoutine(time));
        TimeStop();
    }

    IEnumerator ShakeRoutine(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;


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
        vcam = (CinemachineVirtualCamera)GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
