using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdaterGambleButtonImageUpdater : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.green;
    [SerializeField] private Color selectColor = Color.red;

    [Header("Auto-Set - visible for debug")]
    [SerializeField] private Image currentImage = null;
    [SerializeField] private TMP_Text currentText = null;
    [SerializeField] private int buttonNo = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentImage = GetComponent<Image>();
        currentText = GetComponentInChildren<TMP_Text>();
        buttonNo = int.Parse(currentText.text);
        GambleManager.onGambleNoChosen += CheckHighlightButton;
    }
    private void OnDestroy() {
        GambleManager.onGambleNoChosen -= CheckHighlightButton;
    }

    private void CheckHighlightButton(int chosenGambleNo)
    {
        if (chosenGambleNo == buttonNo) {
            currentImage.color = selectColor;
        } else {
            if (currentImage.color != defaultColor) {
                currentImage.color = defaultColor;
            }
        }
    }

    public void SetClickedColor() {
        currentImage.color = selectColor;
    }
}
