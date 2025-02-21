using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public PlayerConfig playerConfig;
    public TMP_Text moneyText;   
    public TMP_Text specialMoneyText;   
    public GameObject timeSpeedButton;   

    public UpgradeManager upgradeManager;

    public bool timeSpeedOn;
    public float timeSpeedTimerMax;
    private float timeSpeedTimer;

    public bool blockTimeSpeed;
    public float blockTimeSpeedTimerMax;
    private float blockTimeSpeedTimer;

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
        specialMoneyText.text = playerConfig.moneySpecial.ToString("0");

        TimeSpeed();

        if(blockTimeSpeed)
        {
            blockTimeSpeedTimer -= Time.deltaTime;
        }

        if(blockTimeSpeedTimer <= 0)
        {
            blockTimeSpeed = false;
        }
        
        if(upgradeManager.upgrades[0].level > 0)
        {
            timeSpeedButton.SetActive(!blockTimeSpeed);
        }
        else
        {
            timeSpeedButton.SetActive(false);
        }
    }

    public void EnableTimeSpeedButton()
    {
        timeSpeedButton.SetActive(true);
    }

    public void ActiveTimeSpeed()
    {
        timeSpeedTimer = timeSpeedTimerMax;
        blockTimeSpeedTimer = blockTimeSpeedTimerMax;

        if(!blockTimeSpeed)
        {
            timeSpeedOn = true;
            blockTimeSpeed = true;
        }
    }

    public void TimeSpeed()
    {
        if(timeSpeedOn)
        {
            timeSpeedTimer -= Time.deltaTime;

            Time.timeScale = 10;

            if(timeSpeedTimer <= 0)
            {
                timeSpeedTimer = timeSpeedTimerMax;
                Time.timeScale = 1;
                timeSpeedOn = false;
            }
        }
    }
}
