using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    TextMeshProUGUI timerText;
    public float absoluteTimerValue = 0f;
    public float maxTimer = 120f;

    void Start() {
        timerText = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("CalculateTimer", 1f, 1f);
    }

    void CalculateTimer()
    {
        absoluteTimerValue++;

        var minutes = absoluteTimerValue / 60f;
        var minutesFloored = Mathf.Floor(minutes);

        var seconds = (minutes - minutesFloored) * 60f;

        if(seconds >= 10)
            timerText.text = $"{minutesFloored}:{seconds}";
        else
            timerText.text = $"{minutesFloored}:0{seconds}";
    }

    public float GetNormalizedValue() => absoluteTimerValue / maxTimer;
}
