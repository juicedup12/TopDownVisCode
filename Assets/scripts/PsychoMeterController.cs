using UnityEngine;

public class PsychoMeterController : MonoBehaviour
{

    public static PsychoMeterController instance;
    Canvas parentcanvas;
    public RectTransform rect;
    public Vector3 anchoredpos;
    public Vector2 localpoint;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        parentcanvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        if (instance == null)
        {
            instance = this;
        }
        anchoredpos = rect.anchoredPosition;
        localpoint = RectTransformUtility.WorldToScreenPoint(parentcanvas.worldCamera, rect.localPosition);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
