using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthBase : MonoBehaviour
{
    //受击到攻击
    //扣血

    protected Animator _animtor;
    protected AttackAssistant _assistant;

    protected Transform _currentAttacker;//当前的攻击者
    [SerializeField,Header("生命信息模版")] private CharacterHealthInfo _healthInfoModel;
    protected CharacterHealthInfo _healthInfo;
    

    protected virtual void Awake()
    {
        _animtor = GetComponent<Animator>();
        _healthInfo = Instantiate(_healthInfoModel);
        _assistant = GetComponent<AttackAssistant>();
    }
    protected virtual void Start()
    {
        _healthInfo.InitHealthInfo();
    }

    protected virtual bool CanDefense()
    {
        if ((_animtor.AnimationAtTag("Hit")) && _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.65f
            &&_animtor.GetBool(AnimatorID.Defense)) return true;
        if ((_animtor.AnimationAtTag("Dodge")) && _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.55f
            && _animtor.GetBool(AnimatorID.Defense)) return true;
        if ((_animtor.AnimationAtTag("Dodge2")) && _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.55f
            && _animtor.GetBool(AnimatorID.Defense)) return true;
        if (!(_animtor.AnimationAtTag("Defense")|| _animtor.AnimationAtTag("Parry"))) return false;
        //if (!_healthInfo.StrengthIsFull) return false;
        return true;
    }

    protected virtual void Update()
    {
        LookAttackerInHit();
    }
    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening<float, string, string, Transform, Transform>("CharacterBeHit", OnCharacterHitEventHandle);
        EventManager.MainInstance.AddEventListening<string, Transform, Transform>("CharacterBeFinality", OnCharacterFinalityHitEventHandle);
        EventManager.MainInstance.AddEventListening<float, Transform, Transform>("CharacterBeDamage", OnCharacterBeTriggerDamageEventHandle);
    }

    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent<float, string, string, Transform, Transform>("CharacterBeHit", OnCharacterHitEventHandle);
        EventManager.MainInstance.RemoveEvent<string, Transform, Transform>("CharacterBeFinality", OnCharacterFinalityHitEventHandle);
        EventManager.MainInstance.RemoveEvent<float, Transform, Transform>("CharacterBeDamage", OnCharacterBeTriggerDamageEventHandle);

    }

    protected bool CanDodgeDamage()
    {
        if ((_animtor.AnimationAtTag("Dodge"))
            && (_animtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f && _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4f))
        {
            return true;
        }
        else if (_animtor.AnimationAtTag("Dodge2") 
            && (_animtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f && _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f))
            return true;
        return false;
    }
    protected bool CanPerfectDodge()
    {
        if (_animtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f && _animtor.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.1f)
        {
            return true;
        }
        return false;
    }

    protected virtual void CharacterHitAction(float damage,string hitName,string parryName)
    {

    }

    protected void TakeDamage(float damage)
    {
        //TODO:扣血
    }

    private void LookAttackerInHit()
    {
        if (_animtor == null) return;
        if (_animtor.AnimationAtTag("Hit")|| _animtor.AnimationAtTag("Parry")) 
        {
            //DevelopmentToos.WTF($"{transform.name}看向{_currentAttacker.name}");
            transform.Look(_currentAttacker.position, 50f);
        }
    }

    private void SetAttacker(Transform attacker)
    {
        if (_currentAttacker == null || _currentAttacker != attacker)
        {
            _currentAttacker = attacker;
        }
    }

    private void OnCharacterHitEventHandle(float damage,string hitName,string parryName,Transform attacker,Transform self)
    {

        if (self != transform) return;
        SetAttacker(attacker);
        CharacterHitAction(damage,hitName, parryName);
        TakeDamage(damage);
    }
    private void OnCharacterFinalityHitEventHandle(string hitName, Transform attacker, Transform self)
    {

        if (self != transform) return;
        SetAttacker(attacker);
        BeFinality(hitName);
    }
    private void BeFinality(string hitName)
    {
        _animtor.Play(hitName, 0, 0);
    }

    private void OnCharacterBeTriggerDamageEventHandle(float Damage, Transform attacker, Transform self)
    {

        if (self != transform) return;
        SetAttacker(attacker);
        BeTriggerDamage(Damage);
    }

    private void BeTriggerDamage(float Damage)
    {
        //扣血
        TakeDamage(Damage);

        GamePoolManager.MainInstance.TryAwakeOneItemFromPool("HitSound", transform.position, Quaternion.identity);
    }

    
}
