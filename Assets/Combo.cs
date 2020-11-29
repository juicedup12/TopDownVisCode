using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    public static Combo instance;
    TextMeshProUGUI text;
    int KillCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {

        text = GetComponent<TextMeshProUGUI>();
        text.text = "Combo: " + KillCount;
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddToCombo()
    {
        KillCount++;
        text.text = "Combo: " + KillCount;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
