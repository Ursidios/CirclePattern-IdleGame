using System;
using System.IO;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private PlayerConfig playerConfig;
    public UiManager uiManager;
    public Upgrades[] upgrades;
    private string savePath;
    public SaveGameScript saveGameScript;

    void Start()
    {
        playerConfig = GetComponent<PlayerConfig>();
        savePath = Path.Combine(Application.persistentDataPath, "upgradeData.json");
        //LoadPlayerPrefs();
        
        UpdateUI();
    }

    // public void LoadPlayerPrefs()
    // {
    //     if (File.Exists(savePath))
    //     {
    //         string json = File.ReadAllText(savePath);
    //         print(savePath);
    //         UpgradeData data = JsonUtility.FromJson<UpgradeData>(json);
            
    //         for (int i = 0; i < upgrades.Length; i++)
    //         {
    //             upgrades[i].moneyCost = data.upgrades[i].moneyCost;
    //             upgrades[i].level = data.upgrades[i].level;
    //             upgrades[i].maxLevel = data.upgrades[i].maxLevel;
    //             upgrades[i].moneyPercentageIncrease = data.upgrades[i].moneyPercentageIncrease;
    //             upgrades[i].inflationPercentageCost = data.upgrades[i].inflationPercentageCost;
    //         }
            
    //         playerConfig.pencilConfig.trailRenderer.time = data.trailDuration;
    //         uiManager.MainCamera.fieldOfView = data.fieldOfView;
    //     }
    // }

    // public void SavePlayerPrefs()
    // {
    //     UpgradeData data = new UpgradeData();
    //     data.upgrades = upgrades;
    //     data.trailDuration = playerConfig.pencilConfig.trailRenderer.time;
    //     data.fieldOfView = uiManager.MainCamera.fieldOfView;
        
    //     string json = JsonUtility.ToJson(data, true);
    //     File.WriteAllText(savePath, json);
    // }

    public void IncreaseSpeed(int index)
    {
        if (!MoneyComparison(0))
            return;
        
        playerConfig.drawCirclesInGameList[index].GetComponent<CircleDrawScript>().speed += 0.5f;
        playerConfig.IncreaseMoneyMult(10);
    }

    public void IncreaseRotationSpeed(int index)
    {
        if (!MoneyComparison(1))
            return;

        playerConfig.drawCirclesInGameList[index].GetComponent<CircleDrawScript>().RotationSpeedMulti += 0.5f;
        playerConfig.IncreaseMoneyMult(10);
    }

    public void IncreaseDrawDuration()
    {
        if (!MoneyComparison(2))
            return;

        playerConfig.pencilConfig.trailRenderer.time += 1;
        playerConfig.IncreaseMoneyMult(10);
    }

    public void IncreaseCircleSize()
    {
        if (!MoneyComparison(4))
            return;

        playerConfig.circlesInGameList[playerConfig.circleAmount - 1].transform.localScale += new Vector3(1, 1, 1);
        playerConfig.IncreaseMoneyMult(10);
        uiManager.ZoomOutButton();
    }

    public void IncreaseCiclesNumber()
    {
        if (!MoneyComparison(3))
            return;

        playerConfig.SpawnCircle(new Vector3(1, 1, 1));
        playerConfig.IncreaseMoneyMult(50);
        upgrades[4].level = 0;
        uiManager.ZoomOutButton();
        uiManager.ZoomOutButton();
    }

    public void IncreaseDrawCicles()
    {
        if (!MoneyComparison(5))
            return;

        playerConfig.SpawnDrawCircle();
        playerConfig.IncreaseMoneyMult(50);
    }

    public bool MoneyComparison(int UpgradeIndex)
    {
        if (upgrades[UpgradeIndex].moneyCost <= playerConfig.money)
        {
            playerConfig.money -= upgrades[UpgradeIndex].moneyCost;
            upgrades[UpgradeIndex].moneyCost += upgrades[UpgradeIndex].moneyCost * (upgrades[UpgradeIndex].inflationPercentageCost / 100f);
            upgrades[UpgradeIndex].level++;
            UpdateUI();
            //SavePlayerPrefs();
            saveGameScript.SaveAll();
            return true;
        }
        return false;
    }

    public void UpdateUI()
    {
        foreach (var upgrade in upgrades)
        {
            upgrade.textCost.text = upgrade.moneyCost.ToString("$0.0");
        }
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

[Serializable]
public class UpgradeData
{
    public Upgrades[] upgrades;
    public float fieldOfView;
    public float trailDuration;
}
