using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PromotionalCodeScript : MonoBehaviour
{
    public TMP_InputField promotionalText;
    public UnityEvent[] promotionalCodeEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfirmPromotionalCode()
    {
        if(promotionalText.text == "T.A.M.M")
        {
            promotionalCodeEvent[0]?.Invoke();
            promotionalText.text = "";
        }
    }
}
