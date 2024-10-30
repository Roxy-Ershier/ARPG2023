using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthDataSO", menuName = "SO/HealthDataSO")]
public class CharacterHealthDataSO : ScriptableObject
{
    [SerializeField] private float _hp_MAX;
    [SerializeField] private float _strength_MAX;




    public float HP_MAX => _hp_MAX;
    public float Strength_MAX => _strength_MAX;


}
