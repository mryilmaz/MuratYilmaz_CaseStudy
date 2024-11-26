using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event Action onLevelFinished,onLevelStart,coinIncrement,onPlayerFail;
    private int coinAmount;
    private int failCount;

    private void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(1080, 1920, false);
        Screen.fullScreen = false;
#endif
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  
        }
        failCount = PlayerPrefs.GetInt("FailCount", 0); 
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

        if (Input.GetKeyDown(KeyCode.F))
        {
           OnPlayerFail();
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
    public void OnPlayerFail()
    {
        failCount++;  
        PlayerPrefs.SetInt("FailCount", failCount);  
        PlayerPrefs.Save();  
        onPlayerFail?.Invoke(); 
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
    public int FailCount
    {
        get => failCount;
    }
}