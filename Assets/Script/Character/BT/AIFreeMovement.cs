using BehaviorDesigner.Runtime.Tasks;
using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFreeMovement : Action
{
    private EnemyMovementControl _enemyMovementControl;
    private EnemyCombatControl _enemyCombatControl;

    private float _actionTime=2f;
    private float _curActionTime;
    private Vector2 _DodgeColdTime = new Vector2(3, 5);
    private float _currentdodgeColdTime;
    
    private Animator _animator;

    public override void OnAwake()
    {
        base.OnAwake();
        _enemyMovementControl = GetComponent<EnemyMovementControl>();
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
        _animator=GetComponent<Animator>();
    }


    private bool CanDodge()
    {
        if (DevelopmentToos.DistanceForTarget(transform, EnemyManager.MainInstance.GetCurTarget()) > 2f) return false;
        if (_animator.AnimationAtTag("Dodge")) return false;
        if (_animator.AnimationAtTag("Attack")) return false;
        if (_animator.AnimationAtTag("Hit")) return false;
        if (_currentdodgeColdTime > 0) return false;
        return true;
    }

    public override TaskStatus OnUpdate()
    {

        _currentdodgeColdTime -= Time.deltaTime;
        if (_enemyCombatControl.IsAttacker())
        {
            return TaskStatus.Success;
        }
        else
        {
            if (_animator.AnimationAtTag("Attack")) return TaskStatus.Running;
            _enemyMovementControl.SetLookTargetState(true);
            if (CanDodge() && !_animator.AnimationAtTag("FinalityHit")) 
            {
                _currentdodgeColdTime = Random.Range(_DodgeColdTime.x,_DodgeColdTime.y);
                int i =Random.Range(0, 3);
                switch (i)
                {
                    case 0: _animator.Play("Dodge_Back"); break;
                    case 1: _animator.Play("Dodge_Right"); break;
                    case 2: _animator.Play("Dodge_Left"); break;
                }

                _curActionTime = _actionTime;
                return TaskStatus.Success;
            }
            
            _curActionTime -= Time.deltaTime;
            if (_curActionTime <= 0)
            {
                if (DevelopmentToos.DistanceForTarget(transform, EnemyManager.MainInstance.GetCurTarget()) < 6f)
                {
                    int tempX = Random.Range(-1, 2);

                    DevelopmentToos.WTF(tempX);

                    _enemyMovementControl.SetAIInput(tempX, -1);
                }
                else if (DevelopmentToos.DistanceForTarget(transform, EnemyManager.MainInstance.GetCurTarget()) > 10f)
                {
                    _enemyMovementControl.SetAIInput(0, 1);
                }
                else
                {
                    UpdateMovement();
                    _actionTime = Random.Range(1, 4);
                }
                 _curActionTime = _actionTime;
            }

            return TaskStatus.Running;
        }

    }

    private void UpdateMovement()
    {
        int index = Random.Range(1, 7);
        int run = GetIsRun();
        switch (index)
        {
            case 1: _enemyMovementControl.SetAIInput(1, 0, run); break;
            case 2: _enemyMovementControl.SetAIInput(-1, 0, run); break;
            case 3: _enemyMovementControl.SetAIInput(1, 1, run); break;
            case 4: _enemyMovementControl.SetAIInput(1, -1, run); break;
            case 5: _enemyMovementControl.SetAIInput(-1, 1, run); break;
            case 6: _enemyMovementControl.SetAIInput(-1, -1, run); break;
        }
        
    }

    private int GetIsRun()
    {
        int run = Random.Range(1, 6);
        if (run == 1)
        {
            run = 2;
        }
        else
        {
            run = 1;
        }
        return run;
    }
}
