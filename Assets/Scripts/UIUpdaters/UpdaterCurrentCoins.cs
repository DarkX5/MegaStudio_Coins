using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdaterCurrentCoins : MonoBehaviour
{
    [Header("Auto-Set - visible for debug")]
    [SerializeField] private TMP_Text text = null;
    [SerializeField] private int maxCoins = 10;
    [SerializeField] private int currentCoins = 5;

    private string baseText = "Coins: {current}/{max}";

    // Start is called before the first frame update
    void Start()
    {
        // setup script data
        if (text == null) {
            text = GetComponent<TMP_Text>();
        }
        // get initial text for further replacements
        baseText = text.text;
        // get Coins Manager Data
        var cMan = FindObjectOfType<CoinsManager>();
        maxCoins = cMan.MaxCoins;
        currentCoins = cMan.MaxCoins;

        CoinsManager.onCoinsClaimed += CoinsTextUpdate;
    }
    private void OnDestroy() {
        CoinsManager.onCoinsClaimed -= CoinsTextUpdate;
    }

    private void CoinsTextUpdate(int newCoinsValue)
    {
        currentCoins = newCoinsValue;
        // get current coins text values
        var txt = baseText.Replace("{current}", currentCoins.ToString())
                            .Replace("{max}", maxCoins.ToString());
        text.text = txt;
    }
}
