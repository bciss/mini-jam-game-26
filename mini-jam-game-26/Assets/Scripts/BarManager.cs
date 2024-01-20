using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    private float lerpTimer;
    
    [Header("Money Bar")]
    private float money;
    public float maxMoney = 100;
    public float chipSpeed = 2f;
    public Image frontMoneyBar;
    public Image backMoneyBar;

    [Header("Suscpicion Bar")]
    private float suspicion;
    public float maxSuspicion = 100;
    public Image frontSuspicionBar;
    public Image backSuspicionBar;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
        suspicion = 0;
    }

    // Update is called once per frame
    void Update()
    {
        money = Mathf.Clamp(money, 0, maxMoney);
    }
    void LateUpdate()
    {
        UpdateMoneyUI();
        UpdateSuspicionUI();
    }
    
    public void UpdateMoneyUI()
    {
        float fillF = frontMoneyBar.fillAmount;
        float fillB = backMoneyBar.fillAmount;
        float hFraction = money/maxMoney;
        if (fillB > hFraction)
        {
            frontMoneyBar.fillAmount = hFraction;
            backMoneyBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backMoneyBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backMoneyBar.color = Color.green;
            backMoneyBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontMoneyBar.fillAmount = Mathf.Lerp(fillF, backMoneyBar.fillAmount, percentComplete);
        }
    }

    public void UpdateSuspicionUI()
    {
        float fillF = frontSuspicionBar.fillAmount;
        float fillB = backSuspicionBar.fillAmount;
        float hFraction = suspicion/maxSuspicion;
        if (fillB > hFraction)
        {
            frontSuspicionBar.fillAmount = hFraction;
            backSuspicionBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backSuspicionBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backSuspicionBar.color = Color.red;
            backSuspicionBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontSuspicionBar.fillAmount = Mathf.Lerp(fillF, backSuspicionBar.fillAmount, percentComplete);
        }
    }

    
    public void AddMoney(float moneyToAdd)
    {
        money += moneyToAdd;
        lerpTimer = 0f;
    }
    public void AddSuspicion(float suspicionToAdd)
    {
        suspicion += suspicionToAdd;
        lerpTimer = 0f;
    }
}
