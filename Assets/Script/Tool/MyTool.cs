using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGG.Tool.Singleton;

public class MyTool : Singleton<MyTool>
{
    private ChangeSlot _changeSlot;
    [SerializeField] private CameraShake _cameraShake;

    protected override void Awake()
    {
        base.Awake();
        _changeSlot = new ChangeSlot();
        _cameraShake = new CameraShake();
    }
    private void Update()
    {
        _changeSlot.Update();
    }

    public void RecoveryTimeScale()
    {
        _changeSlot.TryChangeTheSlot(Time.timeScale, 1, 0.2f, true);
    }

    public void TryChangeTheSlot(float currentValue, float targetValue, float time, bool Add)
    {
        _changeSlot.TryChangeTheSlot(currentValue, targetValue, time, Add);
    }

    public void TryPauseFrame(float duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(float duration)
    {
        float pauseTime = duration / 60.0f;
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }


    /// <summary>
    /// 受击者调用.
    /// </summary>
    public void TryGetACameraShake(float shakeForce, float shakeSpeed, float shakeDuration)
    {
        StopCoroutine(_cameraShake.Shake());
        _cameraShake.PlayShake(shakeForce, shakeSpeed, shakeDuration);

        StartCoroutine(_cameraShake.Shake());
    }


}
