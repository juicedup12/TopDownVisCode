using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseUI : MonoBehaviour
{


    public static PauseUI instantce;
    public Button ResumeButton;
    Image img;
    UnityAction action;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        if(instantce == null)
        {
            instantce = this;
        }
        
        action += ResumeGame;
        ResumeButton.onClick.AddListener(action);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1; img.enabled = false; ResumeButton.gameObject.SetActive(false); 
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        img.enabled = true;
        ResumeButton.gameObject.SetActive(true);
    }
    

}
