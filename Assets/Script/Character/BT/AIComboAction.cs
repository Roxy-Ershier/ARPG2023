using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommonAttack : Action
{

    private EnemyCombatControl _enemyCombatControl;
    private EnemyMovementControl _enemyMovementControl;

    public override void OnStart()
    {
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
        _enemyMovementControl = GetComponent<EnemyMovementControl>();
    }

    public override TaskStatus OnUpdate()
    {

        if (_enemyCombatControl.IsAttacker())
        {
            _enemyMovementControl.SetLookTargetState(false);
            _enemyCombatControl.CharacterBaseAttackInput();
            return TaskStatus.Success;
        }


        return TaskStatus.Running;
    }
}
