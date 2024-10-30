using BehaviorDesigner.Runtime.Tasks;
using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackCommandCondiction : Conditional
{
    private EnemyCombatControl _enemyCombatControl;
    private Animator _animator;


    public override void OnAwake()
    {
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
        _animator = GetComponent<Animator>();
    }


    public override TaskStatus OnUpdate()
    {
        if (_enemyCombatControl.IsAttacker() && !_animator.AnimationAtTag("Hit") && 
            !(_animator.AnimationAtTag("Parry")&&_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            && !_animator.AnimationAtTag("FinalityHit") && !_animator.AnimationAtTag("Attack")) 
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

}
