using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterComboSO", menuName = "SO/CharacterComboSO")]

public class CharacterComboSO : ScriptableObject
{
    [SerializeField] private List<ComboDataInfo> _allComboDate = new List<ComboDataInfo>();


    public string TryGetOneComboAction(int index)
    {
        if (index >= _allComboDate.Count) return null;

        return _allComboDate[index].ComboName;
    }

    public string TryGetOneHitName(int index, int hitIndex)
    {
        if (index >= _allComboDate.Count) return null;
        if (hitIndex >= _allComboDate[index].GetCombeHitAndParryCountMax()) return null;
        return _allComboDate[index].ComboHitName[hitIndex];
    }

    public string TryGetOneParryName(int index, int parryNIndex)
    {
        if (index >= _allComboDate.Count) return null;
        if (parryNIndex >= _allComboDate[index].GetCombeHitAndParryCountMax()) return null;
        return _allComboDate[index].ComboParryName[parryNIndex];
    }

    public float TryGetOneAttackDistance(int index, int hitIndex)
    {
        if (index >= _allComboDate.Count) return 0;
        if (hitIndex >= _allComboDate[index].GetCombeHitAndParryCountMax()) return 0;
        return _allComboDate[index].AttackDistance[hitIndex];
    }

    public float TryGetOneAttackRange(int index, int hitIndex)
    {
        if (index >= _allComboDate.Count) return 0;
        if (hitIndex >= _allComboDate[index].GetCombeHitAndParryCountMax()) return 0;
        return _allComboDate[index].AttackRange[hitIndex];
    }
    public float TryGetDamge(int index)
    {
        if (index >= _allComboDate.Count) return 0;

        return _allComboDate[index].Damage;
    }

    public float TryGetColdTime(int index)
    {
        if (index >= _allComboDate.Count) return 0;

        return _allComboDate[index].ColdTime;
    }
    public float TryGetComboPositionOffset(int index)
    {
        if (index >= _allComboDate.Count) return 0;

        return _allComboDate[index].ComboPositionOffset;
    }

    public float TryGetAnimationCurveDuration(int index, int hitIndex)
    {
        if (index >= _allComboDate.Count) return 0;

        return _allComboDate[index].AnimationCurveDuration[hitIndex];
    }

    public float TryGetShakeDuration(int index,int hitIndex)
    {
        if (index >= _allComboDate.Count) return 0;

        return _allComboDate[index].ShakeDuration[hitIndex];
    }
    public float TryGetShakeForce(int index, int hitIndex)
    {
        if (index >= _allComboDate.Count) return 0;

        return _allComboDate[index].ShakeForce[hitIndex];
    }
    public float TryGetShakeSpeed(int index, int hitIndex)
    {
        if (index >= _allComboDate.Count) return 0;

        return _allComboDate[index].ShakeSpeed[hitIndex];
    }

    public AnimationCurve TryGetAnimationCurve(int index, int hitIndex)
    {
        if (index >= _allComboDate.Count) return null;

        return _allComboDate[index].PositionMove[hitIndex];
    }

    public int TryGetHitOrParryMaxCount(int index) => _allComboDate[index].GetCombeHitAndParryCountMax();

    public int TryGetComboMaxCount() => _allComboDate.Count;

}
