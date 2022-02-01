using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

namespace topdown
{

    //the behavior that lets player chose wether or not to perform an action
    //pressing a button 
    public class SelectionBehavior : MonoBehaviour
    {
        public TextMeshProUGUI Yestext;
        public TextMeshProUGUI Notext;
        public TextMeshProUGUI Description;
        public GameObject firstselected;
        public TMP_ColorGradient onGrad;
        public TMP_ColorGradient offGrad;
        public GameObject SelectionParent;
        public Image img;
        bool Selecting;
        bool Yes;
        Item itemref;
        public EventSystem eventsys;
        public Button YesButton;
        public Button NoButton;
        public enum ActionType { item, button, ledge}


        private void OnEnable()
        {
            //YesButton.onClick.AddListener(delegate { Time.timeScale = 1; });
            YesButton.onClick.AddListener(delegate { print("disabling selection UI"); SelectionParent.SetActive(false); });
            NoButton.onClick.AddListener(delegate { Time.timeScale = 1;});
        }

        private void OnDisable()
        {
            //remove listeners so previous items don't get added
            YesButton.onClick.RemoveAllListeners();
        }


        // Start is called before the first frame update
        void Start()
        {
            Yestext.colorGradientPreset = onGrad;
            Notext.colorGradientPreset = offGrad;

            YesButton.onClick.AddListener(delegate { print(" clicked"); });
            SelectionParent.SetActive(false);
            //NoButton.onClick.AddListener(() => Time.timeScale = 1);

        }



        //receives an action to execute when yes is pressed
        public void OpenUI(UnityAction ActionOnActivate, ActionType type)
        {
            YesButton.onClick.RemoveAllListeners();
            ActionOnActivate += () =>  SelectionParent.SetActive(false); 
            Gmanager.instance.GamePaused = true;
            Time.timeScale = 0;
            eventsys.SetSelectedGameObject(firstselected);
            YesButton.onClick.AddListener(ActionOnActivate);
            SelectionParent.SetActive(true);
            switch (type)
            {
                case ActionType.item:
                    Description.text = "Pick up item?";
                    break;
                case ActionType.button:
                    Description.text = "press button?";
                    break;
                case ActionType.ledge:
                    Description.text = "get on ledge?";

                    break;
                default:
                    break;
            }
           

        }



    }

}
