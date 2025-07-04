using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticUpgradesManager : MonoBehaviour
{
    public CosmeticUpgrades[] cosmeticUpgrades;

    private PlayerConfig playerConfig;
    public List<PencilConfig> pencilConfig;
    public bool isShootingStar;
    public int drawCounter;

    public GameObject shootingStarParticle;
    private SaveGameScript saveGameScript;
    private UpgradeManager upgradeManager;
    public bool canBuyDraw;
    public GameObject popUpBlockDrawUpgrade;

    public AudioSource buySoundSource;
    public Color defealtColorButtons;
    public bool allUpgradesInMax;

    public GameObject allInMaxText;
    public bool isInfiniteMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerConfig = GetComponent<PlayerConfig>();
        saveGameScript = FindObjectOfType<SaveGameScript>();
        upgradeManager = GetComponent<UpgradeManager>();
    }

    // Update is called once per frame
    void Update()
    {


        foreach (var upgrade in cosmeticUpgrades)
        {
            upgrade.textCost.text = upgrade.moneyCost.ToString("0");

            if (upgrade.level >= upgrade.maxLevel)
            {
                upgrade.buttons.SetActive(false);
            }
            else
            {
                upgrade.buttons.SetActive(true);
            }

            if (upgrade.moneyCost > playerConfig.moneySpecial)
            {
                upgrade.buttons.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
            else
            {
                upgrade.buttons.GetComponent<CanvasGroup>().alpha = 1;
            }
        }

        shootingStarParticle.SetActive(isShootingStar);

        if (cosmeticUpgrades[0].level < upgradeManager.upgrades[3].level)
        {
            canBuyDraw = true;
            cosmeticUpgrades[0].buttons.gameObject.GetComponent<Image>().color = defealtColorButtons;
        }
        else
        {
            canBuyDraw = false;
            cosmeticUpgrades[0].buttons.gameObject.GetComponent<Image>().color = Color.red;
        }

        if (!allUpgradesInMax)
        {
            for (int i = 0; i < cosmeticUpgrades.Length; i++)
            {
                int doneUpgradeCounter = 0;

                foreach (var upgrade in cosmeticUpgrades)
                {
                    if (upgrade.level >= upgrade.maxLevel)
                    {
                        doneUpgradeCounter++;
                        //print(doneUpgradeCounter);
                    }
                }

                if (doneUpgradeCounter >= cosmeticUpgrades.Length)
                {
                    allUpgradesInMax = true;
                    break;
                }
            }
        }

        allInMaxText.SetActive(allUpgradesInMax);
    }

    public void MoreDraw(bool isStarting)
    {
        if (!canBuyDraw && !isStarting)
        {
            popUpBlockDrawUpgrade.SetActive(true);
        }
        else
        {
            if (!MoneyComparison(0, isStarting))
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
    }

    public void ShootingStar(bool isStarting)
    {
        if (!isStarting)
        {
            if (!MoneyComparison(1, false))
                return;
        }
    }

    public bool MoneyComparison(int UpgradeIndex, bool isStarting)
    {

        if (cosmeticUpgrades[UpgradeIndex].level >= cosmeticUpgrades[UpgradeIndex].maxLevel)
        {
            return false;
        }
        else
        {
            if (cosmeticUpgrades[UpgradeIndex].moneyCost <= playerConfig.moneySpecial || isStarting)
            {
                if (!isStarting)
                {
                    playerConfig.moneySpecial -= cosmeticUpgrades[UpgradeIndex].moneyCost;
                    buySoundSource.Play();
                }
                else
                {
                    
                }

                playerConfig.IncreaseSpecialMoneyMult(cosmeticUpgrades[UpgradeIndex].moneyPercentageIncrease);
                cosmeticUpgrades[UpgradeIndex].moneyCost += cosmeticUpgrades[UpgradeIndex].moneyCost * (cosmeticUpgrades[UpgradeIndex].inflationPercentageCost / 100f);
                cosmeticUpgrades[UpgradeIndex].level++;

                if (UpgradeIndex == 1)
                {
                    isShootingStar = true;
                    shootingStarParticle.SetActive(isShootingStar);
                }



                return true;
            }
        }
        return false;
    }

    public void infinitMode()
    {
        foreach (var item in cosmeticUpgrades)
        {
            item.maxLevel = 1000;
        }

        isInfiniteMode = true;
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