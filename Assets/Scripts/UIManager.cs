using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject paintingPanel,uiJoystick;
    [SerializeField] private TextMeshProUGUI coinText,failText;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.onLevelStart += OnLevelStart;
        gameManager.onLevelFinished += OnLevelFinished;
        gameManager.coinIncrement += ChangeCoinText;
        gameManager.onPlayerFail += UpdateFailText;
    }


    private void OnLevelStart()
    {
        paintingPanel.SetActive(false);
        UpdateFailText();
    }
    private void OnLevelFinished()
    {
        paintingPanel.SetActive(true);
        uiJoystick.SetActive(false);
    }

    private void ChangeCoinText()
    {
        DOTween.Complete("coinPunch");
        coinText.transform.DOPunchScale(Vector3.one*.5f, 0.3f,  10, 0.2f).SetId("coinPunch");
        coinText.text = "x" + GameManager.instance.CoinAmount.ToString();
    }

    private void UpdateFailText()
    {
        failText.text="Failed "+GameManager.instance.FailCount.ToString()+" times";
    }
}
