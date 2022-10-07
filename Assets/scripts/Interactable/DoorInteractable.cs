using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using topdown;

public class DoorInteractable : MonoBehaviour, Iinteractable
{
    public PostLevelBehavior PostLevel;
    public Vector3Int direction;
    public void interact()
    {
        PostLevel.MoveDirection(direction);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
