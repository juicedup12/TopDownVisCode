using System.Collections.Generic;
using UnityEngine;
using topdown;

//holds data and holds functions that affect UIinventory
public class PlayerInventoryController : MonoBehaviour
{
    public UIInventory uIInventory;
    public List<ItemClass> items;
    public RadialController RadInventory;
    bool isDirty;
    public bool isOpen;
    public player Playerref;
    public float InventoryDir;
    public bool ConsumeItem;
    int ItemCount;
    int ItemIndex;
    public GameObject ThrowItemprefab;
    public float angleoffset;
    public Vector2 PointDir;

    // Start is called before the first frame update
    void Start()
    {

        isDirty = true;
        isOpen = false;
        items = new List<ItemClass>();
        Playerref = GetComponent<player>();
        ItemIndex = 0;
        if(RadInventory)
        RadInventory.inventorycontroller = this;
    }



    //displays inventory UI and updates fields
    public void ShowInventory(bool ThrowItem = false)
    {
        if (!isOpen)
        {
            print("opening inventory");
            Time.timeScale = 0;
            //set to zero so previous inputs dont affect menu
            InventoryDir = 0;
            ConsumeItem = false;

            //if no items don't proceed
            if(items.Count < 1)
            {
                print("no items in inventory");
                //return;
            }
            print("there are " + items.Count + " items in inventory");
            //uIInventory.ShowUIInventory();
            RadInventory.gameObject.SetActive(true);

            if (isDirty)
            {
                //uIInventory.ResetItems();
                //for (int i = 0; i < items.Count; i++)
                //{
                //    //uIInventory.AsignItem(items[i].itemsprite, items[i].itemcolor, i);

                //}


                RadInventory.AssignItemButtons(items);
                //print("item " + items[i].name + " has been assigned to itemUI slot " + i);
                isDirty = false;
            }
        }
        else
        {
            print("closing inventory");
            //uIInventory.gameObject.SetActive(false);
            //Playerref.CloseInventory = true;
            Time.timeScale = 1;
            if (!ThrowItem)
            {
                Playerref.InventoryClose();
                RadInventory.gameObject.SetActive(false);
            }
            else
            {
                print("inventory close set to true");
                Playerref.InventoryClose(true);
                RadInventory.gameObject.SetActive(false);
            }
            isOpen = false;
            ItemIndex = 0;
        }
    }
    
    public bool AddItem(ItemClass item)
    {
        if(items.Count > 5)
        {
            print("inventory full");
            return false;
        }
        items.Add(item);
        isDirty = true;
        print("added " + item.name + " item amount  is " + items.Count);
        return true;
    }

    public void RemoveItem(int index)
    {
        print("removing item at index " + index);
        //remove when item is used
        items.RemoveAt(index);
        

        isDirty = true;
    }


    public void ThrowItem()
    {
        float mouseangle = Playerref.GetAngleToMouse();
        ProjectileItem ItemProjectile = Instantiate(ThrowItemprefab, transform.position, Quaternion.Euler(new Vector3(0,0, mouseangle + angleoffset))).GetComponent<ProjectileItem>();
        print("Instantiated item with an angle of : " + mouseangle);
        ItemProjectile.Item = items[ RadInventory.HighlightedButton.ItemIndex];
        RemoveItem(ItemIndex);
        
        
    }



    public void InventorySelelct()
    {
        //if (items.Count < 2) return;

        //allows player class to read and modify input variables
        Playerref.move = Vector3.zero;
        Playerref.MoveLookAhead();
        //change to check for animator parameter values
        //Playerref.MovementInput();

        if(InventoryDir > .2 || InventoryDir < -.2)
        {
            if (InventoryDir < 0 )
            {
                //pressed left
                if(ItemIndex - 1 < 0)
                {
                    print("moving left went lower than item bounds, item index is " + ItemIndex );
                    InventoryDir = 0;
                    return;
                }

                uIInventory.MoveItem(-1);
                print("Moving items left, item index is " + ItemIndex);
                InventoryDir = 0;
                ItemIndex--;
            }
            else
            {
                //pressed right
                if (ItemIndex + 1 > items.Count -1)
                {
                    print("moving left went over item bounds item index is " + ItemIndex);
                    InventoryDir = 0;
                    return;
                }
                uIInventory.MoveItem(1);
                print("Moving items right, item index is " + ItemIndex);
                InventoryDir = 0;
                ItemIndex++;
            }
        }


        //if accept button press consume item
        if (ConsumeItem)
        {
            RadInventory.UseSelceted();
            ConsumeItem = false;
        }

        RadInventory.PointDir = PointDir;

        //close inventory if player presses inventory button again
        if(Playerref.OpenInventory)
        {

            ShowInventory();
            Playerref.OpenInventory = false;
        }

        //change to a seperate animation that checks for animator parameter
        //throw item
        //if(Playerref.Attacking)
        //{

        //    //move code into a seperate animation SMB where letting go of attack will throw the item

        //    if (RadInventory.HighlightedButton != null)
        //    {            //throw item 
        //        print("throwing Item");
        //        //use a get from player for the angle

        //        ShowInventory(true);
        //    }
        //}

        //identify
        if(Playerref.Parry)
        {
            if(RadInventory.HighlightedButton != null)
            {
                //update radial icon and description

                RadInventory.IdentifySelected();
                RadInventory.AssignItemButtons(items);
            }
        }

    }
}



