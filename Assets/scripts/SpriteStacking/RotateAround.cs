using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject target;
    displayObject displayObject;

    private void Start()
    {
        displayObject = GetComponent<displayObject>();
    }

    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, Vector3.forward, 20 * Time.deltaTime);
        displayObject.rotation = transform.rotation.eulerAngles;
    }
}