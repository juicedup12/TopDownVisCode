using UnityEngine;
using Cinemachine;


//for containing code regarding which object the camera should follow and various functions
public class CameraManager : MonoBehaviour
{

    public CinemachineVirtualCamera Playercam;
    public CinemachineVirtualCamera RoomCam;
    public static CameraManager instance;

    // Start is called before the first frame update
    void Start()
    {
     if(instance == null)
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //deactivates player cam and sets room cam active
    public void TransferCam(GameObject room)
    {
        print("start transfer cam to room " + room.name + " at " + room.transform.position);
        Playercam.gameObject.SetActive(false);
        RoomCam.gameObject.SetActive(true);
        RoomCam.Follow = room.transform;
    }

    public void TransferCamToPlayer()
    {
        Playercam.gameObject.SetActive(true);
        RoomCam.gameObject.SetActive(false);
    }
}
