using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RadialButton : MonoBehaviour
{
    RadialController RadController;
    Outline outline;

    public float viewFov;
    public float viewDistance;
    Sprite emptyimg;
    public Image Itemimg;

    public float currentScale;

    //index will indicate what item in the list this button is connected to
    public int ItemIndex;

    public bool SlotOccupied;

    //when mouse is distant enough from center;
    public float MaxScale;
    //when mouse is farthest away from center;
    public float MinScale;

    //the rate at which the button changes size depending on how far the mouse is
    public float ScaleRate;

    //mouse direction will be the direction from the center of the screen to the mouse
    //will be provided by the radial controller
    Vector3 mouseDir;



    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        RadController = GetComponentInParent<RadialController>();
        Itemimg = transform.GetChild(0).GetComponent<Image>();
        emptyimg = Itemimg.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        //if mouse is close and angle mathes arc
        float angle = Vector2.Angle(transform.up, RadController.PointDir);
        if(angle< viewFov * .5)
        {
            currentScale = RadController.MouseAngle().magnitude * ScaleRate;
            if (currentScale < MinScale) currentScale = MinScale;
            if (currentScale > MaxScale) currentScale = MaxScale;
            SetHighlighted(currentScale == MaxScale);
            transform.localScale = Vector3.one * currentScale;
            
            //Debug.Log("mouse angle is on view fov of " + transform.up, gameObject);
            //print("current mouse mag is :" + RadController.PointDir.magnitude);

        }
        else
        {
            transform.localScale =  Vector3.one;
            SetHighlighted(false);
        }
        
    }

    //makes current button the highlighted one or removes it
    void SetHighlighted(bool Selecting)
    {
        if (Selecting)
        {
            outline.enabled = true;
            //tell controller this is the currently highlighted button
            if(SlotOccupied)
            {
                //tell rad controller to show current item description
                RadController.HighlightedButton = this;
                RadController.ShowItemDescription(ItemIndex);
            }
            else
            {
                //tell rad controller that there is no item
                RadController.ShowEmptyDescription();
                RadController.HighlightedButton = null;

            }
        }
        else
        {
            outline.enabled = false;
            RadController.Deselected();
            //tell controller to remove this button
        }
    }

    public void AssignItemValue(Image ItemImage)
    {

    }


    public void SetEmptySlot()
    {
        Itemimg.sprite = emptyimg;
        SlotOccupied = false;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //draw the cone of view
        Vector3 forward = transform.up;


        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

        Handles.color = new Color(0, 1.0f, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

        //Draw attack range
        //Handles.color = new Color(1.0f, 0, 0, 0.1f);
        //Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
    }
#endif
}

