using TMPro;
using UnityEngine;

public class AdOfferScript : MonoBehaviour
{
    public PlayerConfig playerConfig;
    public float specialMoneyReward;
    public TMP_Text valueText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnEnable()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        specialMoneyReward = playerConfig.moneySpecialMult * 50;

        if (specialMoneyReward > 1000000000)
        {
            valueText.text = "GANHE " + "Mais de BILHÃO" + " pontos assistindo uma propaganda" ;
        }
        else if (specialMoneyReward > 1000000)
        {
            specialMoneyReward -= 1000000;
            valueText.text = "GANHE " + specialMoneyReward.ToString("+0 M") + " pontos assistindo uma propaganda" ;
        }
        else if (specialMoneyReward > 1000)
        {
            specialMoneyReward -= 1000;
            valueText.text = "GANHE " + specialMoneyReward.ToString("+0 K") + " pontos assistindo uma propaganda" ;
        }
        else
        {
            valueText.text = "GANHE " + specialMoneyReward.ToString("+0") + " pontos assistindo uma propaganda" ;
        }

    }


}
