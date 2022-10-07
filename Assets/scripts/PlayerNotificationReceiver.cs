using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using topdown;

public class PlayerNotificationReceiver : MonoBehaviour, INotificationReceiver
{
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if(notification is PlayerNotificationMarker)
        {
            Debug.Log("player notification received");
            if (_player) _player.WalkIntoLevel();
        }
    }

    [SerializeField] player _player;

}
