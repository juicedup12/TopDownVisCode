using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoneyUIBehavior : MonoBehaviour
{
    public Text MoneyAmount;
    public float CurrentMoneyAmount;
    public float ChangeRate;
    RectTransform rectTransform;
    public float AmountToMove;
    Vector2 Originalpos;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Originalpos = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReflectMoneyChange(int currentPlayerMoney , int amount)
    {
        StopCoroutine("DoMoneyChange");
        rectTransform.position = Originalpos;
        CurrentMoneyAmount = currentPlayerMoney;
        rectTransform.DOAnchorPosX(rectTransform.position.x + AmountToMove, 1);
        StartCoroutine("DoMoneyChange", amount);
    }


    IEnumerator DoMoneyChange(int amount)
    {
        int DesiredAmount = (int)CurrentMoneyAmount + amount;
        while(CurrentMoneyAmount != DesiredAmount)
        {
            float Direction = Mathf.Sign(amount);
            CurrentMoneyAmount += Direction * ChangeRate;
            MoneyAmount.text = Mathf.FloorToInt(CurrentMoneyAmount).ToString();
            if (BetweenRanges(DesiredAmount - 1, DesiredAmount + 1, (int)CurrentMoneyAmount))
            {
                print("done counting money");
                MoneyAmount.text = DesiredAmount.ToString();
                yield return new WaitForSeconds(.5f);
                rectTransform.DOAnchorPosX(rectTransform.position.x - AmountToMove, 1);
                StopCoroutine("DoMoneyChange");
            }
            yield return null;
        }

        
    }


    public static bool BetweenRanges(int a, int b, int number)
    {
        return (a <= number && number <= b);
    }

}
