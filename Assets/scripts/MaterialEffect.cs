using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MaterialEffect : MonoBehaviour
{

    Material mat;
    RawImage img;
    float opacity = 0;
    public float OpacitySpeed;
    [SerializeField]
    private bool RightSide;
    Vector3 Originalpos;
    Quaternion OriginalRot;
    RectTransform rect;
    public float coroutineTimer = 2;
    Tween CurrentMoveTween, CurrentRotTween;
    public int TweenDur;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        Originalpos = transform.position;
        OriginalRot = transform.rotation;
        img = GetComponent<RawImage>();
        mat =  new Material(img.material);
        img.material = mat;
        mat.SetFloat("_Opacity", 0);
        if(RightSide)
        {
            //print("setting side float to .50");
            mat.SetFloat("_SideFloat", .50f);
        }
        else
        {
            //print("setting side float to .49");
            mat.SetFloat("_SideFloat", .49f);

        }
        //Debug.Log("material is " + mat.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IncreaseOpacity()
    {
        if (opacity < 1)
        {
            opacity += OpacitySpeed * Time.deltaTime;
            mat.SetFloat("_Opacity", opacity);
        }
    }

    private void ResetSelf()
    {
        CurrentMoveTween.Complete();
        CurrentRotTween.Complete();
        print("REesting " + name);
        opacity = 0;
        mat.SetFloat("_Opacity", opacity);
        transform.position = Originalpos;
        rect.rotation = Quaternion.Euler(Vector3.zero);
        coroutineTimer = 2;
        print(name + " rotation is " + rect.rotation);
        img.enabled = false;
    }


    //moves UI elements downwards and changes material properties
    public IEnumerator BeginEffect()
    {
        mat.SetFloat("_Opacity", 0);
        opacity = 0;
        yield return new WaitForEndOfFrame();
        print("routine started");
        //IncreaseOpacity();
        DOTween.To(() => opacity, x => opacity = x, 1, 2);

        if (RightSide)
        {

            mat.SetFloat("_SideFloat", .50f);
            CurrentMoveTween = transform.DOMoveY(-100, TweenDur, false).SetEase(Ease.InQuad);
            CurrentRotTween = transform.DORotate(new Vector3(0, 0, -70), 2.7f).SetEase(Ease.InQuad);
            while (coroutineTimer > 0)
            {
                mat.SetFloat("_Opacity", opacity);
                coroutineTimer -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {

            CurrentMoveTween = transform.DOMoveY(-100, TweenDur, false).SetEase(Ease.InQuad);
            mat.SetFloat("_SideFloat", .49f);
            CurrentRotTween = transform.DORotate(new Vector3(0, 0, 70), 2.7f).SetEase(Ease.InQuad);
            while (coroutineTimer > 0)
            {

                mat.SetFloat("_Opacity", opacity);
                coroutineTimer -= Time.deltaTime;
                yield return null;
            }

        }
        
        ResetSelf();
    }
}
