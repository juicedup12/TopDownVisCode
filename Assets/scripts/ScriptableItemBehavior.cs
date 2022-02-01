using UnityEngine;

namespace topdown
{



    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScriptableItemBehavior : Item
    {
        public ItemObject ScriptableItem;
        SpriteRenderer sprenderer;
        SpriteRenderer pulserender;
        Animator anim;
        BoxCollider2D boxcol;
        GameObject ObjPrefab;

        private void OnValidate()
        {
            if (ScriptableItem != null)
            {
                sprenderer = GetComponent<SpriteRenderer>();
                sprenderer.sprite = ScriptableItem.ItemSprite;
                pulserender = transform.GetChild(0).GetComponent<SpriteRenderer>();
                pulserender.sprite = ScriptableItem.ItemSprite;
                //Debug.Log("pulse obj is " + pulserender.gameObject, pulserender.gameObject);
                if(ScriptableItem.ItemPrefab != null)
                {
                    ObjPrefab = ScriptableItem.ItemPrefab;
                }
                else
                {
                    print("scriptable item prefab is null");
                }
            }
        }

        // Start is called before the first frame update
        public override void Start()
        {
            if (ScriptableItem.TypeOfItem == ItemObject.ItemType.inventory)
            {
                sprenderer.sprite = ScriptableItem.ItemToAdd.UnidentifiedSprite;
            }
            boxcol = GetComponent<BoxCollider2D>();
            boxcol.isTrigger = true;
                anim = GetComponent<Animator>();
            if (ScriptableItem.ItemPrefab != null)
            {
                ObjPrefab = ScriptableItem.ItemPrefab;
            }
            else
            {
                print("scriptable item prefab is null");
            }
            base.Start();
            

            switch (ScriptableItem.TypeOfItem)
            {
                case ItemObject.ItemType.room:
                    ClickAction += CreateInRoom;
                    break;

                case ItemObject.ItemType.global:
                    ClickAction += CreateGlobally;
                    break;

                case ItemObject.ItemType.inventory:
                    ClickAction += AddInventory;

                    break;
            }

            InteractAction = () => {
                print("player interacting with obj");
                Pressed = true;
                switch (ScriptableItem.TypeOfItem)
                {
                    case ItemObject.ItemType.room:
                        selectionUI.OpenUI(ClickAction, SelectionBehavior.ActionType.button);
                        break;
                    case ItemObject.ItemType.global:
                        selectionUI.OpenUI(ClickAction, SelectionBehavior.ActionType.button);

                        break;
                    case ItemObject.ItemType.inventory:
                        selectionUI.OpenUI(ClickAction, SelectionBehavior.ActionType.item);

                        break;
                    default:
                        break;
                }

                //gameObject.SetActive(false);
            };
        }

        void CreateInRoom()
        {
            Debug.Log("spawning object in room", gameObject);
            Time.timeScale = 1;
            //Gmanager.instance.StartRoomEvent(ScriptableItem.spawnType, ObjPrefab);
            //Destroy(gameObject);
            boxcol.enabled = false;
        }

        void CreateGlobally()
        {
            Debug.Log("creating global event");
            Gmanager.instance.StartGlobalEvent(ScriptableItem.ItemPrefab);
            //Destroy(gameObject);
            boxcol.enabled = false;
        }


        //puts item into player's inventory
         void AddInventory()
        {
            Debug.Log("calling gmanager inventoryadd");

            ScriptableItem.ItemToAdd.itemsprite = ScriptableItem.ItemSprite;
            playerref.AddToInventory(ScriptableItem.ItemToAdd);
            //Destroy(gameObject);
            boxcol.enabled = false;
        }

        
    }
        
}
