using UnityEngine;

public class DrawingSelectorButtonScript : MonoBehaviour
{
    public int ID;
    public bool isSelected;
    private PlayerConfig playerConfig;

    public void Start()
    {
        playerConfig = FindObjectOfType<PlayerConfig>();
        isSelected = true;
    }


    public void ActionOnClick()
    {
        isSelected = !isSelected;
        //playerConfig.ChangeDrawCircleEnabledState(ID, isSelected);
    }
}
