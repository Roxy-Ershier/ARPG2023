using BehaviorDesigner.Runtime.Tasks;
using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWalkToTarget : Action
{

    private EnemyCombatControl _enemyCombatControl;
    private EnemyMovementControl _enemyMovementControl;

    public override void OnAwake()
    {
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
        _enemyMovementControl = GetComponent<EnemyMovementControl>();
    }
    public override void OnStart()
    {
        _enemyMovementControl.SetLookTargetState(true);
    }
    public override TaskStatus OnUpdate()
    {
        
        if (DevelopmentToos.DistanceForTarget(transform, EnemyManager.MainInstance.GetCurTarget()) < _enemyCombatControl.GetCurAttackDistance())
        {
            return TaskStatus.Success;
        }
        else
        {
            _enemyMovementControl.SetAIInput(0, 1, 2f);
            return TaskStatus.Running;
        }
    }
}
