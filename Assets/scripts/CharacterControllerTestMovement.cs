using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using topdown;
using UnityEngine.InputSystem;

public class CharacterControllerTestMovement : MonoBehaviour
{
    Controlls inputs;
    Vector2 InputMove;
    Rigidbody2D rb2d;
    [SerializeField] float speed;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {

        //rb2d.velocity = InputMove * speed * Time.fixedDeltaTime;
        rb2d.AddForce(InputMove * speed);
    }




    private void OnEnable()
    {
        inputs = new Controlls();
        inputs.Enable();

        inputs.Player.Walking.performed += onMoveInput;
        inputs.Player.Walking.canceled += (InputAction.CallbackContext ctx) => { InputMove = Vector2.zero; };
    }

    private void OnDisable()
    {

        inputs.Disable();

        inputs.Player.Walking.performed -= onMoveInput;
    }

    void onMoveInput(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        InputMove = value.normalized;
        //print("input move is " + InputMove);
    }
}
