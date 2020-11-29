using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class Item : MonoBehaviour
{

    CircleCollider2D colCirc;
    public delegate void ContacftAction(IEnumerator Callback);
    public static event ContacftAction OnContact;
    bool Pressed;
    public SelectionBehavior selectionUI;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Pressed = false;
        colCirc = GetComponent<CircleCollider2D>();
        colCirc.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void OnYes()
    {
        print("Item was given yes");
    }


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Pressed)
        {
            selectionUI.OpenUI(this);
            Pressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
         if(Pressed)
        {
            Pressed = false;
        }
    }

}
