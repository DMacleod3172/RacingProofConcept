using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrialTimer : MonoBehaviour
{
    public float currentTime = 0f;
    public float checkpointBonus = 10f;

    public Text timerText;

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TimerText is not assigned!");
        }
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerUI();
    }

    public void AddCheckpointTime()
    {
        currentTime += checkpointBonus;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + currentTime.ToString("F2");  // Display time with 2 decimal places
    }
}


