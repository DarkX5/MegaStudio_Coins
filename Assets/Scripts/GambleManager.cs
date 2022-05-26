using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GambleManager : MonoBehaviour
{
    // // Action< selectedGamble, selectedGambleNo >
    // public static event Action<int, int> onGambleNoChosen = null;
    // Action <gambled coins>
    public static event Action<int> onGambleFinished = null;
    [SerializeField] private float gambleSelectionDisableTime = 3f;
    [SerializeField] private GameResultTypes[] gambleWinList;
    [SerializeField] private float gambleWinChance = 5f;
    
    [Header("Auto-Set - visible for debug")]
    [SerializeField] private int selectedGamble = 0;
    [SerializeField] private int selectedGambleNo = 0;
    [SerializeField] private int currentGambleTurn = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // start canvas as disabled (keeps script alive)
        GetComponent<Canvas>().enabled = false;
        // gameObject.SetActive(false);

        // subscribe to gamble changes
        UpdaterGambleCanvas.onGambleValueChanged += SetGamble;
    }
    private void OnDestroy() {
        // subscribe to gamble changes
        UpdaterGambleCanvas.onGambleValueChanged -= SetGamble;
    }

    // called from UI
    public void ChooseGambleNo(TMP_Text activatedButtonText)
    {
        // use int for param in case the button changes to images numbers
        selectedGambleNo = int.Parse(activatedButtonText.text);

        Gamble(selectedGamble, selectedGambleNo);
        // Debug.Log("ChooseGambleNo - " + activatedButtonText.text);
    }

    public void Gamble(int chosenGambleAmount, int chosenGambleNo) {
        // avoid IndexOutOfRange errors
        GameResultTypes grt;
        if (currentGambleTurn < gambleWinList.Length) {
            grt = gambleWinList[currentGambleTurn];
        } else {
            grt = gambleWinList[gambleWinList.Length - 1];
        }

        int gambleCoinsValue = 0;
        // check victory/loss
        switch(grt) {
            case GameResultTypes.Win:
                gambleCoinsValue = chosenGambleAmount;
                Debug.Log("Win - " + gambleCoinsValue);
                break;
            case GameResultTypes.Lose:
                gambleCoinsValue = -chosenGambleAmount;
                Debug.Log("Lose - " + gambleCoinsValue);
                break;
            case GameResultTypes.DefaultChance:
                int random = UnityEngine.Random.Range((int)0, (int)101);
                if (random > gambleWinChance) {
                    // lose
                    gambleCoinsValue = -chosenGambleAmount;
                } else {
                    // win
                    gambleCoinsValue = chosenGambleAmount;
                }
                Debug.Log("Default - " + gambleCoinsValue);
                break;
        }

        onGambleFinished?.Invoke(gambleCoinsValue);

        // next gamble turn
        currentGambleTurn += 1;
        Debug.Log("GambleMan - Gamble" + currentGambleTurn.ToString());
    }

    private void SetGamble(int newGambleValue) {
        Debug.Log("Set Gamble - " + newGambleValue);
        selectedGamble = newGambleValue;
    }

    private IEnumerator ShowGambleResultCO() {

        yield return new WaitForSecondsRealtime(gambleSelectionDisableTime);
    }
}
