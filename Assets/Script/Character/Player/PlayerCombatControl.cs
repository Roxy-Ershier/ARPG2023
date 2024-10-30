using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerCombatControl : CharacterCombatControlBase
{

    [SerializeField, Header("处决连招表")] private CharacterComboSO _FinalityCombo;
    [SerializeField, Header("刺杀连招表")] private CharacterComboSO _AssassinateCombo;
    
    

    [SerializeField, Header("检测参数")] private float _detectionRang;
    [SerializeField] private float _detectionDistance;
    [SerializeField] private LayerMask _detectionLayer;
    private Vector3 _detectionDirection;
    private Transform _camera;


    private bool _canFinality;


    [SerializeField,Header("处决信息")] private List<Transform> CanFinalityObj = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main.transform;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening<Transform, bool>("AddOrRemoveFinalityObjList", AddOrRemoveCanFinalityObjInfoEventHandle);
        EventManager.MainInstance.AddEventListening<bool>("SetPlayerCanFinality", SetCanFinality);
    }

    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent<Transform, bool>("AddOrRemoveFinalityObjList", AddOrRemoveCanFinalityObjInfoEventHandle);
        EventManager.MainInstance.RemoveEvent<bool>("SetPlayerCanFinality", SetCanFinality);
    }

    protected override void Update()
    {
        base.Update();
        CharacterBaseAttackInput();
        UpdateDetection();
        GetFinalityInput();
        MatchAnimationInFinality();
        GetAssassinateInput();
    }

    private void FixedUpdate()
    {
    }
    


    

    #region 检测敌人

    private void UpdateDetection()
    {
        _detectionDirection = (_camera.forward * GameInputManager.MainInstance.MovementVec.y)
            + (_camera.right * GameInputManager.MainInstance.MovementVec.x);
        _detectionDirection.Set(_detectionDirection.x, 0, _detectionDirection.z);
        _detectionDirection = _detectionDirection.normalized;
        if (Physics.SphereCast(transform.position+Vector3.up*0.85f,_detectionRang,_detectionDirection,
            out var hit,_detectionDistance,_detectionLayer,QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.name);
            _currentTarget = hit.collider.transform;
        }

    }



    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.85f + _detectionDirection * _detectionDistance, _detectionRang);
    //}
    #endregion

    #region 处决

    private bool CanFinality()
    {
        if (!_canAttackInput) return false;
        if (_animtor.AnimationAtTag("Finality")) return false;
        if (_animtor.AnimationAtTag("Assassinate")) return false;
        if (_currentTarget == null) return false;
        if (DevelopmentToos.DistanceForTarget(transform, _currentTarget) > 2f) return false;
        if (!CanFinalityObj.Contains(_currentTarget)) return false;
        return true;

    }

    private void AddOrRemoveCanFinalityObjInfoEventHandle(Transform obj,bool Add=true)
    {
        if (Add)
        {
            if(!CanFinalityObj.Contains(obj))
                CanFinalityObj.Add(obj);
        }
        else
        {
            if (CanFinalityObj.Contains(obj))
                CanFinalityObj.Remove(obj);
        }
    }


    private void GetFinalityInput()
    {
        if (!CanFinality()) return;

        if (GameInputManager.MainInstance.Finality)
        {
            //CanFinalityObj.Remove(_currentTarget);
            _currentComboIndex = Random.Range(0, _FinalityCombo.TryGetComboMaxCount());
            _animtor.CrossFadeInFixedTime(_FinalityCombo.TryGetOneComboAction(_currentComboIndex), 0.1555f, 0, 0);

            EventManager.MainInstance.CallEvent("CharacterBeFinality", _FinalityCombo.TryGetOneHitName(_currentComboIndex, 0), transform, _currentTarget);

            TimerManager.MainInstance.TryGetOneTimer(_FinalityCombo.TryGetColdTime(_currentComboIndex), ResetComboInfo);
        }
    }
    private void MatchAnimationInFinality()
    {
        if (!_animtor) return;
        if (_animtor.AnimationAtTag("Finality"))
        {
            transform.Look(_currentTarget.position, 1000f);
            RunningMatch(_currentTarget.position + _currentTarget.forward * _FinalityCombo.TryGetComboPositionOffset(_currentComboIndex)
                , Quaternion.identity,
                new MatchTargetWeightMask(Vector3.one, 0), 0, 0.1f);
        }
        else if (_animtor.AnimationAtTag("Assassinate"))
        {
            var direction = (_currentTarget.transform.position - transform.position).normalized;
            direction.y = 0f;
            RunningMatch(_currentTarget.position -_currentTarget.forward * _AssassinateCombo.TryGetComboPositionOffset(_currentComboIndex)
                ,Quaternion.LookRotation(direction), new MatchTargetWeightMask(Vector3.one, 1), 0, 0.1f);
        }
    }
    
    private void RunningMatch(Vector3 matchPos,Quaternion rotation, MatchTargetWeightMask weight,float startTime=0f,float endTime=0.1f)
    {
        if (!_animtor.isMatchingTarget&&!_animtor.IsInTransition(0))
        {
             _animtor.MatchTarget(matchPos
            , Quaternion.identity, AvatarTarget.Body, new MatchTargetWeightMask(Vector3.one, 0), startTime, endTime);
        }
       
    }
    #endregion

    #region 暗杀
    private bool CanAssassinate()
    {
        if (_animtor.AnimationAtTag("Attack")) return false;
        if (_animtor.AnimationAtTag("Finality")) return false;
        if (_animtor.AnimationAtTag("Assassinate")) return false;
        if (_currentTarget == null) return false;
        if (Vector3.Dot(transform.forward, -_currentTarget.forward) > -0.85f) return false;
        return true;
    }

    private void GetAssassinateInput()
    {
        if (!CanAssassinate()) return;

        if (GameInputManager.MainInstance.Finality)
        {
            _currentCombo = _AssassinateCombo;
            _currentComboIndex = Random.Range(0, _AssassinateCombo.TryGetComboMaxCount());
            _animtor.CrossFadeInFixedTime(_AssassinateCombo.TryGetOneComboAction(_currentComboIndex), 0.1555f, 0, 0);

            EventManager.MainInstance.CallEvent("CharacterBeFinality", _AssassinateCombo.TryGetOneHitName(_currentComboIndex, 0), transform, _currentTarget);

            TimerManager.MainInstance.TryGetOneTimer(_AssassinateCombo.TryGetColdTime(_currentComboIndex), ResetComboInfo);
        }
    }

    #endregion

    #region 角色攻击输入
    public override void CharacterBaseAttackInput()
    {
        if (!CanBaseAttackInput()) return;

        if (GameInputManager.MainInstance.LAttack)
        {
            if (_canFinality)
            {
                //PlayVoice();
                Invoke("PlayVoice", 0.5f);
                GamePoolManager.MainInstance.TryAwakeOneItemFromPool("FinalitySound", transform.position, Quaternion.identity);
                ResetComboInfo();
                ChangeTheCombo(_FinalityCombo);
                EventManager.MainInstance.CallEvent("IntoExecuteTime");
                _animtor.CrossFade("Execute1", 0.1555f, 0, 0);
                SetCanFinality(false);
            }
            else
            {
                ChangeTheCombo(_baseCombo);

            }
                ExecuteComboInfo();

        }
        //else if (GameInputManager.MainInstance.RAttack)
        //{

        //    ChangeTheCombo(_HeavyCombo);

        //    ExecuteComboInfo();
        //}

    }
    #endregion
    [SerializeField]private PlayerVoiceAssets _playerVoiceAssets;
    private AudioSource _audioSource;
    public void PlayVoice()
    {
        _audioSource.clip = _playerVoiceAssets.TryGetOneClip(PlayerVoiceType.FinalityVoice);
        _audioSource.Play();

    }

    private void SetCanFinality(bool value)
    {
        _canFinality = value;
    }





}
