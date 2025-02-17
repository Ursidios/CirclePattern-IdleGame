using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public PlayerConfig playerConfig;
    public TMP_Text moneyText;   

    public UpgradeManager upgradeManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeManager.upgrades[5].buttons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        upgradeManager.upgrades[4].buttons.SetActive(!upgradeManager.upgrades[3].buttons.activeSelf);


        if(upgradeManager.upgrades[4].level >= upgradeManager.upgrades[4].maxLevel)
        {
            upgradeManager.upgrades[3].buttons.SetActive(true);
        }
        else
        {
            upgradeManager.upgrades[3].buttons.SetActive(false);
        }
        moneyText.text = playerConfig.money.ToString("0");
    }
}
