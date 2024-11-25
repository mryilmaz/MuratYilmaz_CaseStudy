using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event Action onLevelFinished,onLevelStart,coinIncrement;
    private int coinAmount;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    private void Start()
    {
        OnLevelStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onLevelFinished?.Invoke();
            CoinAmount += 5;
        }
    }


    public void OnLevelFinished()
    {
        onLevelFinished?.Invoke();
    }

    public void OnLevelStart()
    {
        onLevelStart?.Invoke();
    }

    public int CoinAmount
    {
        get => coinAmount;

        set
        {
            coinAmount = value;
            coinIncrement?.Invoke();
        }
    }
}