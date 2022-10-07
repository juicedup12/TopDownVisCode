using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputMessageResponse : MonoBehaviour
{

    InputAction inputAction;
    // Start is called before the first frame update
    void Start()
    {
        inputAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Activate");
        inputAction.started += ActivateStarting;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateStarting(InputAction.CallbackContext ctx)
    {
        print("starting");
    }

    public void OnLook(InputValue value)
    {
        var val = value.Get<Vector2>();
        print("onlook responding with value: " + val);
    }

    public void OnActivate( )
    {
        print("activateTriggered");
    }
}
