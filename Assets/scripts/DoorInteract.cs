using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for use with doors that each do seperate things, like changing scenes
public class DoorInteract : MonoBehaviour, Iinteractable
{
    public virtual void interact()
    {
        print("door interacted");
    }

}
