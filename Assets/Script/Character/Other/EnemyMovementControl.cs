using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGG.Tool;

public class EnemyMovementControl : CharacterMovementControlBase
{

    [SerializeField] private bool _applyMovement;
    private float _horizontal;
    private float _vertical;
    private float _movement;

    [SerializeField]private bool isLookTarget;

    protected override void Awake()
    {
        base.Awake();
        _applyMovement = true;
    }

    protected override void Update()
    {
        base.Update();
        LookAtTarget();
        UpdateAIMovement();
        DrawLine();
    }




    private void UpdateAIMovement()
    {

        _animator.SetFloat(AnimatorID.LockID, 1);
        _animator.SetFloat(AnimatorID.HorizontalID, _horizontal, 0.15f, Time.deltaTime);
        _animator.SetFloat(AnimatorID.VerticalID, _vertical, 0.15f, Time.deltaTime);
        _animator.SetFloat(AnimatorID.MovementID, _movement, 0.15f, Time.deltaTime);
        _animator.SetBool(AnimatorID.HasInputID, true);

    }

    public void SetAIInput(float horizontal,float vertical,float movement=1)
    {
        if(_applyMovement)
        {
            _animator.SetFloat(AnimatorID.LockID, 1);
            _animator.SetBool(AnimatorID.HasInputID, true);
            _horizontal = horizontal;
            _vertical = vertical;
            _movement = movement;

        }
        else
        {
            _animator.SetFloat(AnimatorID.LockID, 0);
            _animator.SetBool(AnimatorID.HasInputID, false);
            _horizontal = 0;
            _vertical = 0;
            _movement = 0;

        }
    }

    public void LookAtTarget()
    {
        if (!isLookTarget) return;
        if (_animator.AnimationAtTag("FinalityHit"))
        {
            return;
        }
        transform.Look(EnemyManager.MainInstance.GetCurTarget().position, 50f);
    }

    public void SetLookTargetState(bool value)
    {
        isLookTarget = value;
    }

    public void SetLookTargetStateInAnimation(int value)
    {
        if (value == 1)
        {
            isLookTarget = true;

        }
        else
        {
            isLookTarget = false;
        }
    }




    private void DrawLine()
    {
        Debug.DrawRay(transform.position + Vector3.up * .8f, EnemyManager.MainInstance.GetCurTarget().position - transform.position, Color.yellow);
    }

}
