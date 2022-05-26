using UnityEngine;
using TMPro;
using System;

public class UpdaterGetExtraCoin : MonoBehaviour
{
    private TMP_Text buttonText = null;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponent<TMP_Text>();

        CoinsManager.onGetExtraCoinResetTime += UpdateText;
    }
    private void OnDestroy() {
        CoinsManager.onGetExtraCoinResetTime -= UpdateText;
    }

    private void UpdateText()
    {
        buttonText.text = "Claim";
    }
}
