using GGG.Tool;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatControlBase : MonoBehaviour
{

    protected Animator _animtor;

    [SerializeField, Header("基础连招表")] protected CharacterComboSO _baseCombo;
    [SerializeField, Header("重击连招表")] protected CharacterComboSO _HeavyCombo;
    protected CharacterComboSO _currentCombo;
    protected int _currentComboIndex;
    protected int _hitIndex;

    protected float _maxColdTime;

    protected bool _canAttackInput;

    protected Transform _currentTarget;

    protected virtual void Awake()
    {
        _animtor = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        _canAttackInput = true;
    }
    protected virtual void Update()
    {
        MatchAttackPosition();
        StopCombo();
        ATKLookTarget();
    }
    protected bool CanBaseAttackInput()
    {
        if (!_canAttackInput) return false;
        if (_animtor.AnimationAtTag("Hit")) return false;
        if (_animtor.AnimationAtTag("Hit2")) return false;

        if (_animtor.AnimationAtTag("Finality")) return false;
        if (_animtor.AnimationAtTag("Parry")) return false;
        if (_animtor.AnimationAtTag("Dodge")) return false;
        if (_animtor.AnimationAtTag("Dodge2")) return false;
        if (_animtor.AnimationAtTag("DefenseFailed")) return false;
        return true;
    }

    public Transform GetCurTarget() => _currentTarget;

    #region 触发伤害

    /// <summary>
    /// 攻击时触发的动画事件
    /// </summary>
    protected void ATK()
    {
        TriggerDamage();
        GamePoolManager.MainInstance.TryAwakeOneItemFromPool("ATKSound", transform.position, Quaternion.identity);
        UpdateHitIndex();
    }

    private void MatchAttackPosition()
    {
        if (_animtor == null)
        {
            return;
        }
        if (_animtor.AnimationAtTag("Attack"))
        {
            var timer = _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (timer > 0.3f) return;
            if (_currentTarget == null) return;
            if (DevelopmentToos.DistanceForTarget(transform, _currentTarget) > 2f) return;

            if (!_animtor.isMatchingTarget && !_animtor.IsInTransition(0))
            {
                var direction = _currentTarget.position - transform.position;
                direction.Set(direction.x, 0, direction.z);
                _animtor.MatchTarget(_currentTarget.position - direction.normalized * _currentCombo.TryGetComboPositionOffset(_currentComboIndex)
                , Quaternion.identity, AvatarTarget.Body,
                new MatchTargetWeightMask(Vector3.one, 0), 0, 0.1f);
            }
        }
    }


    protected void TriggerDamage()
    {
        if (_currentTarget == null) return;


        if (DevelopmentToos.DistanceForTarget(transform, _currentTarget) > _currentCombo.TryGetOneAttackDistance(_currentComboIndex, _hitIndex)) return;

        if (_animtor.AnimationAtTag("Assassinate"))
        {
            EventManager.MainInstance.CallEvent("CharacterBeDamage", _currentCombo.TryGetDamge(_currentComboIndex), transform, _currentTarget);
        }
        if (Vector3.Dot(transform.forward, DevelopmentToos.DirectionForTarget(transform, _currentTarget)) < _currentCombo.TryGetOneAttackRange(_currentComboIndex, _hitIndex)) return;

        if (_animtor.AnimationAtTag("Attack")|| _animtor.AnimationAtTag("Finality"))
        {
            //设置屏幕震动
            EventManager.MainInstance.CallEvent("SetCharacterAttackAssistant", _currentCombo.TryGetShakeDuration(_currentComboIndex, _hitIndex)
            , _currentCombo.TryGetShakeForce(_currentComboIndex, _hitIndex), _currentCombo.TryGetShakeSpeed(_currentComboIndex, _hitIndex), _currentTarget);


            //设置拼刀后移补偿
            EventManager.MainInstance.CallEvent("SetCharacterAnimationCurve", _currentCombo.TryGetAnimationCurve(_currentComboIndex, _hitIndex),
                _currentCombo.TryGetAnimationCurveDuration(_currentComboIndex, _hitIndex), _currentTarget);

            Debug.Log(_currentComboIndex);

            //判定攻击对象
            EventManager.MainInstance.CallEvent("CharacterBeHit", _currentCombo.TryGetDamge(_currentComboIndex)
                , _currentCombo.TryGetOneHitName(_currentComboIndex, _hitIndex)
                , _currentCombo.TryGetOneParryName(_currentComboIndex, _hitIndex), transform, _currentTarget);


        }
        else
        {
            EventManager.MainInstance.CallEvent("CharacterBeDamage", _currentCombo.TryGetDamge(_currentComboIndex), transform, _currentTarget);
        }

    }

    protected void ATKLookTarget()
    {
        if (_currentTarget == null) return;
        if (DevelopmentToos.DistanceForTarget(transform, _currentTarget) > 3f) return;
        if (_animtor.AnimationAtTag("Attack") && _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
        {

            transform.Look(_currentTarget.position, 100f);
        }
    }

    protected void UpdateHitIndex()
    {
        _hitIndex++;
        if (_hitIndex >= _currentCombo.TryGetHitOrParryMaxCount(_currentComboIndex))
        {
            _hitIndex = 0;
        }
    }

    #endregion

    #region 角色攻击输入
    public virtual void CharacterBaseAttackInput()
    {
    }
    #endregion

    #region 角色动作执行
    protected void ExecuteComboInfo()
    {
        _hitIndex = 0;

        if (_currentComboIndex >= _currentCombo.TryGetComboMaxCount())
        {
            _currentComboIndex = 0;
        }
        _animtor.CrossFadeInFixedTime(_currentCombo.TryGetOneComboAction(_currentComboIndex), 0.15555f, 0, 0);
        _maxColdTime = _currentCombo.TryGetColdTime(_currentComboIndex);
        TimerManager.MainInstance.TryGetOneTimer(_maxColdTime, UpdateComboInfo);
        _canAttackInput = false;
    }
    #endregion

    #region 更新连招信息
    protected void UpdateComboInfo()
    {
        _currentComboIndex++;
        _maxColdTime = 0;
        _canAttackInput = true;
        if (_currentComboIndex >= _currentCombo.TryGetComboMaxCount())
        {
            _currentComboIndex = 0;
        }
    }

    #endregion

    #region 重置连招
    protected void ResetComboInfo()
    {
        _currentComboIndex = 0;
        _maxColdTime = 0;
    }
    #endregion

    #region 停止连招

    protected void StopCombo()
    {
        if (_animtor.AnimationAtTag("Motion") && _canAttackInput)
        {
            ResetComboInfo();
        }
    }

    #endregion


    #region 更改招式
    protected void ChangeTheCombo(CharacterComboSO data)
    {
        if (_currentCombo == null)
        {
            ResetComboInfo();
        }
        if (_currentCombo != data)
        {
            _currentCombo = data;
        }
    }

    #endregion
}
