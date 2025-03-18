using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public float money;
    public float moneyMult;
    public float moneySpecial;
    public float moneySpecialMult;
    float accumulatedTime = 0f;
    [Space(20)]
    [Header("New Circle Config")]
    public int circleAmount;
    public int circleDrawAmount;
    public GameObject circlePrefab;
    public GameObject circleDrawPrefab;
    public PencilConfig pencilConfig;
    public List<GameObject> circlesInGameList;
    public List<GameObject> drawCirclesInGameList;
    public Color[] circleColors;
    public int colorSelector;
    public SaveGameScript saveGameScript;

    void Start()
    {

        circleAmount = circlesInGameList.Count;
        circleDrawAmount = drawCirclesInGameList.Count;
    }


    void Update()
    {
        MoneyRules(moneyMult);
        SpecialMoneyRules(moneySpecialMult);
    }

    public void IncreaseMoneyMult(float amount)
    {
        moneyMult += moneyMult * (amount / 100f);
    }
    public void IncreaseSpecialMoneyMult(float amount)
    {
        moneySpecialMult += moneySpecialMult * (amount / 100f);

    }

    public void MoneyRules(float mult)
    {
        money += Time.deltaTime * mult;
    }
    public void SpecialMoneyRules(float mult)
    {
        moneySpecial += Time.deltaTime * mult;
    }

    public void SpawnCircle(Vector3 circleScale)
    {
        GameObject newCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);

        if (circleScale == new Vector3(1,1,1))
        {
            newCircle.transform.localScale = circlesInGameList[circleAmount - 1].transform.localScale + circleScale;
        }
        else
        {
            newCircle.transform.localScale = circleScale;
        }
        
        newCircle.GetComponent<SpriteRenderer>().color = circleColors[colorSelector];
        
        if(newCircle.GetComponent<CircleDrawScript>().drawPencil != null)
        {
            newCircle.GetComponent<CircleDrawScript>().drawPencil.GetComponent<PencilConfig>().color = circleColors[colorSelector];
        }
        
        newCircle.GetComponent<CircleDrawScript>().speed = 1;
        newCircle.GetComponent<CircleDrawScript>().RotationSpeedMulti = 1;
        
        circlesInGameList.Add(newCircle);
        circleAmount = circlesInGameList.Count;
        circlesInGameList[circleAmount - 2].GetComponent<CircleDrawScript>().baseCircle = newCircle;
        colorSelector = (colorSelector + 1) % circleColors.Length;
    }

    public void SpawnDrawCircle()
    {
        GameObject newCircle = Instantiate(circleDrawPrefab, Vector3.zero, Quaternion.identity);
        newCircle.GetComponent<CircleDrawScript>().speed = 1;
        newCircle.GetComponent<CircleDrawScript>().RotationSpeedMulti = 1;

        circleDrawAmount = drawCirclesInGameList.Count;
        drawCirclesInGameList.Add(newCircle);
    }
}

