using UnityEngine;
using TMPro;
using DG.Tweening;




public class Combo : MonoBehaviour
{
    public static Combo instance;
    TextMeshProUGUI text;
    int KillCount = 0;
    Tween SwipeTween;
    float CanvasWidth;
    float CanvasHeight;
    RectTransform rect;
    public float Duration;
    public Ease SlideEase;
    public AnimationCurve curve;
    public float TextEdgeOffset;
    Tween colortween;
    Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        CanvasWidth = 640;
        CanvasHeight = 360;
        text = GetComponent<TextMeshProUGUI>();
        mat = text.fontSharedMaterial;
        text.text = "";
        if (instance == null)
        {
            instance = this;
        }

        SwipeTween = rect.DOLocalMoveX(CanvasWidth, Duration).SetEase(curve);
        SwipeTween.Pause();
        SwipeTween.onComplete = () => { KillCount = 0; print("restarting kill count"); SwipeTween.Restart(); SwipeTween.Pause(); text.text = ""; };
        colortween = mat.DOColor(Color.red, ShaderUtilities.ID_FaceColor, .5f).From(Random.ColorHSV(0,1,0,1,0,1,1,1));
        colortween.SetLoops(-1);
        SwipeTween.SetUpdate(true);
        //colortween.onComplete= () => { print("colortween done"); }
    }

    public void ComboStart()
    {
        if(!SwipeTween.IsPlaying())
        {
            SwipeTween.Play();
        }
        KillCount++;
        text.text = "Combo: " + KillCount;
        SwipeTween.Restart();
        float ypos = Random.Range(0 + TextEdgeOffset, CanvasHeight - TextEdgeOffset);
        rect.anchoredPosition = new Vector3(0, ypos);
        print("spawning combo at " + ypos);
        
    }


    /// <summary>
    /// Will need to use co routines,
    /// at the end of frame will need to check if multiple enemy's are hit
    /// then only in end of frame use add to combo or consecutive combo
    /// </summary>

    //when a combo has already started, do a fade out animation
    public void AddToCombo()
    {

    }


    //when multiple enemies have been attacked
    public void ConsecutiveCombo()
    {

    }
    
}
