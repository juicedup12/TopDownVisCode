using System.Collections.Generic;
using UnityEngine;
using topdown;
using UnityEngine.InputSystem;
using TMPro;



//handles the display and back end of radial buttons
public class RadialController : MonoBehaviour
{


    public Vector2 PointDir;
    Vector2 centerScreen;
    public TextMeshProUGUI Description;
    public RadialButton HighlightedButton;
    public Transform DoubleRadialParent;
    RadialButton[] doubleRadialMenu = new RadialButton[2];
    public PlayerInventoryController inventorycontroller;
    


    // Start is called before the first frame update
    void Start()
    {
        Description = GetComponentInChildren<TextMeshProUGUI>();
        assignRadialBtnComponents();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //PointDir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //centerScreen = (Vector2)Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f));
        //MouseAngle();

    }

    //gets references to buttons
    void assignRadialBtnComponents()
    {
        for (int i = 0; i < 2; i++)
        {
            doubleRadialMenu[i] = DoubleRadialParent.GetChild(i).GetComponent<RadialButton>();
            Debug.Log("assigned " + i + "double radial button to " + DoubleRadialParent.GetChild(i), DoubleRadialParent.GetChild(i));
        }
    }


    //check if item has been identified
    public void AssignItemButtons(List<ItemClass> items)
    {
        
        int ItemNum = items.Count;
        switch (ItemNum)
        {
            case  0:
                //show 2 radial buttons, don't assign items
                print("(radial controller) no items in items list");

                break;
            case 1:
                //show 2 radial buttons, assign 1 item

                print("(radcontroller) 1 item in items list");
                print("item 0 is " + items[0].name);
                print("double radial 1 is " + doubleRadialMenu[0]);
                doubleRadialMenu[0].Itemimg.sprite = items[0].GetSprite();
                doubleRadialMenu[0].SlotOccupied = true;
                break;
            case 2:
                //show 2 radial buttons, assign 2 items
                break;
            
            default:
                break;
        }
    }

    //uses item in button that is currently highlighted
    public void UseSelceted()
    {
        //close menu if currently highlighted radial button is empty
        if (HighlightedButton == null) return;


        if(HighlightedButton.SlotOccupied)
        {
            //makes the item use it's effect on player
            inventorycontroller.items[HighlightedButton.ItemIndex].UseItem(Gmanager.instance._player);
            //stop showing inventory since item was used
            print("using " + inventorycontroller.items[HighlightedButton.ItemIndex]);
            print("consuming item index " + HighlightedButton.ItemIndex);
            inventorycontroller.RemoveItem(HighlightedButton.ItemIndex);
            HighlightedButton.SetEmptySlot();
            //close inventory
            inventorycontroller.ShowInventory();
        }
        else
        {
            //if empty stop showing inventory
            inventorycontroller.ShowInventory();
        }
    }

    public void ThrowSelected()
    {
        //only throw if current highlighted radial button is not empty
    }


    //for revealing items
    public void IdentifySelected()
    {
        //must account for item being already identified, and identification sticks in player inventory
        if(HighlightedButton != null)
        if(HighlightedButton.SlotOccupied)
        {
                inventorycontroller.items[HighlightedButton.ItemIndex].Identify();
                ShowItemDescription(HighlightedButton.ItemIndex);
        }
    }


    //based on the index provided the description will match the item in the itemslist
    public void ShowItemDescription(int index)
    {
        Description.text = "highlighted item is : " + inventorycontroller.items[HighlightedButton.ItemIndex].GetName();
    }

    public void ShowEmptyDescription()
    {
        Description.text = "No item in this slot";
    }

    public void Deselected()
    {
        HighlightedButton = null;
        Description.text = "";
    }

    public Vector2 MouseAngle()
    {
        Vector2 angle;
        angle = PointDir - centerScreen;
        //print("mouse pos is " + mousepos);
        //print("mouse dir is " + angle);
        //print("screen center point is " + Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f)));
        Debug.DrawLine((Vector2)Camera.main.ViewportToWorldPoint(new Vector2(.5f, .5f)), (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Color.red, .1f);
        return angle;
        
    }
}
