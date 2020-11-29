using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace topdown
{
    public class SpearsActivateItem : Item
    {

        RoomGen roomref;
        public GameObject ObjToInstantiate;
        // Start is called before the first frame update
        
        public override void Start()
        {
            base.Start();
            roomref = GetComponentInParent<RoomGen>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnYes()
        {
            base.OnYes();
            CreateSpears();
        }

        void CreateSpears()
        {
            roomref.ActivateSpears(ObjToInstantiate);
        }

    } 
}
