using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{

    public static GameOverController instance;
    public Button GameOverButton;
    Image redScreen;
    public Text gameovvertext;

    // Start is called before the first frame update
    void Start()
    {
        redScreen = GetComponent<Image>();
        if(instance == null)
        {
            instance = this;
        }
    }

    public void GameOverScreen()
    {
        redScreen.enabled = true;
        redScreen.DOColor(Color.red, 2);
        gameovvertext.DOText("game over", 2);
        GameOverButton.gameObject.SetActive(true);
    }

    public void startover()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
