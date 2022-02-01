using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIInventory : MonoBehaviour
{
    public Transform player;
    Canvas parentcanvas;
    public float verticaloffset;
    public RectTransform InventoryScroll;
    public GameObject inventoryObj;
    public List<Image> itemimages;
    int itemIndex;
    bool isDirty;
    Tween movetween;

    // Start is called before the first frame update
    void Start()
    { 
        parentcanvas = GetComponentInParent<Canvas>();
        isDirty = true;
        InventoryScroll = inventoryObj.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = worldToUISpace(parentcanvas, player.position) + new Vector3(0, verticaloffset) ;
        transform.position = player.position;
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

    public void MoveItem(float dir)
    {

        //InventoryScroll.anchoredPosition += dir == 1 ? new Vector2(-50,0): new Vector2( +50, 0);
        movetween.Kill();
        movetween =  InventoryScroll.DOAnchorPosX(InventoryScroll.anchoredPosition.x + -dir * 50, .5f);
    }

    public void ShowUIInventory()
    {
        InventoryScroll.anchoredPosition = Vector2.zero;
        gameObject.SetActive(true);
    }

    public void AsignItem(Sprite img ,Color itemcolor, int index)
    {
        print("setting item scroll active");
        inventoryObj.SetActive(true);
        print("setting item 1 active");
        itemimages[index].gameObject.SetActive(true);
        itemimages[index].sprite = img;
        itemimages[index].color = itemcolor;
        itemIndex++;
    }


    public void ResetItems()
    {
        foreach(Image img in itemimages)
        {
            img.gameObject.SetActive(false);
        }
    }


    
}
