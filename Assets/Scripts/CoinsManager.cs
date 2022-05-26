// using System.Collections;
// using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class CoinsManager : MainMenu, ICoins
{
    public static event Action onGetExtraCoinResetTime = null;
    public static event Action onGetExtraCoinTimeElapsed = null;
    public static event Action<int> onCoinsClaimed = null;
    [SerializeField] private int maxCoins = 10;
    [SerializeField] private int noOfCoinsClaimed = 1;
    [SerializeField] private double secondsPerDay = 86400;
    [SerializeField] private string coinsSaveFileName = "Coins.txt";
    [SerializeField] private string freeCoinCollectedTimeFileName = "CollectTime.txt";
    [SerializeField] private ResetTime freeCoinsTime = new ResetTime(13, 0, 0);
    private DateTime lastCollectedTime;
    private double timeLeft;
    private bool canClaimFreeCoin = false;

    private Coroutine ct = null;
    public int coins { get; set; }

    public int MaxCoins { get { return maxCoins; } }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    private void OnDestroy()
    {
        // stop running the coroutine on exit
        StopCoroutine(ct);

        // // use this if above gives errors || breaks
        // StopAllCoroutines();
    }

    public void Initialize() {
        // load saved coins
        string tempTxt = IOUtility.LoadFromDisk(coinsSaveFileName);

        // check saved coins exist
        if (tempTxt == string.Empty) {
            // set coins starting value
            coins = 5;
            IOUtility.SaveToDisk(coinsSaveFileName, "5");
        } else {
            // get coins value
            coins = int.Parse(tempTxt);
        }

        // update any subscribers to saved/starting coin values
        onCoinsClaimed?.Invoke(coins);

        // load last collected time
        tempTxt = IOUtility.LoadFromDisk(freeCoinCollectedTimeFileName);
        if (tempTxt == string.Empty) {
            lastCollectedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            // save last collected time as Beginning-Of-Time
            IOUtility.SaveToDisk(freeCoinCollectedTimeFileName, lastCollectedTime.ToString());
        } else {
            // load last collected time
            lastCollectedTime = DateTime.Parse(tempTxt);
        }

        // start checking for free coins time
        ct = StartCoroutine(CheckTimeElapsedCO());
    }

    private IEnumerator CheckTimeElapsedCO() {
        // check every second if Free Coin time elapsed
        yield return new WaitForSecondsRealtime(1f);

        var cTime = TimerUtility.CurrentTime;

        // get elapsed time since yesterday @freeCoinsTime (default: 13:00:00)
        double elapsedTime = (cTime - lastCollectedTime).TotalSeconds;

        // check if enough time elapsed for a free coin
        if (elapsedTime >= secondsPerDay)
        {
            canClaimFreeCoin =  true;
            // give coin (invoke subscribers)
            onGetExtraCoinTimeElapsed?.Invoke();
        } else {
            canClaimFreeCoin = false;
        }

        ct = StartCoroutine(CheckTimeElapsedCO());
    }

    // called from UI
    public void GetFreeCoins() {
        // do nothing once reaching maxCoins (default: 10)
        if (coins >= maxCoins) { return; }
        // return if coin already collected
        if (canClaimFreeCoin == false) { return; }

        // get free coin(s)
        coins += noOfCoinsClaimed;

        // set last collected as today
        lastCollectedTime = new DateTime(TimerUtility.CurrentTime.Year, TimerUtility.CurrentTime.Month, TimerUtility.CurrentTime.Day,
                                        freeCoinsTime.Hours, freeCoinsTime.Minutes, freeCoinsTime.Seconds);

        // save new last collected time
        IOUtility.SaveToDisk(freeCoinCollectedTimeFileName, lastCollectedTime.ToString());
        // save new coins value
        IOUtility.SaveToDisk(coinsSaveFileName, coins.ToString());
        
        // give coin (invoke subscribers)
        onGetExtraCoinResetTime?.Invoke();

        // call coin value change subscribers
        onCoinsClaimed?.Invoke(coins);
    }

    // called from UI
    public void GetExtraCoins() {
        Debug.Log("Get Extra Coins");
    }
    // called from UI
    public void Gamble() {
        Debug.Log("Gamble");
    }
}
