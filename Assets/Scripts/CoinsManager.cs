// using System.Collections;
// using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class CoinsManager : MainMenu, ICoins
{
    public static event Action onGetFreeCoinResetTime = null;
    public static event Action onGetFreeCoinTimeElapsed = null;
    public static event Action<int> onCoinsValueChanged = null;
    [SerializeField] private int maxCoins = 10;
    [SerializeField] private int noOfFreeCoinsClaimed = 1;
    [SerializeField] private int maxExtraCoinsPerDay = 5;
    [SerializeField] private double secondsPerDay = 86400;
    [SerializeField] private string coinsSaveFileName = "Coins.txt";
    [SerializeField] private string freeCoinCollectedTimeFileName = "CollectTime.txt";
    [SerializeField] private string extraCoinUsesFileName = "ExtraCoinUses.txt";
    [SerializeField] private string extraCoinCollectedTimeFileName = "ExtraCoinCollectTime.txt";
    [SerializeField] private ResetTime freeCoinsTime = new ResetTime(13, 0, 0);
    // [SerializeField] private GameResultTypes[] gambleWinList;
    private DateTime lastCollectedTime;
    private DateTime lastExtraCollectedTime;
    private double timeLeft;
    private bool canClaimFreeCoin = false;
    private int noOfExtraCoinsClaimed = 0;
    // private int currentGambleTurn = 0;

    private Coroutine ct = null;
    public int coins { get; set; }

    public int MaxCoins { get { return maxCoins; } }

    // Start is called before the first frame update
    void Start()
    {
        // start with a small delay to wait for everything else to load
        Invoke("Initialize", 1f);

        // GambleManager.onGambleNoChosen += Gamble;
        GambleManager.onGambleFinished += SetGambleCoins;
    }
    private void OnDestroy()
    {
        // GambleManager.onGambleNoChosen -= Gamble;
        GambleManager.onGambleFinished -= SetGambleCoins;

        // stop running the coroutine on exit
        StopAllCoroutines();
    }

    public void Initialize() {
        // load saved coins
        string tempTxt = IOUtility.LoadFromDisk(coinsSaveFileName);

        // check saved coins exist
        if (tempTxt == string.Empty) {
            // set coins starting value
            coins = 5;
            SaveCoins();
        } else {
            // get coins value
            coins = int.Parse(tempTxt);
        }
        // update any subscribers to saved/starting coin values
        onCoinsValueChanged?.Invoke(coins);

        // load extra coin uses
        tempTxt = IOUtility.LoadFromDisk(extraCoinUsesFileName);

        // check saved extra coins exist
        if (tempTxt == string.Empty)
        {
            // set coins starting value
            noOfExtraCoinsClaimed = 0;
            SaveExtraCoinUses();
        }
        else
        {
            // get extra coin uses
            noOfExtraCoinsClaimed = int.Parse(tempTxt);
        }

        // load last collected time
        tempTxt = IOUtility.LoadFromDisk(freeCoinCollectedTimeFileName);
        if (tempTxt == string.Empty) {
            lastCollectedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            // save last collected time as Beginning-Of-Time
            SaveLastCoinCollectionTime();
        } else {
            // load last collected time
            lastCollectedTime = DateTime.Parse(tempTxt);
        }

        // load extra last collected time
        tempTxt = IOUtility.LoadFromDisk(extraCoinCollectedTimeFileName);
        if (tempTxt == string.Empty)
        {
            lastExtraCollectedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            // reset claimed coins
            noOfExtraCoinsClaimed = 0;
            // save last extra collected time as Beginning-Of-Time
            SaveExtraCoinUses();
        }
        else
        {
            // load last extra collected time
            lastExtraCollectedTime = DateTime.Parse(tempTxt);
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
        double elapsedExtraTime = (cTime - lastExtraCollectedTime).TotalSeconds;

        // check if enough time elapsed for a free coin
        if (elapsedTime > secondsPerDay)
        {
            canClaimFreeCoin = true;
            // Debug.Log("reset time elapsed");

            // update coins (invoke subscribers)
            onGetFreeCoinTimeElapsed?.Invoke();
        } else {
            canClaimFreeCoin = false;
        }

        // check if enough time elapsed for extra coins
        if (elapsedExtraTime > secondsPerDay)
        {
            noOfExtraCoinsClaimed = 0;
            SaveExtraCoinUses();
            Debug.Log("reset extra time elapsed");
        } else {
            canClaimFreeCoin = false;
        }
        
        ct = StartCoroutine(CheckTimeElapsedCO());
    }

    // called from UI
    public void GetFreeCoins()
    {
        // do nothing once reaching maxCoins (default: 10)
        if (coins >= maxCoins) { return; }
        // return if coin already collected
        if (canClaimFreeCoin == false) { return; }

        // get free coin(s)
        coins += noOfFreeCoinsClaimed;

        // set last collected as today
        lastCollectedTime = new DateTime(TimerUtility.CurrentTime.Year, TimerUtility.CurrentTime.Month, TimerUtility.CurrentTime.Day,
                                        freeCoinsTime.Hours, freeCoinsTime.Minutes, freeCoinsTime.Seconds);

        SaveLastCoinCollectionTime();
        SaveCoins();

        // give coin (invoke subscribers)
        onGetFreeCoinResetTime?.Invoke();

        // call coin value change subscribers
        onCoinsValueChanged?.Invoke(coins);
    }
    // called from UI
    public void GetExtraCoins() {
        // return if all extra coins already collected
        if (noOfExtraCoinsClaimed >= maxExtraCoinsPerDay) { return; }

        // get extra coin
        coins += 1;
        SaveCoins();

        // update no of extra coin uses
        noOfExtraCoinsClaimed += 1;
        SaveExtraCoinUses();

        // update coins (invoke subscribers)
        onCoinsValueChanged?.Invoke(coins);

        Debug.Log("Get Extra Coins - " + noOfExtraCoinsClaimed);
    }
    // called from UI
    public void SpendOneCoin()
    {
        // check if enough coins to spend
        if (coins <= 0)
        {
            // set coins value to 0 to make sure it never displays less than 0
            coins = 0;
            return;
        }
        else
        {
            coins -= 1;
        }
        SaveCoins();

        onCoinsValueChanged?.Invoke(coins);
        Debug.Log("SpendOneCoin");
    }

    private void SetGambleCoins(int coinsToAdd) {
        coins += coinsToAdd;

        SaveCoins();

        onCoinsValueChanged?.Invoke(coins);
        Debug.Log("SpendOneCoin");
    }

#region Helpers
    private void SaveExtraCoinUses()
    {
        // save new coins value
        IOUtility.SaveToDisk(extraCoinUsesFileName, noOfExtraCoinsClaimed.ToString());
        // save new last extra coin collection time
        IOUtility.SaveToDisk(extraCoinCollectedTimeFileName, lastExtraCollectedTime.ToString());
    }
    private void SaveCoins()
    {
        // save new coins value
        IOUtility.SaveToDisk(coinsSaveFileName, coins.ToString());
    }
    private void SaveLastCoinCollectionTime()
    {
        // save new last collected time
        IOUtility.SaveToDisk(freeCoinCollectedTimeFileName, lastCollectedTime.ToString());
    }
#endregion
}
