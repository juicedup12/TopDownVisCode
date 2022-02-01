using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyButton : Button, ISelectHandler, IDeselectHandler
{
    public bool IsItHighlighted() { return IsHighlighted(); }
    Text text;
    bool Highlighted;
    bool selected;
    public string HighlightText;

    protected override void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = "";
        Highlighted = false;
    }

    private void Update()
    {
        if (IsItHighlighted() && !Highlighted && !selected)
        {
            print("highlighted");
            text.text = HighlightText;
            Highlighted = true;
            //ShopController.instance.deselectCurrent();
            ShopController.instance.SelectCurrent(gameObject);
        }
        else if (!IsItHighlighted() && Highlighted && !selected)
        {
            print("unhilighted");
            text.text = "";
            Highlighted = false;
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (!Highlighted)
        {
            Debug.Log(this.gameObject.name + " was selected");
            print("selected");
            text.text = HighlightText;
            Highlighted = true;
            selected = true;
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (Highlighted)
        {
            Debug.Log(this.gameObject.name + " was deselected");
            print("deselected");
            text.text = "";
            Highlighted = false;
            selected = false;
        }
    }

}
