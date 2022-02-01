using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

//objects with this script will activate ui, pause the game 
//and run the action associated with this script
namespace topdown
{
    public class Item : MonoBehaviour
    {
        
        public delegate void ContactAction(IEnumerator Callback);
        public static event ContactAction OnContact;
        protected bool Pressed;
        public SelectionBehavior selectionUI;
        public player playerref;
        GameObject SelectionObj;
        protected Action InteractAction;

        //what to do when yes is pressed
        public UnityAction ClickAction;



        public virtual void OnDisable()
        {
            ClickAction -= PrintonButtonPress;
        }


        // Start is called before the first frame update
        public virtual void Start()
        {
            

            //get selection object reference
            SelectionObj = GameObject.Find("ChoiceSelection");
            print("item press selection obj is " + selectionUI == null);
            selectionUI = SelectionObj.GetComponent<SelectionBehavior>();
            ClickAction += PrintonButtonPress;
            Debug.Log("selection game object assigned " + selectionUI.gameObject.name, selectionUI.gameObject);
            Pressed = false;


        }

        // Update is called once per frame
        void Update()
        {

        }

        void PrintonButtonPress()
        {
            print("click action activated");
            Pressed = true;
            
        }


        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {

                Vector3 dirtoplayer = collision.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirtoplayer, dirtoplayer.magnitude, LayerMask.GetMask("wall"));
                Debug.DrawRay(transform.position, dirtoplayer, Color.red, 5f);
                print("layermask is " + LayerMask.GetMask("wall"));
                if (hit.collider == null)
                {
                    if (!Pressed)
                    {
                        InteractUIBehavior.instance.DisplayUI(InteractAction, transform.position, "To inspect");

                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //not sure why this was here...
                //if (Pressed)
                //{
                //    selectionUI.OpenUI(ClickAction);
                //    Pressed = false;
                //}

                InteractUIBehavior.instance.HideUI();
            }
        }

    }

    
}
