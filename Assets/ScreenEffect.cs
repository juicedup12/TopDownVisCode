using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour
{

    public RectTransform LeftImg;
    public RectTransform RightImg;
    public GameObject bg;
    public Material TintMat;
    float opacity = 0;
    public float OpacitySpeed;
    Texture texture;
    

    // Start is called before the first frame update
    void Start()
    {
        //TintMat = LeftImg.GetComponent<RawImage>().material;
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool IncreaseOpacity()
    {
        if (opacity > 1)
        {
            opacity += OpacitySpeed * Time.deltaTime;
            TintMat.SetVector("_Opacity", new Vector4(opacity, 0));
            return true;
        }
        return false;
    }

    public IEnumerator MoveImg()
    {
        
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, LeftImg.DOMove(LeftImg.position + new Vector3(0, -650, 0), 1.5f).SetEase(Ease.InQuint));
        seq.Insert(.2f, LeftImg.DORotate(new Vector3(0, 0, 70), 2).SetEase(Ease.InQuad));
        seq.Insert(.4f, RightImg.DOMove(RightImg.position + new Vector3(0, -650, 0), 1.5f).SetEase(Ease.InQuint));
        seq.Insert(.6f, RightImg.DORotate(new Vector3(0, 0, -70), 2).SetEase(Ease.InQuad));

        //LeftImg.DOMove(LeftImg.position + new Vector3(0, -650, 0), 2).SetEase(Ease.InQuint);
        //LeftImg.DORotate(new Vector3(0, 0, 70), 4).SetEase(Ease.InQuad);
        //RightImg.DOMove(RightImg.position + new Vector3(0, -650, 0), 2).SetEase(Ease.InQuint);
        //RightImg.DORotate(new Vector3(0, 0, -70), 4).SetEase(Ease.InQuad);
        yield return new WaitForEndOfFrame();

        //on complete start level animations
        //seq.onComplete
        yield return null;
    }
}
