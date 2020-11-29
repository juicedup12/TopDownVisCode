using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionBehavior : MonoBehaviour
{
    public TextMeshProUGUI Yestext;
    public TextMeshProUGUI Notext;
    public TMP_ColorGradient onGrad;
    public TMP_ColorGradient offGrad;
    public GameObject UIObj;
    bool Selecting;
    bool Yes;
    Item itemref;

    // Start is called before the first frame update
    void Start()
    {
        Yestext.colorGradientPreset = onGrad;
        Notext.colorGradientPreset = offGrad;
        Yes = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Selecting)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SwitchChoice();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(Yes)
                {
                    print("chose yes");
                    Time.timeScale = 1;
                    itemref.OnYes();
                    UIObj.SetActive(false);

                }
                else
                {
                    print("chose no");
                    Time.timeScale = 1;
                    UIObj.SetActive(false);
                }
            }
        }
    }


    public void OpenUI(Item item)
    {
        itemref = item;
        Time.timeScale = 0;
        UIObj.SetActive(true);
        Selecting = true;
    }

    void SwitchChoice()
    {
        print("switching choice");
        if(!Yes)
        {
            Yestext.colorGradientPreset = onGrad;
            Notext.colorGradientPreset = offGrad;
            Yes = true;
        }
        else
        {
            Yestext.colorGradientPreset = offGrad;
            Notext.colorGradientPreset = onGrad;
            Yes = false;
        }
    }


}
