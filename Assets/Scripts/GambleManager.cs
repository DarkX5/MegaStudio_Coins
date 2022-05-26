using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GambleManager : MonoBehaviour
{
    // Action< selectedGambleNo >
    public static event Action<int> onGambleNoChosen = null;
    // Action <gambled coins>
    public static event Action<int> onGambleFinished = null;
    [SerializeField] private float gambleSelectionDisableTime = 3f;
    [SerializeField] private GambleResultTypes[] gambleWinList;
    [SerializeField] private float gambleWinChance = 5f;

    [Header("Auto-Set - visible for debug")]
    [SerializeField] private int selectedGamble = 0;
    [SerializeField] private int selectedGambleNo = 0;
    [SerializeField] private int currentGambleTurn = 0;
    [SerializeField] private Canvas gambleCanvas = null;

    public void Init()
    {
        // init buttons
        onGambleNoChosen?.Invoke(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        gambleCanvas = GetComponent<Canvas>();
        // start canvas as disabled (keeps script alive)
        gambleCanvas.enabled = false;
        // gameObject.SetActive(false);

        // subscribe to gamble changes
        UpdaterGambleCanvas.onGambleValueChanged += SetGamble;
    }
    private void OnDestroy()
    {
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

    public void Gamble(int chosenGambleAmount, int chosenGambleNo)
    {
        // avoid IndexOutOfRange errors
        GambleResultTypes grt;
        if (currentGambleTurn < gambleWinList.Length)
        {
            grt = gambleWinList[currentGambleTurn];
        }
        else
        {
            grt = gambleWinList[gambleWinList.Length - 1];
        }

        int gambleCoinsValue = 0;
        // check victory/loss
        switch (grt)
        {
            case GambleResultTypes.Win:
                gambleCoinsValue = chosenGambleAmount;
                // call button ui subscribers
                onGambleNoChosen?.Invoke(chosenGambleNo);
                // Debug.Log("Win - " + chosenGambleNo);
                break;
            case GambleResultTypes.Lose:
                gambleCoinsValue = -chosenGambleAmount;
                // call button ui subscribers
                onGambleNoChosen?.Invoke(GetLostGambleChoice(chosenGambleNo));
                Debug.Log("Lose - " + chosenGambleNo);
                break;
            case GambleResultTypes.DefaultChance:
                int random = UnityEngine.Random.Range((int)0, (int)101);
                if (random > gambleWinChance)
                {
                    // lose
                    gambleCoinsValue = -chosenGambleAmount;
                    // call button ui subscribers
                    onGambleNoChosen?.Invoke(GetLostGambleChoice(chosenGambleNo));
                }
                else
                {
                    // win
                    gambleCoinsValue = chosenGambleAmount;
                    // call button ui subscribers
                    onGambleNoChosen?.Invoke(chosenGambleNo);
                }
                Debug.Log("Default - " + chosenGambleNo);
                break;
        }

        onGambleFinished?.Invoke(gambleCoinsValue);

        // next gamble turn
        currentGambleTurn += 1;

        FinishGamble();
    }
    public void FinishGamble()
    {
        // update UI (disable after gambleSelectionDisableTime seconds)
        StartCoroutine(ShowGambleResultCO());
    }

    private void SetGamble(int newGambleValue)
    {
        selectedGamble = newGambleValue;
    }

    private IEnumerator ShowGambleResultCO()
    {
        yield return new WaitForSecondsRealtime(gambleSelectionDisableTime);
        // disable canvas after delay (keeps script alive)
        gambleCanvas.enabled = false;
    }

    private int GetLostGambleChoice(int chosenGambleNo)
    {
        if (chosenGambleNo == 1)
        {
            return UnityEngine.Random.Range(chosenGambleNo + 1, 10);
        }
        if (chosenGambleNo == 10)
        {
            return UnityEngine.Random.Range(1, chosenGambleNo - 1);
        }

        int random = UnityEngine.Random.Range(0, 2);
        switch (random)
        {
            case 0:
                return UnityEngine.Random.Range(1, chosenGambleNo - 1);
            case 1:
                return UnityEngine.Random.Range(chosenGambleNo + 1, 10);
        }

        // default value
        return 1;
    }
}
