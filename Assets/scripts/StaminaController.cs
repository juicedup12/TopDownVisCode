using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    Image ImageFill;

    // Start is called before the first frame update
    void Start()
    {
        ImageFill = transform.GetChild(0).GetComponent<Image>();
        Debug.Log("image fill is ", ImageFill.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetFillAmount(float value)
    {
        if(ImageFill)
        ImageFill.fillAmount = value;
    }

}
