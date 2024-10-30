using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class PlayerMovementControl : CharacterMovementControlBase
{
    private float _rotationCurVelocity;
    [SerializeField] private float _rotationSmoothTime;
    private float _rotationAngle;

    private Transform _mainCamera;


    [SerializeField, Header("目标锁定")] private float _detectionDistance;
    [SerializeField] private LayerMask _whatIsTarget;
    private bool _isLock;
    private Transform _lockTarget;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening<bool,Transform,float>("LockOrUnLockTarget", LockOrUnLockTarget);
    }

    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent<bool, Transform, float>("LockOrUnLockTarget", LockOrUnLockTarget);
    }

    protected override void Update()
    {
        base.Update();
        CheckLockInput();
    }
    private void LateUpdate()
    {
        UpdateCharacterRotation();
        UpdateCharacterAnimation();
    }

    private void UpdateCharacterRotation()
    {
        if (!CharacterIsOnGround()) return;

        if (CameraIsLock())
        {
            if (_animator.AnimationAtTag("Dodge"))
            {

            }
            else
            {
                transform.Look(_lockTarget.position, 5f);
            }
        }
        else
        {
            //未锁定敌人
            if (_animator.GetBool(AnimatorID.HasInputID))
            {
                _rotationAngle = Mathf.Atan2(GameInputManager.MainInstance.MovementVec.x, GameInputManager.MainInstance.MovementVec.y)
                * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;

            }
            if (_animator.GetBool(AnimatorID.HasInputID) && _animator.AnimationAtTag("Motion"))
            {
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    _rotationAngle, ref _rotationCurVelocity, _rotationSmoothTime);
            }
        }
        
    }

    private bool CameraIsLock()
    {
        return _isLock && _lockTarget != null;
    }

    private void UpdateCharacterAnimation()
    {
        if (!CharacterIsOnGround()) return;

        _animator.SetBool(AnimatorID.HasInputID, GameInputManager.MainInstance.MovementVec != Vector2.zero);
        _animator.SetBool(AnimatorID.RunID, GameInputManager.MainInstance.Run);


        _animator.SetFloat(AnimatorID.HorizontalID, GameInputManager.MainInstance.MovementVec.x, 0.1f, Time.deltaTime);
        _animator.SetFloat(AnimatorID.VerticalID, GameInputManager.MainInstance.MovementVec.y, 0.1f, Time.deltaTime);
        _animator.SetBool(AnimatorID.Defense, GameInputManager.MainInstance.Defense);
       
        
        if (_animator.GetBool(AnimatorID.HasInputID))
        {
            _animator.SetFloat(AnimatorID.MovementID, _animator.GetBool(AnimatorID.RunID) ?
            2f : GameInputManager.MainInstance.MovementVec.sqrMagnitude, 0.15f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(AnimatorID.MovementID, 0, 0.15f, Time.deltaTime);
        }
        if (_isLock)
        {
            _animator.SetFloat(AnimatorID.LockID, 1f, 0.15f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(AnimatorID.LockID, 0f, 0.15f, Time.deltaTime);

        }
    }

    private void CheckLockInput()
    {
        

        if (GameInputManager.MainInstance.LockUnLock)
        {
            Vector3 dir = Camera.main.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * .85f, dir, out hit, _detectionDistance, _whatIsTarget, QueryTriggerInteraction.Ignore)) 
            {
                EventManager.MainInstance.CallEvent("LockOrUnLockTarget", !_isLock, hit.collider.transform,_detectionDistance);
            }
            else
            {
                EventManager.MainInstance.CallEvent<bool,Transform,float>("LockOrUnLockTarget", false, null, _detectionDistance);
            }

        }
    }


    private void LockOrUnLockTarget(bool isLock, Transform target = null, float maxLockDistance = 10)
    {
        _isLock = isLock;
        _lockTarget = target;
        
    }
}
