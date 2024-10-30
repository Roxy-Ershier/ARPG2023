using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ComboDataInfo", menuName = "SO/ComboDataInfo")]
public class ComboDataInfo : ScriptableObject
{
    /*
     * 播放的动画
     * 过度时间
     * 受击动画
     * 格挡动画
     * 攻击最佳位置
     */
    [SerializeField] private string _comboName;
    [SerializeField] private string[] _comboHitName;
    [SerializeField] private string[] _comboParryName;
    [SerializeField] private float _damage;
    [SerializeField] private float _coldTime;
    [SerializeField] private float _comboPositionOffset;
    [SerializeField] private float[] _attackDistance;
    [SerializeField] private float[] _attackRange;

    [SerializeField] private float[] _shakeForce;
    [SerializeField] private float[] _shakeSpeed;
    [SerializeField] private float[] _shakeDuration;

    public AnimationCurve[] PositionMove;
    public float[] AnimationCurveDuration;


    public string ComboName => _comboName;
    public string[] ComboHitName => _comboHitName;
    public string[] ComboParryName => _comboParryName;
    public float Damage => _damage;
    public float ColdTime => _coldTime;
    public float ComboPositionOffset => _comboPositionOffset;
    public float[] AttackDistance => _attackDistance;
    public float[] AttackRange => _attackRange;

    public float[] ShakeForce => _shakeForce;
    public float[] ShakeSpeed => _shakeSpeed;
    public float[] ShakeDuration => _shakeDuration;

    public int GetCombeHitAndParryCountMax() => _comboHitName.Length;


}
