using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAssistant : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _controller;
    private CharacterCombatControlBase _controlBase;
    private float _shakeForce;
    private float _shakeSpeed;
    private float _shakeDuration;

    public float ShakeForce => _shakeForce;
    public float ShakeSpeed => _shakeSpeed;
    public float ShakeDuration => _shakeDuration;

    private AnimationCurve _currentAnimationCurve;
    private float _duration;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _controlBase = GetComponent<CharacterCombatControlBase>();
    }

    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening<float, float, float, Transform>("SetCharacterAttackAssistant", SetAttackAssistant);
        EventManager.MainInstance.AddEventListening<AnimationCurve, float, Transform>("SetCharacterAnimationCurve", SetAnimationCurve);
    }
    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent<float, float, float, Transform>("SetCharacterAttackAssistant", SetAttackAssistant);
        EventManager.MainInstance.RemoveEvent<AnimationCurve, float, Transform>("SetCharacterAnimationCurve", SetAnimationCurve);

    }

    private void Update()
    {
        //DevelopmentToos.WTF(Time.timeScale);
        
    }

    /// <summary>
    /// 攻击者使用，设置震动参数
    /// </summary>
    /// <param name="shakeDuration"></param>
    /// <param name="shakeForce"></param>
    /// <param name="shakeSpeed"></param>
    void SetAttackAssistant(float shakeDuration, float shakeForce, float shakeSpeed,Transform self)
    {
        if (self != transform) return;
        _shakeDuration = shakeDuration;
        _shakeForce = shakeForce;
        _shakeSpeed = shakeSpeed;
    }

    void SetAnimationCurve(AnimationCurve curve,float duration, Transform self)
    {
        if (self != transform) return;
        _currentAnimationCurve = curve;
        _duration = duration;
    }


    public void IntoParryHelper()
    {
        if (_controlBase.GetCurTarget() == null) return;
        StopAllCoroutines();
        StartCoroutine(AttackHelper());
    }
    
    IEnumerator AttackHelper()
    {
        var elapsed = 0.0f;
        Vector3 dir = _controlBase.GetCurTarget().forward;
        while (elapsed<_duration)
        {
            elapsed += Time.deltaTime;
            if (_animator.AnimationAtTag("Parry"))
            {
                elapsed += Time.deltaTime;
                var percentComplete = elapsed / _duration;
                var damper = _currentAnimationCurve.Evaluate(percentComplete);
                _controller.Move(Time.deltaTime * damper * dir);

            }
            yield return Time.deltaTime ;
        }
    }
}
