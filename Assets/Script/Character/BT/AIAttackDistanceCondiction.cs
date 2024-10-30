using BehaviorDesigner.Runtime.Tasks;
using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackDistanceCondiction : Conditional
{
    private EnemyCombatControl _enemyCombatControl;

    public override void OnAwake()
    {
        _enemyCombatControl=GetComponent<EnemyCombatControl>();
    }

    public override TaskStatus OnUpdate()
    {
        if (DevelopmentToos.DistanceForTarget(transform, EnemyManager.MainInstance.GetCurTarget()) < _enemyCombatControl.GetCurAttackDistance()+1f) 
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

}
