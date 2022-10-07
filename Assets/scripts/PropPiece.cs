using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropPiece : MonoBehaviour
{
    Rigidbody2D rb2;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPropObject(Vector2 velocity, Sprite spr = null)
    {
        rb2.velocity = velocity;
        if (spr) spriteRenderer.sprite = spr;
    }
}
