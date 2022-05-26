using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    BlockingWait blockingWaitPopup;

    private void Awake()
    {
        
    }

    // called from UI
    public void Action_SpendOneCoin()
    {
        Debug.Log("MainMenu:Action_SpendOneCoin");
    }

    // called from UI
    public void Action_GetExtraCoin()
    {
        Debug.Log("MainMenu:Action_GetExtraCoin");
        blockingWaitPopup.gameObject.SetActive(true);
    }

    // called from UI
    public void Action_ClaimFreeCoin()
    {
        Debug.Log("MainMenu:Action_ClaimFreeCoin");
    }
}
