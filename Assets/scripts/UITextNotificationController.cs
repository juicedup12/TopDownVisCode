using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public enum ControllerType { GamePad, Keyboard }
public class UITextNotificationController : MonoBehaviour
{
    ControllerType CurrentController = ControllerType.Keyboard;
    public static UITextNotificationController instance;
    TextMeshProUGUI uitext;

    public string currentAcceptButton { get; private set; }
    public string currentCancelButton { get; private set; }
    public string currentPrompt;

    void OnEnable()
    {
        InputSystem.onDeviceChange += (InputDevice inputDevice, InputDeviceChange change) => 
        {
            if(change == InputDeviceChange.ConfigurationChanged)
            {
                print("current input device is " + inputDevice.displayName);
            }
        };
    }


    //Start is called before the first frame update
     void Start()
    {
        uitext = GetComponent<TextMeshProUGUI>();
        //uitext.text = $"press <sprite index={(int)CurrentController}>";
        currentAcceptButton = $"<sprite index={(int)CurrentController}>";
        currentCancelButton = $"cancel butt {(int)CurrentController}";
        UpdateText("press " + currentAcceptButton + " to enter the room");
        CurrentController = ControllerType.Keyboard;
        if(instance == null)
        {
            instance = this;
        }
    }
    


    public void CheckControllerChange(ControllerType Controller)
    {
        if(Controller != CurrentController)
        {
            CurrentController = Controller;
            print("Switching controller to " + Controller);
            //update UI
            //uitext.text = $"press <sprite index={(int)CurrentController}> to go";
            currentAcceptButton = $"<sprite index={(int)CurrentController}>";
            currentCancelButton = $"cancel butt {(int)CurrentController}";
            UpdateText(currentPrompt);
            //for prompts that highlight objects
               InteractUIBehavior.instance.CurrentButtonSprite = $"<sprite index={(int)CurrentController}> ";
            InteractUIBehavior.instance.UpdateText();
        }
    }

    public void UpdateText(string prompt)
    {
        //gameObject.SetActive(true);
        ShowNotification(true);
        currentPrompt = prompt;
        uitext.text = prompt;
    }

    public void ShowNotification(bool show)
    {
        if(show)
        {
            //add an animation
            uitext.enabled = true;
        }
        else
        {
            uitext.enabled = false;
        }
    }

}

