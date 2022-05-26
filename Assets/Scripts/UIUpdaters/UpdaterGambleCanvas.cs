using System;
using UnityEngine;
using TMPro;

public class UpdaterGambleCanvas : MonoBehaviour
{
    public static event Action<int> onGambleValueChanged = null;
    [SerializeField] private TMP_Text gambleText = null;
    [SerializeField] private GameObject gambleButton = null;
    [SerializeField] private int maxGambleAmount = 10;
    [Header("Auto-Set - visible for debug")]
    [SerializeField] private int gambleAmount = 1;

    private int currentCoins = 0;

    private void OnEnable()
    {
        // reset gamble amount at every start
        gambleAmount = 1;
        
        // call subscribers
        onGambleValueChanged?.Invoke(gambleAmount);
        
        if (gambleText == null)
        {
            Debug.Log("Gamble Text needs to be set");
        }
        else
        {
            UpdateGambleText();
        }

        if (gambleButton == null)
        {
            Debug.Log("Gamble Button needs to be set");
        }
        else
        {
            UpdateGambleButton();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // load coins value from disk (seems to have a delay at start & doesn't update)
        LoadCoins();

        CoinsManager.onCoinsValueChanged += SetCurrentCoins;
    }
    private void OnDestroy()
    {
        CoinsManager.onCoinsValueChanged -= SetCurrentCoins;
    }

    private void SetCurrentCoins(int newCurrentCoins)
    {
        currentCoins = newCurrentCoins;
        UpdateGambleText();
        UpdateGambleButton();
    }

    // called from UI 
    public void IncreaseGamble()
    {
        if (gambleAmount < maxGambleAmount && gambleAmount < currentCoins)
        {
            gambleAmount += 1;
        }

        // call subscribers
        onGambleValueChanged?.Invoke(gambleAmount);

        UpdateGambleText();
    }
    // called from UI
    public void DecreaseGamble()
    {
        if (gambleAmount > 1)
        {
            gambleAmount -= 1;
        }

        // call subscribers
        onGambleValueChanged?.Invoke(gambleAmount);

        UpdateGambleText();
    }

    #region Helpers
    private void UpdateGambleText()
    {
        gambleText.text = gambleAmount.ToString();
    }
    private void UpdateGambleButton()
    {
        if (currentCoins < 1)
        {
            gambleButton.SetActive(false);
        }
        else
        {
            gambleButton.SetActive(true);
        }
    }
    private void LoadCoins() {
        // load saved coins
        string tempTxt = IOUtility.LoadFromDisk("Coins.txt");

        // check saved coins exist
        if (tempTxt == string.Empty)
        {
            // set coins starting value
            SetCurrentCoins(5);
        }
        else
        {
            // get coins value
            SetCurrentCoins(int.Parse(tempTxt));
        }

    }
    #endregion
}
