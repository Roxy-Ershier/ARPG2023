using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeSlot
{
    private float _currentSpeed;

    private float _targetValue;

    private bool _add;
    private bool _finish;

    public ChangeSlot() 
    {
        _targetValue = Time.timeScale;
        _finish = true;
    }

    public void Update()
    {
        if (_finish)
        {
            return;
        }
        //Time.timeScale -= _currentSpeed * Time.deltaTime;
        Time.timeScale -= _currentSpeed * Time.unscaledDeltaTime;
        Debug.Log(Time.deltaTime);
        if (_add)
        {
            if (Time.timeScale > _targetValue)
            {
                Time.timeScale = _targetValue;
                _finish = true;
            }
        }
        else
        {
            if (Time.timeScale < _targetValue)
            {
                Time.timeScale = _targetValue;
                _finish = true;
            }
        }
        

    }

    public void RecoveryTimeScale()
    {
        TryChangeTheSlot(Time.timeScale, 1, 0.2f, true);
    }

    public void TryChangeTheSlot(float currentValue,float targetValue, float time,bool Add)
    {
        _finish = false;
        float x = currentValue - targetValue;
        _currentSpeed = x / time;
        _targetValue = targetValue;
        _add = Add; 
    }


}
