using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForwardFollow : MonoBehaviour
{

    Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowCamera();
    }

    void FollowCamera()
    {
        //transform.position = new Vector3(cameraTransform.position.x + CamOffset.x, cameraTransform.position.y + CamOffset.y, cameraTransform.forward + CamOffset.z);
        transform.position = cameraTransform.position + (cameraTransform.forward * 10);
        //transform.position = transform.position + new Vector3(0, cameraTransform.position.y, 0);
        transform.LookAt(cameraTransform.position);
        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        //transform.position = new Vector3(transform.position.x, YPosition, transform.position.z);
        //transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
