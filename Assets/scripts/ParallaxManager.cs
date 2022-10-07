using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ParallaxManager : MonoBehaviour
{

    public Parralax[] parralaxObjs;
    Vector2 parralaxeffectMultiplier;

    public AnimationCurve curve;
    public float RotationAmount;
    public float timeToComplete;
    float rotation = 0;

    public Ease ease;

    private void Start()
    {
        //StartCoroutine(RotateCoroutine());
    }


    IEnumerator RotateCoroutine()
    {
        yield return new WaitForSeconds(1);
        float i = 0;
        float rate =  1/timeToComplete;
        float rotation = 0;
        while (i < 1)
        {
            i += rate * Time.deltaTime;
            rotation = Mathf.Lerp(0, RotationAmount, curve.Evaluate(i));
            AssignParalaxValue(rotation);
            yield return null;
        }
        yield return null;
    }

    public void TweenParalax(float rotationAmount, float duration)
    {
        print("starting tween");
        Tween rotateTween = DOTween.To(() => rotation, x => rotation = x, rotation + rotationAmount, duration);
        rotateTween.SetEase(ease);
        
        rotateTween.onUpdate = () => { AssignParalaxValue(rotation);  };

    }

    public void AssignParalaxValue(float f)
    {
        //asign rotation to parralax objects
        foreach (Parralax parralax in parralaxObjs)
        {
            parralax.xValue = f;
        }

    }
}
