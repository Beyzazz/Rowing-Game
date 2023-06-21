using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private static Timer _ins;
    public static Timer Ins
    {
        get
        {
            if (!_ins)
                _ins = FindObjectOfType<Timer>();
            return _ins;
        }
    }

    public TMP_Text timeText;
    private float _elapsedTime = 0f;
    private float _bestTimeWASD
    {
        get
        {
            return PlayerPrefs.GetFloat("bestTimeWASD", Mathf.Infinity);
        }
        set
        {
            PlayerPrefs.SetFloat("bestTimeWASD", value);
        }
    }

    private float _bestTimeArrows
    {
        get
        {
            return PlayerPrefs.GetFloat("bestTimeArrows", Mathf.Infinity);
        }
        set
        {
            PlayerPrefs.SetFloat("bestTimeArrows", value);
        }
    }
    private bool _isRunning = false;

    private void Start()
    {
        DisplayTime();
    }

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
            DisplayTime();
        }
    }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void DisplayTime()
    {
        timeText.text = GetTime();
    }

    public string GetTime()
    {
        ConvertTime(_elapsedTime, out int minutes, out int seconds, out int milliseconds);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void SetBestTime(bool isKeyboard)
    {
        if (isKeyboard)
        {
            if (_bestTimeWASD == Mathf.Infinity)
                _bestTimeWASD = _elapsedTime;
            else if(_bestTimeWASD > _elapsedTime)
                _bestTimeWASD = _elapsedTime;
        }
        if (!isKeyboard)
        {
            if (_bestTimeArrows == Mathf.Infinity)
                _bestTimeArrows = _elapsedTime;
            else if (_bestTimeArrows > _elapsedTime)
                _bestTimeArrows = _elapsedTime;
        }
    }

    public string GetBestTimeWASD()
    {
        if (_bestTimeWASD == Mathf.Infinity) return "-";
        ConvertTime(_bestTimeWASD, out int minutes, out int seconds, out int milliseconds);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public string GetBestTimeArrows()
    {
        if (_bestTimeArrows == Mathf.Infinity) return "-";
        ConvertTime(_bestTimeArrows, out int minutes, out int seconds, out int milliseconds);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void ConvertTime(float time, out int minutes, out int seconds, out int milliseconds)
    {
        minutes = (int)(time / 60);
        seconds = (int)(time % 60);
        milliseconds = (int)((time % 1) * 100);
    }
}
