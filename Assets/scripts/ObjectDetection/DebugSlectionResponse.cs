using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMonkey;
using UnityEngine;

namespace InteractableSelect
{
    public class DebugSlectionResponse : MonoBehaviour, ISelectionResponse
    {
        float timer = 0;
        bool StartCooldown = false;
        public GameObject SelectionTarget;

        private void Start()
        {
            if (!SelectionTarget) return;
            SelectionTarget.SetActive(false);
        }

        private void Update()
        {
           
        }

        public void OnDeselect()
        {
            if(SelectionTarget)
            SelectionTarget.SetActive(false);
            //print("deselecting");
        }

        public void OnSelect(Transform item)
        {
            if (item)
            {
                print("item detected " + gameObject.name);
                if (!SelectionTarget) return;
                SelectionTarget.SetActive(true);
                SelectionTarget.transform.position = item.position;
            }
            
        }
    }
}
