using UnityEngine;
using UnityEngine.EventSystems;
using topdown;

public class ShopController : MonoBehaviour 
{

    //shop controller will display inventory items represented by buttons.
    //when buttons are highlighted a price will appear and a button hint to buy
    //it will handle logic when buying items
    //When items are bought, they will be added to player's inventory controller
    //items will be removed when they are bought or are out of stock


    //items to include will be mystery potions, identifying sticks, and a sword

    //hold references to item objects


    public MyButton[] buttons;
    public ItemClass[] items;
    public player Playerref;
    public MyButton ExitButton;
    
    public EventSystem eventsys;
    public static ShopController instance;
    

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.sprite = items[i].itemsprite;

            print("adding listener to button " + items[i].name + " at index " + i);
            int index = i;
            buttons[i].HighlightText = "buy for 5 coins";
            buttons[i].onClick.AddListener(delegate { TryPurchase(index); });
            eventsys.SetSelectedGameObject(buttons[0].gameObject);
        }
        //ItemButton = (MyButton)bttn;
        //rint("itembutton is null? " + ItemButton);
        //print("item button is highlighted " + ItemButton.IsItHighlighted());
        ExitButton.HighlightText = "press to exit";
        if (instance == null)
            instance = this;
    }
    
    public void deselectCurrent()
    {
        eventsys.SetSelectedGameObject(null);
    }


    public void SelectCurrent(GameObject obj)
    {
        eventsys.SetSelectedGameObject(obj);
    }

    public void TryPurchase(int itemIndex)
    {
        if(Playerref.Money >= 5)
        {
            print($"item index is {itemIndex}");
            print("adding item " + itemIndex);
            Playerref.AddToInventory(items[itemIndex]);
            print("purchased item");
            Playerref.AddMoney(-5);
        }
        else
        {
            print("Not enough money to purchase");
        }
    }

    



    public void OnExit()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        print("exited shop");
    }
}


