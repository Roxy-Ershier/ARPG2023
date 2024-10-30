using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatControl : CharacterCombatControlBase
{
    [SerializeField] private bool _isAttacker;

    [SerializeField]private List<CharacterComboSO> _combinations;
    protected override void Start()
    {
        base.Start();
        _currentTarget = EnemyManager.MainInstance.GetCurTarget();
    }

    public bool IsAttacker() => _isAttacker;


    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.B))
        {
            CharacterBaseAttackInput();
        }
    }
    public override void CharacterBaseAttackInput()
    {
        if (!CanBaseAttackInput())
        {
            return;
        }
        int index = Random.Range(0, _combinations.Count);

        ChangeTheCombo(_combinations[index]);

        ExecuteComboInfo();
        _isAttacker = false;
    }

    public float GetCurAttackDistance()
    {
        if (_currentCombo == null)
        {
            _currentCombo = _baseCombo;
        }
        return _currentCombo.TryGetOneAttackDistance(_currentComboIndex, _hitIndex);
    }

    public void SetAttackCommand(bool value)
    {
        _isAttacker = value;
    }
}
