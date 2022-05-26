// using System.Collections;
// using System.Collections.Generic;
using System;
using UnityEngine;

public class CoinsManager : MonoBehaviour, ICoins
{
    public static event Action onGetExtraCoinResetTime = null;
    [SerializeField] private int maxCoins = 10;
    [SerializeField] private double secondsPerDay = 86400;
    [SerializeField] private TimeSpan freeCoinsTime = new TimeSpan(13, 0, 0);
    private string coinsSaveFileName = "Coins.txt";
    private bool lastFreeCoinCollected = false;

    public int coins { get; set; }

    public void Initialize() {
        // load saved coins
        string savedCoins = IOUtility.LoadFromDisk(coinsSaveFileName);

        // check saved coins exist
        if (savedCoins == string.Empty) {
            // set coins starting value
            coins = 5;
            IOUtility.SaveToDisk(coinsSaveFileName, "5");
        } else {
            // get coins value
            coins = int.Parse(savedCoins);
        }
    }
    public void GetFreeCoins() {
        // do nothing once reaching maxCoins (default: 10)
        if (coins >= maxCoins) { return; }

        var cTime = TimerUtility.CurrentTime;
        
        // get today @freeCoinsTime (default: 13:00:00)
        var yTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 
                                 freeCoinsTime.Hours, freeCoinsTime.Minutes, freeCoinsTime.Seconds);
        // decrease by one day (less errors than building it manually - takes everything into account -> month, year, leap year, etc)
        yTime = yTime.AddDays(-1);

        // get elapsed time since yesterday @freeCoinsTime (default: 13:00:00)
        double elapsedTime = (cTime - yTime).TotalSeconds;

        // check if enough time elapsed for a free coin
        if (elapsedTime >= secondsPerDay) {
            // give coin (invoke subscribers)
            onGetExtraCoinResetTime?.Invoke();
        }
    }
    public void GetExtraCoins() {
        Debug.Log("Get Extra Coins");
    }
    public void Gamble() {
        Debug.Log("Gamble");
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
