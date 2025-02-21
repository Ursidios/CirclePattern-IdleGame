using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CosmeticUpgradesManager : MonoBehaviour
{
    public CosmeticUpgrades[] cosmeticUpgrades;

    private PlayerConfig playerConfig;
    public List<PencilConfig> pencilConfig;
    public bool isShootingStar;
    public int drawCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerConfig = GetComponent<PlayerConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var upgrade in cosmeticUpgrades)
        {
            upgrade.textCost.text = upgrade.moneyCost.ToString("0");
        }
    }

    public void MoreDraw(int index)
    {
        if (!MoneyComparison(index))
            return;


        pencilConfig.Clear();
        foreach (var item in FindObjectsOfType<PencilConfig>())
        {
            pencilConfig.Add(item);
        }
        pencilConfig.Reverse();
        for (int i = 0; i < cosmeticUpgrades[0].level + 1; i++)
        {
            pencilConfig[i].enabled = true;

        }
    }

    public void ShootingStar(int index)
    {
        if (!MoneyComparison(index))
            return;
       isShootingStar = true;

    }

    public bool MoneyComparison(int UpgradeIndex)
    {
        if (cosmeticUpgrades[UpgradeIndex].moneyCost <= playerConfig.moneySpecial)
        {
            playerConfig.money -= cosmeticUpgrades[UpgradeIndex].moneyCost;
            cosmeticUpgrades[UpgradeIndex].moneyCost += cosmeticUpgrades[UpgradeIndex].moneyCost * (cosmeticUpgrades[UpgradeIndex].inflationPercentageCost / 100f);
            cosmeticUpgrades[UpgradeIndex].level++;
            
            //saveGameScript.SaveAll();
            return true;
        }
        return false;
    }
}

[Serializable]
public class CosmeticUpgrades
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