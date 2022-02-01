using System.Collections;
using UnityEngine;

public class ShortSceneController : MonoBehaviour
{
    //holds references to UI elements and plays their animations on calls

    //will stop time while scene plays, resumes when 

    //will respond to player input and stop playing scene, resuming game


    public static ShortSceneController instance;

    public Animator ledgeanim;
    AnimatorClipInfo[] CurrentClipInfo;
    float  currentcliplength;

    public enum ShortScene
    {
        ledge, other,
    }


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void playscene(ShortScene sceneToPlay)
    {
        switch (sceneToPlay)
        {
            case ShortScene.ledge:
                Time.timeScale = 0;
                ledgeanim.gameObject.SetActive(true);
                CurrentClipInfo = ledgeanim.GetCurrentAnimatorClipInfo(0);
                currentcliplength = CurrentClipInfo[0].clip.length;
                print("current clip length is " + currentcliplength);
                StartCoroutine(OnUnscaledTime());
                break;
            case ShortScene.other:
                break;
            default:
                break;
        }
    }

    public void StopScene()
    {
        Time.timeScale = 1;
        print("stopping scene");

    }

    IEnumerator OnUnscaledTime()
    {
        yield return new WaitForSecondsRealtime(currentcliplength);
        print("clip length reached");
        ledgeanim.gameObject.SetActive(false);
        StopScene();
    }

}
