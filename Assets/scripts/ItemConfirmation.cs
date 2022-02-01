using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using topdown;

//class will display info if item was obtained
public class ItemConfirmation : MonoBehaviour
{

    public Sprite InventoryFullImg;
    public Sprite ItemSprite;
    public Image ImageToDislay;
    public TextMeshProUGUI ItemNameUIText;
    public TextMeshProUGUI ConfirmationText;
    public string GotItemText;
    public string ItemName;
    public string InventoryFullText;
    [SerializeField]
    player playerref;
    public static ItemConfirmation instance;
    bool Confirmed = false;



    // Start is called before the first frame update
    void Start()
    {
        //playerref = Gmanager.instance._player;
        if(instance == null)
        {
            instance = this;
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowItemConfirmation(bool isItemAdded, ItemClass item = null)
    {
        gameObject.SetActive(true);
        if(isItemAdded)
        {
            //set the image to the sprite that was found
            ImageToDislay.sprite = item.itemsprite;
            //set the bottom text to the item name
            ItemNameUIText.text = item.name;

            //set the top text telling the player the item was obtained
            ConfirmationText.text = GotItemText;
        }
        else
        {

            ImageToDislay.sprite = InventoryFullImg;
            ConfirmationText.text = InventoryFullText;
        }
        StartCoroutine(WaitforButton());

    }
    


    IEnumerator WaitforButton()
    {
        yield return new WaitForSecondsRealtime(1f);
        print("wait for button delay");
        yield return new WaitUntil(() => playerref.PressedAccept);
        print("unpausing UI");
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

}
