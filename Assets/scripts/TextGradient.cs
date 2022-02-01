using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextGradient : MonoBehaviour
{
    TextMeshProUGUI tmpobj;
    TMP_ColorGradient gradient;
    Color tmpcolor, tmpcolor2, tmpcolor3, tmpcolor4;

    private void Start()
    {
        tmpobj = GetComponent<TextMeshProUGUI>();
        tmpcolor = Color.white;
        tmpcolor2 = Color.white;
        tmpcolor3 = Color.black;
        tmpcolor4 = Color.black;
       // tmpcolor = Color.red;
        //tmpcolor2 = Color.red;
        //tmpcolor3 = Color.red;
        DOTween.To(() => tmpcolor, x => tmpcolor = x, Color.red, 10).SetEase(Ease.InBounce);
        DOTween.To(() => tmpcolor2, x => tmpcolor2 = x, Color.red, 10);
        DOTween.To(() => tmpcolor3, x => tmpcolor3 = x, Color.green, 10);
        DOTween.To(() => tmpcolor4,x => tmpcolor4 = x, Color.green, 10).SetLoops(-1);
        tmpobj.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    private void Update()
    {
        //tmpobj.ForceMeshUpdate();
        tmpobj.colorGradient = new VertexGradient(tmpcolor, tmpcolor2, tmpcolor3, tmpcolor4);
        //tmpobj.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

}

