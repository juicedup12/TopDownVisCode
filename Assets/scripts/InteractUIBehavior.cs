using UnityEngine;
using System;
using topdown;
using TMPro;

public class InteractUIBehavior : MonoBehaviour
{
    public player playerref;
    Vector3 ScreenPosition;
    Canvas parentcanvas;
    Action OnAccept;
    Action OnAttack;
    public TextMeshProUGUI text;

    public string CurrentPromptText;
    public string CurrentButtonSprite;

    public static InteractUIBehavior instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        text = GetComponentInChildren<TextMeshProUGUI>();

        parentcanvas = GetComponentInParent<Canvas>();
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = worldToUISpace(parentcanvas, ScreenPosition);
        //if(playerref.PressedAccept)
        //{
        //    OnAccept();
        //}
    }


    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    public void DisplayUI(Action ActionToInvoke, Vector3 interactPosition, string InteractText)
    {
        gameObject.SetActive(true);
        OnAccept = ActionToInvoke;
        ScreenPosition = interactPosition;
        CurrentPromptText = InteractText;
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = CurrentButtonSprite + CurrentPromptText;
    }

    public void SetCurrentButtonSprite(string buttonSprite)
    {

    }


    public void HideUI()
    {
        OnAccept = null;
        gameObject.SetActive(false);
        print("hiding ui");
    }
}
