using UnityEngine;
using TMPro;
using System;

public class UpdaterGetExtraCoin : MonoBehaviour
{
    private TMP_Text buttonText = null;
    private string defaultText = "Get Extra Coin";
    private string claimText = "Claim";

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponent<TMP_Text>();
        // set current button text as default
        defaultText = buttonText.text;

        CoinsManager.onGetFreeCoinTimeElapsed += UpdateTextToClaim;
        CoinsManager.onGetFreeCoinResetTime += UpdateTextToDefault;
    }
    private void OnDestroy() {
        CoinsManager.onGetFreeCoinTimeElapsed -= UpdateTextToClaim;
        CoinsManager.onGetFreeCoinResetTime -= UpdateTextToDefault;
    }

    private void UpdateTextToClaim()
    {
        if (buttonText.text != claimText) {
            buttonText.text = claimText;
            Debug.Log(buttonText.text);
        }
    }

    // called from UI
    public void UpdateTextToDefault()
    {
        if (buttonText.text != defaultText) {
            buttonText.text = defaultText;
            Debug.Log(buttonText.text);
        }
    }
}
