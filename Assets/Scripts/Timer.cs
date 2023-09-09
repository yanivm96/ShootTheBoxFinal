using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text boxesLeftText;

    private bool isRunning = false;
    private float startTime;
    private float duration;
    private float stoppedDuration;

    public delegate void TimerDoneEventHandler();
    public event TimerDoneEventHandler OnTimerDone;

    private void Start()
    {
        GameManager.Instance.Timer_ = this;
    }

    void Update()
    {
        if (isRunning)
        {
            float remainingTime = duration - (Time.time - startTime);
            remainingTime = Mathf.Max(0f, remainingTime);
            DisplayTime(remainingTime);

            if (remainingTime <= 0f)
            {
                StopTimer();
                if (OnTimerDone != null)
                {
                    OnTimerDone();
                }
            }
        }
    }

    private void DisplayTime(float time)
    {
        string formattedTime = time.ToString("F2");
        timerText.text = formattedTime;
        if (time < 5f)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    public void StartTimer(float seconds)
    {
        timerText.color = Color.white;
        if (!isRunning)
        {
            duration = seconds;
            startTime = Time.time;
            isRunning = true;
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            isRunning = false;
            stoppedDuration = (Time.time - startTime);
            stoppedDuration = Mathf.Max(0f, stoppedDuration);
        }
    }

    public float GetStoppedDuration()
    {
        return stoppedDuration;
    }

    public void UpdateBoxesLeftText(int numOfBoxesLeft)
    {
        boxesLeftText.text = "Boxes Left: " + numOfBoxesLeft.ToString();
    }
}
