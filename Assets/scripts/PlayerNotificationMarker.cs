using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace topdown
{
    public class PlayerNotificationMarker : Marker, INotification
    {
        public PropertyName id { get; }
        /// <summary>
        /// set to true if the player is in control, else set to false
        /// </summary>
        [SerializeField] bool SetPlayerActive;
    }
}
