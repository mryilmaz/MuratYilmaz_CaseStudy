using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject paintingPanel;
    [SerializeField] private TextMeshProUGUI coinText; 
    void Awake()
    {
        GameManager.instance.onLevelStart += OnLevelStart;
        GameManager.instance.onLevelFinished += OnLevelFinished;
        GameManager.instance.coinIncrement += ChangeCoinText;
    }


    private void OnLevelStart()
    {
        paintingPanel.SetActive(false);
    }
    private void OnLevelFinished()
    {
        paintingPanel.SetActive(true);
    }

    private void ChangeCoinText()
    {
        coinText.transform.DOPunchScale(Vector3.one*.5f, 0.3f,  10, 0.2f);
        coinText.text = "x" + GameManager.instance.CoinAmount.ToString();
    }
}
