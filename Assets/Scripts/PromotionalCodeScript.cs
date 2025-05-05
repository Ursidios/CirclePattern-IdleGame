using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PromotionalCodeScript : MonoBehaviour
{
    public TMP_InputField promotionalText;
    public Codes[] codes;
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
        for (int i = 0; i < codes.Length; i++)
        {
            if(promotionalText.text == codes[i].code)
            {
                codes[i].promotionalCodeEvent?.Invoke();
                promotionalText.text = "";
            } 
        }

    }
}
[Serializable]
public class Codes
{
    public UnityEvent promotionalCodeEvent;
    public string code;
}
