using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthInfo", menuName = "SO/CharacterHealthInfo")]
public class CharacterHealthInfo : ScriptableObject
{
    [SerializeField] private float _cur_HP;
    [SerializeField] private float _cur_Strength;

    [SerializeField] private CharacterHealthDataSO _healthData;
    [SerializeField] public bool _canBeFinality;
    public bool IsDead => _cur_HP <= 0;
    private bool _strengthIsFull;

    public bool StrengthIsFull => _strengthIsFull;

    public float Cur_HP => _cur_HP;
    public float Cur_Strength => _cur_Strength;

    public CharacterHealthDataSO HealthData => _healthData;

    public bool CanBeFinality => _canBeFinality;
    [SerializeField]private float _strengthRecoverySpeed;
    public void InitHealthInfo()
    {
        _cur_HP = _healthData.HP_MAX;
        _cur_Strength = _healthData.Strength_MAX;
        if (_cur_Strength > 0)
        {
            _strengthIsFull = true;
        }
    }

    public void UpdateStrength()
    {
        if (_strengthIsFull)
        {
            return;
        }
        if(ValueIsBeBlock(ref _cur_Strength, Time.deltaTime* _strengthRecoverySpeed, 0, _healthData.Strength_MAX, true))
        {
            _strengthIsFull = true;
        }
    }


    public void BeHit(float damage, bool Defense = true)
    {
        if (_strengthIsFull && Defense) 
        {
            BeStrength(damage * 1.5f);
        }
        else
        {
            BeDamage(damage);
        }
    }

    public void BeHit_Play(float damage, bool Defense = true)
    {
        if (Defense)
        {
            BeStrength(damage * 1.5f);
            _strengthIsFull = false;

        }
        else
        {
            BeDamage(damage);
        }
    }

    private void BeDamage(float damage)
    {
        if (ValueIsBeBlock(ref _cur_HP,damage, 0, _healthData.HP_MAX))
        {
            return;
        }
    }
    
    private void BeStrength(float damage)
    {
        if(ValueIsBeBlock(ref _cur_Strength,damage, 0, _healthData.Strength_MAX))
        {
           // BeDamage(damage);
            _canBeFinality = true;
            _strengthIsFull = false;
        }
    }

    private bool ValueIsBeBlock(ref float value,float damage,float min,float max,bool Add=false)
    {
        value = value + (Add ? damage : -damage);
        if (value < min || value > max)
        {
            value = Add ? max : min;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PerfectDodge()
    {
        if (ValueIsBeBlock(ref _cur_Strength, 30, 0, _healthData.Strength_MAX, true))
        {
            _strengthIsFull = true;
        }
    }


    public void ResetCanFinality()
    {
        _canBeFinality = false;
    }
}
