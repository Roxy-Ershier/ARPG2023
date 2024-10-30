using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovementControlBase : MonoBehaviour
{
    protected Animator _animator;
    protected CharacterController _control;

    
    [SerializeField,Header("地面检测")]protected LayerMask _whatIsGround;
    [SerializeField] private float _detectionRang;
    [SerializeField] private float _detectionOffsetPosition;

    //重力模拟

    [SerializeField]protected bool _enableGravity;
    protected float _curVerticalVelocity;
    private const float Gravity = -9.8f;
    private Vector2 _moveDirection;

    protected virtual void Awake()
    {
        _animator=GetComponent<Animator>();
        _control=GetComponent<CharacterController>();
    }
    protected virtual void Start()
    {
        //_enableGravity = true;
    }

    protected virtual void Update()
    {
        SetGravityVelocity();
        UpdateCharacterGravity();
    }

    //private void OnAnimatorMove()
    //{
    //    _animator.ApplyBuiltinRootMotion();
    //    UpdateCharacterMoveDirection(_animator.deltaPosition);
    //}


    protected bool CharacterIsOnGround()
    {
        return Physics.CheckSphere(transform.position + _detectionOffsetPosition * Vector3.up
            , _detectionRang, _whatIsGround, QueryTriggerInteraction.Ignore);
    }


    private void SetGravityVelocity()
    {
        if (!_enableGravity) return;
        if (CharacterIsOnGround())
        {
            _curVerticalVelocity = -2f;
        }
        else
        {
            _curVerticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void UpdateCharacterGravity()
    {
        if (!_enableGravity) return;
        _control.Move(_curVerticalVelocity * Vector3.up * Time.deltaTime);
    }

    private Vector3 SlopDetection(Vector3 movePosition)
    {
        if (Physics.Raycast(transform.position + (transform.up * .5f), Vector3.down, out var hit, _control.height, _whatIsGround))
        {
            if (Vector3.Dot(Vector3.up, hit.normal) != 0)
            {
                return Vector3.ProjectOnPlane(movePosition, hit.normal);
            }
        }


        return movePosition;
    }

    protected void UpdateCharacterMoveDirection(Vector3 direction)
    {
        _moveDirection = SlopDetection(direction);
        _control.Move(_moveDirection * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + _detectionOffsetPosition * Vector3.up
            , _detectionRang);
    }

}
