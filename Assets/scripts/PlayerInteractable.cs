using System;
using UnityEngine;
using topdown;

//base class for objects that the player can interact with
public class PlayerInteractable : MonoBehaviour
{

    SelectionBehavior selectionHandler;
    event Action ClickedResponse;
    bool Pressed;

    void OnEnable()
    {
        ClickedResponse -= () => {print($"object{gameObject.name} was selected");};
        print("interactable enabled");

    }

    void OnDisable()
    {
        ClickedResponse -= () => {print($"object{gameObject.name} was selected");};

    }

    void Start()
    {
        FindObjectSelector();
    }

    public virtual void FindObjectSelector()
    {
            //get selection object reference
            GameObject selector = GameObject.Find("ChoiceSelection");
            print("item press selection obj is " + selector == null);
            if(selector != null)
            selectionHandler = selector.GetComponent<SelectionBehavior>();
            ClickedResponse?.Invoke();
            Pressed = false;
    }
}
