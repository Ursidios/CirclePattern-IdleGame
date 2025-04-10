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

    public GameObject shootingStarParticle;
    private SaveGameScript saveGameScript;
    private UpgradeManager upgradeManager;
    public bool canBuyDraw;
    public GameObject popUpBlockDrawUpgrade;

    public AudioSource buySoundSource;
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
            
            if(upgrade.level >= upgrade.maxLevel)
            {
                upgrade.buttons.SetActive(false);
            }

            if(upgrade.moneyCost > playerConfig.moneySpecial)
            {
                upgrade.buttons.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
            else
            {
                upgrade.buttons.GetComponent<CanvasGroup>().alpha = 1;
            }
        }

        shootingStarParticle.SetActive(isShootingStar);

        if(cosmeticUpgrades[0].level < upgradeManager.upgrades[3].level)
        {
            canBuyDraw = true;
        }
        else
        {
            canBuyDraw = false;
        }
    }

    public void MoreDraw(bool isStarting)
    {
            if(!canBuyDraw && !isStarting)
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
        if (!MoneyComparison(1, false))
            return;
        
    }

    public bool MoneyComparison(int UpgradeIndex, bool isStarting)
    {

        if(cosmeticUpgrades[UpgradeIndex].level >= cosmeticUpgrades[UpgradeIndex].maxLevel)
        {
            return false;
        }
        else
        {
            if (cosmeticUpgrades[UpgradeIndex].moneyCost <= playerConfig.moneySpecial || isStarting)
            {
                if(!isStarting)
                {
                    playerConfig.moneySpecial -= cosmeticUpgrades[UpgradeIndex].moneyCost;
                }

                playerConfig.IncreaseSpecialMoneyMult(cosmeticUpgrades[UpgradeIndex].moneyPercentageIncrease);
                cosmeticUpgrades[UpgradeIndex].moneyCost += cosmeticUpgrades[UpgradeIndex].moneyCost * (cosmeticUpgrades[UpgradeIndex].inflationPercentageCost / 100f);
                cosmeticUpgrades[UpgradeIndex].level++;

                if(UpgradeIndex == 1)
                {
                    isShootingStar = true;
                    shootingStarParticle.SetActive(isShootingStar);
                }

                buySoundSource.Play();


                return true;
            }
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