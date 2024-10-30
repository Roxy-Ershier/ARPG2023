using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGG.Tool.Singleton;
using System;
using UnityEngine.UIElements;

public class TimerManager :Singleton<TimerManager>
{
    [SerializeField] private int _maxTimerCount=20;

    private Queue<GameTimer> _notWorkTimer = new Queue<GameTimer>();

    private List<GameTimer> _workingTimer = new List<GameTimer>();


    private void Start()
    {
        InitTimerManager();
    }

    private void Update()
    {
        UpdateWorkingTimer();
    }


    private void UpdateWorkingTimer()
    {

        if (_workingTimer.Count <= 0) return;
        for(int i = 0; i < _workingTimer.Count; i++)
        {
            if (_workingTimer[i].GetTimerState() == TimerState.WORKING)
            {
                _workingTimer[i].UpdateTimer();
            }
            else
            {
                _workingTimer[i].ResetTimer();
                _notWorkTimer.Enqueue(_workingTimer[i]);
                _workingTimer.Remove(_workingTimer[i]);
            }
        }
    }


    public void InitTimerManager()
    {
        for(int i = 0; i < _maxTimerCount; i++)
        {
            CreateTimer();
        }
    }


    public void CreateTimer()
    {
        var newTimer = new GameTimer();
        _notWorkTimer.Enqueue(newTimer);
    }

    public void TryGetOneTimer(float time,Action task)
    {
        if (_notWorkTimer.Count <= 0)
        {
            CreateTimer();
        }
        var timer = _notWorkTimer.Dequeue();
        timer.StartTimer(time, task);
        _workingTimer.Add(timer);
    }



}
