using UnityEngine;

public class DisableWithAnimation : MonoBehaviour
{

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }


    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
