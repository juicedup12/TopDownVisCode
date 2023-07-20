using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Scrip to place on UI text game object to move transform relative to a game object's worldpos
public class WorldUIConvert : MonoBehaviour
{
    [SerializeField]
    Transform Target;
    [SerializeField]
    GameObject DialogueCanvas;
    RectTransform ThisRect;
    public Transform SetTarget { set => Target = value; }


    private void Awake()
    {
        ThisRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        WorldToUI();
    }

    public void WorldToUI()
    {
        if (!Target) return;

        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = DialogueCanvas.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(Target.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        Mathf.FloorToInt((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        Mathf.FloorToInt((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        ThisRect.anchoredPosition = WorldObject_ScreenPosition;
    }

}
