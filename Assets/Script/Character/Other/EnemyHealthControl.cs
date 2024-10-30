using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class EnemyHealthControl : CharacterHealthBase
{
    private AnimationEventEffects _effect;
    

    private bool isRecovery;
    protected override void Awake()
    {
        base.Awake();
        EnemyManager.MainInstance.AddEnemyToList(transform.gameObject);
        _effect = GetComponent<AnimationEventEffects>();
    }
    protected override void Update()
    {
        base.Update();
        _healthInfo.UpdateStrength();
        if (isRecovery && _healthInfo.StrengthIsFull)
        {
            EventManager.MainInstance.CallEvent("RecoveryBossStrenght", _healthInfo);
            isRecovery = false;
        }
        //DevelopmentToos.WTF("’–º‹Ãı     +" + _healthInfo._cur_Strength);
    }
    protected override void CharacterHitAction(float damage, string hitName, string parryName)
    {
        if (CanDodgeDamage()) return;
        if (CanDefense())
        {
            _healthInfo.BeHit(damage, true);
            if (_healthInfo.CanBeFinality)
            {
                EventManager.MainInstance.CallEvent("SetPlayerCanFinality", true);
                _healthInfo.ResetCanFinality();
                _effect.InstantiateEffectHitFX(2);
                MyTool.MainInstance.TryChangeTheSlot(Time.timeScale, 0.15f, 0.1f, false);
                TimerManager.MainInstance.TryGetOneTimer(0.15f, MyTool.MainInstance.RecoveryTimeScale);
                _animtor.Play("DefenseFailed", 0, 0);
                GamePoolManager.MainInstance.TryAwakeOneItemFromPool("HitSound", transform.position, Quaternion.identity);
                GamePoolManager.MainInstance.TryAwakeOneItemFromPool("BreakSound", transform.position, Quaternion.identity);
                isRecovery = true;
                MyTool.MainInstance.TryGetACameraShake(_assistant.ShakeForce*1.5f, _assistant.ShakeSpeed*1.5f, _assistant.ShakeDuration);

            }
            else
            {
                MyTool.MainInstance.TryPauseFrame(2.5f);

                //MyTool.MainInstance.TryChangeTheSlot(Time.timeScale, 0.4f, 0.08f, false);
                //TimerManager.MainInstance.TryGetOneTimer(0.075f, MyTool.MainInstance.RecoveryTimeScale);
                _animtor.Play(parryName, 0, 0);

                MyTool.MainInstance.TryGetACameraShake(_assistant.ShakeForce, _assistant.ShakeSpeed, _assistant.ShakeDuration);
                _assistant.IntoParryHelper();
            }

            GamePoolManager.MainInstance.TryAwakeOneItemFromPool("ParrySound", transform.position, Quaternion.identity);

            _effect.InstantiateEffectHitFX(1);
            _effect.InstantiateEffectHitFX(3);


            EventManager.MainInstance.CallEvent("UpdateBossStrenght", _healthInfo);


        }
        else
        {

            if (!_animtor.AnimationAtTag("Attack"))
            {
                _animtor.Play(hitName, 0, 0);

            }

            GamePoolManager.MainInstance.TryAwakeOneItemFromPool("HitSound", transform.position, Quaternion.identity);
            if (_effect == null)
            {
                _effect = GetComponent<AnimationEventEffects>();
            }
            _effect.InstantiateEffectHitFX(0);
            if (hitName == "BeFinality")
            {
                MyTool.MainInstance.TryPauseFrame(20f);
            }
            else if(hitName== "BeFinality2")
            {
                MyTool.MainInstance.TryPauseFrame(30f);
                MyTool.MainInstance.TryGetACameraShake(_assistant.ShakeForce, _assistant.ShakeSpeed, _assistant.ShakeDuration);

            }
            else
            {
                MyTool.MainInstance.TryPauseFrame(2.5f);
                MyTool.MainInstance.TryGetACameraShake(_assistant.ShakeForce * 0.8f, _assistant.ShakeSpeed * 0.8f, _assistant.ShakeDuration);


            }
            _healthInfo.BeHit(damage, false);
            EventManager.MainInstance.CallEvent("UpdateBossHealth", _healthInfo);
        }

        //if (_healthInfo.CanBeFinality)
        //{
        //    EventManager.MainInstance.CallEvent("AddOrRemoveFinalityObjList", transform, true);
        //    _healthInfo.ResetCanFinality();
        //}
    }

    protected override bool CanDefense()
    {
        if (_healthInfo.CanBeFinality) return true;
        if (!_healthInfo.StrengthIsFull) return false;
        if (_animtor.AnimationAtTag("Motion")) return true;
        if(_animtor.AnimationAtTag("Dodge"))return true;
        if(_animtor.AnimationAtTag("Parry"))return true;
        
        return false;
    }

}
