using System;
using System.IO;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private PlayerConfig playerConfig;
    public UIScript uIScript;
    public UiManager uiManager;
    public Upgrades[] upgrades;
    private string savePath;
    public SaveGameScript saveGameScript;

    void Start()
    {
        playerConfig = GetComponent<PlayerConfig>();
        savePath = Path.Combine(Application.persistentDataPath, "upgradeData.json");   
       
    }
    void Update()
    {
        foreach (var upgrade in upgrades)
        {
            upgrade.textCost.text = upgrade.moneyCost.ToString("$0.0");
        }
    }

    public void TimeSpeed(int index)
    {
        if (!MoneyComparison(0))
            return;

        uIScript.EnableTimeSpeedButton();
        uIScript.timeSpeedTimerMax += 5;
        playerConfig.IncreaseMoneyMult(upgrades[0].moneyPercentageIncrease);
    }

    public void IncreaseRotationSpeed(int index)
    {
        if (!MoneyComparison(1))
            return;

        playerConfig.drawCirclesInGameList[index].GetComponent<CircleDrawScript>().RotationSpeedMulti += 0.5f;
        playerConfig.IncreaseMoneyMult(upgrades[1].moneyPercentageIncrease);
    }

    public void IncreaseDrawDuration()
    {
        if (!MoneyComparison(2))
            return;

        PencilConfig.trailDuration += 1;
        playerConfig.IncreaseMoneyMult(upgrades[2].moneyPercentageIncrease);
    }

    public void IncreaseCircleSize()
    {
        if (!MoneyComparison(4))
            return;

        playerConfig.circlesInGameList[playerConfig.circleAmount - 1].transform.localScale += new Vector3(1, 1, 1);
        playerConfig.IncreaseMoneyMult(upgrades[4].moneyPercentageIncrease);
        uiManager.ZoomOutButton();
    }

    public void IncreaseCiclesNumber()
    {
        if (!MoneyComparison(3))
            return;

        playerConfig.SpawnCircle(new Vector3(1, 1, 1));
        playerConfig.IncreaseMoneyMult(upgrades[3].moneyPercentageIncrease);
        upgrades[4].level = 0;
        uiManager.ZoomOutButton();
        uiManager.ZoomOutButton();
    }

    public void IncreaseDrawCicles()
    {
        if (!MoneyComparison(5))
            return;

        playerConfig.SpawnDrawCircle();
        playerConfig.IncreaseMoneyMult(upgrades[5].moneyPercentageIncrease);
    }

    public bool MoneyComparison(int UpgradeIndex)
    {
        if (upgrades[UpgradeIndex].moneyCost <= playerConfig.money)
        {
            playerConfig.money -= upgrades[UpgradeIndex].moneyCost;
            upgrades[UpgradeIndex].moneyCost += upgrades[UpgradeIndex].moneyCost * (upgrades[UpgradeIndex].inflationPercentageCost / 100f);
            upgrades[UpgradeIndex].level++;

            saveGameScript.SaveAll();
            return true;
        }
        return false;
    }
}

[Serializable]
public class Upgrades
{
    public string name;
    public float moneyCost;
    public int level;
    public int maxLevel;
    public TMP_Text textCost;
    public float moneyPercentageIncrease;
    public float inflationPercentageCost;
    public GameObject buttons;
}
