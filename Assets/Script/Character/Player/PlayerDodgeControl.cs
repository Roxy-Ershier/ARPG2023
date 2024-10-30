using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDodgeControl : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private float _canBreakAttackTime = 0.6f;
    [SerializeField] private float _canBreakHitTime = 0.65f;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDodgeInput();
    }


    private void CheckDodgeInput()
    {
        //Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).IsName("Fly_B_L"));
        if (GameInputManager.MainInstance.Dodge)
        {
            
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Fly_B_L")&& _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f
                &&!_animator.IsInTransition(0))
            {
                _animator.CrossFade("QuickRise_H", 0.25f, 0, 0);
            }
            else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Fly_B") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.15f
                && !_animator.IsInTransition(0))
            {
                _animator.CrossFade("QuickRise_T", 0.1f, 0, 0);
            }

            if (!CanDodge()) return;


            //if (_animator.GetBool(AnimatorID.HasInputID) && GameInputManager.MainInstance.Run)
            //{

                 if (GameInputManager.MainInstance.MovementVec.x > 0.9f && _animator.GetFloat(AnimatorID.LockID) > 0.9f)
                 {
                     _animator.Play("Dodge_Right");
                 }
                else if (GameInputManager.MainInstance.MovementVec.x < -0.9f && _animator.GetFloat(AnimatorID.LockID) > 0.9f)
                {
                    _animator.Play("Dodge_Left");

                }
                else if (!_animator.GetBool(AnimatorID.HasInputID) || GameInputManager.MainInstance.MovementVec.y == -1f && _animator.GetFloat(AnimatorID.LockID) > 0.9f)
                {
                    _animator.Play("Dodge_Back");
                }
                else
                {
                    float _rotationAngle = Mathf.Atan2(GameInputManager.MainInstance.MovementVec.x, GameInputManager.MainInstance.MovementVec.y)
                        * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, _rotationAngle, transform.eulerAngles.z);
                    _animator.Play("Dodge_Front");
                }
            //}
            //else
            //{
            //    if (!_animator.GetBool(AnimatorID.HasInputID) || GameInputManager.MainInstance.MovementVec.y == -1f && _animator.GetFloat(AnimatorID.LockID) > 0.9f)
            //    {
            //        _animator.Play("Dodge_Back");
            //    }
            //    else if (GameInputManager.MainInstance.MovementVec.x > 0.9f && _animator.GetFloat(AnimatorID.LockID) > 0.9f)
            //{
            //    _animator.Play("Dodge_Right");
            //}
            //else if (GameInputManager.MainInstance.MovementVec.x < -0.9f && _animator.GetFloat(AnimatorID.LockID) > 0.9f)
            //{
            //    _animator.Play("Dodge_Left");

            //}
            //}

        }
    }
    
    private bool CanDodge()
    {
        if (_animator.AnimationAtTag("Attack") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime< _canBreakAttackTime) return false;
        if (_animator.AnimationAtTag("Dodge")||_animator.AnimationAtTag("DefenseFailed")) return false;
        if (_animator.AnimationAtTag("Dodge2") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25f) return false;
        if (_animator.AnimationAtTag("Hit") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < _canBreakHitTime) return false;
        if (_animator.AnimationAtTag("Hit2")) return false;

        return true;
    }



}
