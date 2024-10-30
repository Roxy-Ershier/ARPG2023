using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerState
{
    NOTWORKING,//没在工作
    WORKING,//工作中
    DONE//工作完
}

public class GameTimer
{
    private float _runTime;
    private Action _task;
    private bool _isRun;
    private TimerState _state;
    
    

    public GameTimer()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        _runTime = 0;
        _task = null;
        _isRun = false;
        _state = TimerState.NOTWORKING;
    }

    public void StartTimer(float time,Action task)
    {
        _runTime = time;
        _task = task;
        _isRun = true;
        _state = TimerState.WORKING;
    }


    public void UpdateTimer()
    {
        if (!_isRun) return;

        if (_runTime > 0)
        {
            _runTime -= Time.deltaTime;
        }
        else
        {
            _task?.Invoke();
            _isRun = false;
            _state = TimerState.DONE;
        }
    }

    public TimerState GetTimerState() => _state;

}
