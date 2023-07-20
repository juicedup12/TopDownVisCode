using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using topdown;

namespace InteractableSelect
{
    public class ItemSelectionManager : MonoBehaviour
    {
        private ISelectionResponse _selectionResponder;
        private IObjectDetection _objectDetection;

        private Transform CurrentSelection;
        private bool DoDetect = true;
        private Transform Selection;

        // Start is called before the first frame update
        void Start()
        {
            _selectionResponder = GetComponent<ISelectionResponse>();
            _objectDetection = GetComponent<IObjectDetection>();
        }
        private void OnEnable()
        {
            WalkSubscribe();
        }

        private void OnDisable()
        {
            WalkUnsubscribe();
        }

        private void WalkSubscribe()
        {
            WalkSMB.OnBusy += () => { DoDetect = false; _selectionResponder.OnDeselect(); };
            WalkSMB.OnWalk += () => { DoDetect = true; };
        }

        private void WalkUnsubscribe()
        {
            WalkSMB.OnBusy -= () => { DoDetect = false; _selectionResponder.OnDeselect(); };
            WalkSMB.OnWalk -= () => { DoDetect = true; };
        }

        public void InteractWithObject()
        {
            if(CurrentSelection)
            {
                print(CurrentSelection + "interacted");
                CurrentSelection.gameObject.GetComponent<Iinteractable>().interact();
            }
            else
            {
                print("no current selection");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (DoDetect)
            {
                /************************
                remember the single responsibility principle
                this class will only be responsible for getting
                one item, then deciding what to do with it
                if you want debug text on screen, then make another class
                *******************************/

                //deselect 
                //if there's no items detected and there's a current item, then deslect
                //print("object detected: " + _objectDetection.Check());
                //print("current selection :" + CurrentSelection);
                if (!_objectDetection.Check() && CurrentSelection != null)
                {
                    _selectionResponder.OnDeselect();
                }

                //detect for selection
                //get object from getselection
                Selection = null;

                Selection = _objectDetection.GetSelection();


                //onselect
                //select the item
                if (CurrentSelection != Selection || CurrentSelection == null && Selection)
                {
                    CurrentSelection = Selection;
                    _selectionResponder.OnSelect(CurrentSelection);
                }
            }

        }

    }

}