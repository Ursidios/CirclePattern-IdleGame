using System.Threading;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public CosmeticUpgradesManager cosmeticUpgrades;
    public GameObject finalScreen;
    public float Timer;

    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            if (upgradeManager.allUpgradesInMax && cosmeticUpgrades.allUpgradesInMax)
            {
                finalScreen.SetActive(true);
            }
        }
    }
}
