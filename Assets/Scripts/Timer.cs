using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f;
    private bool isRunning = true;

    private void Start()
    {
        GameManager.instance.onLevelFinished += ToggleTimer;
    }

    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;
            DisplayTime(timeElapsed);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60f);
        int milliseconds = Mathf.FloorToInt((timeToDisplay * 1000f) % 1000f) / 10;

        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        timerText.text = formattedTime;
    }

    private void ToggleTimer()
    {
        isRunning = !isRunning;
    }

    private void ResetTimer()
    {
        timeElapsed = 0f;
        DisplayTime(timeElapsed);
    }
}