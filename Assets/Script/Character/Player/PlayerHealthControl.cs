using GGG.Tool;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealthControl : CharacterHealthBase
{

    [SerializeField] private ParticleSystem _hitFX;
    private AnimationEventEffects _effect;

    [SerializeField] private AudioSource _perfectDodge;

    protected override void Update()
    {
        //DevelopmentToos.WTF("时间缩放    " + Time.timeScale);
        base.Update();
        if (!_animtor.AnimationAtTag("Defense"))
        {
            UpdateHPUI();
            _healthInfo.UpdateStrength();


        }

    }


    protected override void CharacterHitAction(float damage, string hitName, string parryName)
    {

        DevelopmentToos.WTF(CanDodgeDamage());
        //玩家在闪避状态
        if (CanDodgeDamage())
        {
            if (CanPerfectDodge())
            {
                _perfectDodge.Play();
                MyTool.MainInstance.TryChangeTheSlot(Time.timeScale, 0.1f, 0.05f, false);
                TimerManager.MainInstance.TryGetOneTimer(0.05f, MyTool.MainInstance.RecoveryTimeScale);
                _healthInfo.PerfectDodge();
                UpdateHPUI();
            }
            
            DevelopmentToos.WTF("GGG");
        }
        else if (CanDefense())
        {
            _healthInfo.BeHit_Play(damage);
            //招架条不足以挡下这次攻击
            if (_healthInfo._canBeFinality)
            {
                MyTool.MainInstance.TryChangeTheSlot(Time.timeScale, 0.15f, 0.1f, false);
                TimerManager.MainInstance.TryGetOneTimer(0.15f, MyTool.MainInstance.RecoveryTimeScale);
                _effect.InstantiateEffectHitFX(2);

                _healthInfo.ResetCanFinality();
                _animtor.Play("DefenseFailed", 0, 0);
                GamePoolManager.MainInstance.TryAwakeOneItemFromPool("ParrySound", transform.position, Quaternion.identity);
                GamePoolManager.MainInstance.TryAwakeOneItemFromPool("HitSound", transform.position, Quaternion.identity);
                GamePoolManager.MainInstance.TryAwakeOneItemFromPool("BreakSound", transform.position, Quaternion.identity);
                MyTool.MainInstance.TryGetACameraShake(_assistant.ShakeForce * 1.5f, _assistant.ShakeSpeed * 1.5f, _assistant.ShakeDuration);

            }
            else
            {
                MyTool.MainInstance.TryPauseFrame(2.5f);
                _healthInfo.ResetCanFinality();
                //玩家不在格挡状态
                _animtor.Play(parryName, 0, 0);
                GamePoolManager.MainInstance.TryAwakeOneItemFromPool("ParrySound", transform.position, Quaternion.identity);
                MyTool.MainInstance.TryGetACameraShake(_assistant.ShakeForce, _assistant.ShakeSpeed, _assistant.ShakeDuration);
                _assistant.IntoParryHelper();

            }

            if (_effect == null)
            {
                _effect = GetComponent<AnimationEventEffects>();
            }
            _effect.InstantiateEffectHitFX(1);
            _effect.InstantiateEffectHitFX(3);

        }
        else
        {
            MyTool.MainInstance.TryPauseFrame(2.5f);
            //玩家不在格挡状态
            _animtor.Play(hitName, 0, 0);

            GamePoolManager.MainInstance.TryAwakeOneItemFromPool("HitSound", transform.position, Quaternion.identity);

            _healthInfo.BeHit_Play(damage,false);

            if (_effect == null)
            {
                _effect = GetComponent<AnimationEventEffects>();
            }
            _effect.InstantiateEffectHitFX(0);
            MyTool.MainInstance.TryGetACameraShake(_assistant.ShakeForce * 0.8f, _assistant.ShakeSpeed * 0.8f, _assistant.ShakeDuration);

        }
        UpdateHPUI();
    }
    private void UpdateHPUI()
    {
        EventManager.MainInstance.CallEvent("UpdatePlayerHealthAndStrenghtUI", _healthInfo);
    }
}
