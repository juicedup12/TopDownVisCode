using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    List<Image> HeartUIList;
    public Image RedScreenFill;
    Tween HeartTween;
    public float ScaleSize;
    public float duration;
    public GameObject HeartUIicon;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        HeartUIList = new List<Image>();
        AddHeartIcon();
        AddHeartIcon();
        currentHealth = 200;
        foreach(Image img in HeartUIList)
        {
            //Debug.Log("heart ui is " + img.name, img.gameObject);

        }
        //HeartTween = HeartUIList.rectTransform.DOPunchScale(new Vector3(ScaleSize, ScaleSize), duration, 1, 1).SetLoops(-1);
        
    }
    

    public void StartDamageIndicator()
    {
        StartCoroutine(IndicateHealthChange(false));
    }


    public void SetHealth(float Health)
    {
        int HeartsToFill =Mathf.CeilToInt( Health / 100);
        print("hearts to fill is " + HeartsToFill);
        foreach(Image img in HeartUIList)
        {
            img.fillAmount = 0;
        }
        
        if(HeartsToFill > HeartUIList.Count)
        {
            //fill all icons
            print("full health");
            foreach(Image hearts in HeartUIList)
            {
                hearts.fillAmount = 1;
            }
            return;
        }


        for (int i = 0; i < HeartsToFill; i++)
        {
            //fill hearts that are before the last one
            if (i != HeartsToFill -1)
            {
                HeartUIList[i].fillAmount = 1;
                print("heart num" + i + " is 100% full");
            }
            else
            {

                if(Health % 100 == 0)
                {
                    HeartUIList[i].fillAmount = 1;
                    print("heart " + i + " is full");
                    return;
                }

                float healthamnt = (Health % 100)/100;
                HeartUIList[i].fillAmount = healthamnt;
                print("heart " + i + " fill amount is " + HeartUIList[i].fillAmount);
                print("heart num" + i + " is " + (Health - (HeartsToFill - 1) * 100) + " full");
            }
            
        }
    }

    public void setCurrentHealth(int Health)
    {
        currentHealth = Health;
    }

    public void ChangePlayerHealth(float health)
    {
        //if healing
        if(health >= currentHealth)
        {
            print("healing");
            StartCoroutine( IndicateHealthChange(true));
            setCurrentHealth((int)health);
            SetHealth(health);
        }
        else if ( health < currentHealth)
        {
            print("damaging");
            StartCoroutine( IndicateHealthChange(false));
            setCurrentHealth((int)health);
            SetHealth(health);
        }
    }

    IEnumerator IndicateHealthChange(bool IsHealing)
    {
        if (!IsHealing)
        {

            RedScreenFill.color = Color.red;
            RedScreenFill.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(.2f);

            RedScreenFill.gameObject.SetActive(false);
            print("activating red screen");

            yield return null;
        }
        else
        {
            RedScreenFill.color = Color.green;
            RedScreenFill.gameObject.SetActive(true);
            yield return new WaitForSeconds(.2f);

            RedScreenFill.gameObject.SetActive(false);

            print("activating green screen");
            yield return null;
        }
    }

    public void AddHeartIcon()
    {
        Image heart = Instantiate(HeartUIicon, transform).transform.GetChild(0).GetComponent<Image>();

        if (HeartUIList.Count >= 1)
        {
            heart.transform.parent.GetComponent<RectTransform>().anchoredPosition += new Vector2(HeartUIList.Count * 30, 0);
         }
        //Debug.Log("heart game object is " + heart);
        HeartUIList.Add(heart);
    }

}
