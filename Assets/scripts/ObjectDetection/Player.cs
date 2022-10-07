using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using TMPro;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI GuiText;
    public float speed;
    Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        Move();
    }

    private void LookAtMouse()
    {
        Vector2 DirToMouse = Camera.main.ScreenToWorldPoint( Input.mousePosition) - transform.position;
        float angle =  Mathf.Atan2(DirToMouse.y , DirToMouse.x) * Mathf.Rad2Deg;
        UpdateDebugTxt(angle.ToString());
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void Move()
    {
        moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += moveVector * speed;
    }


    private void UpdateDebugTxt(string text)
    {
        if (GuiText) 
        GuiText.text = text;
    }

}
