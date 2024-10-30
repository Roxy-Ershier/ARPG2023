using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ExecuteCameraData", menuName = "SO/ExecuteCameraData")]

public class ExecuteCameraData : ScriptableObject
{
    [SerializeField] private List<GameObject> _lookTargets;
    [SerializeField] private Transform[] _position;
    [SerializeField] private float[] _stopTime;
    [SerializeField] private float[] _rotationsmoothTime;
    [SerializeField] private float[] _positionSmoothTime;



    public GameObject TryGetOneLookTarget(int index)
    {
        return _lookTargets[index];
    }
    public Transform TryGetOnePosition(int index)
    {
        return _position[index];
    }
    public float TryGetOneStopTime(int index)
    {
        return _stopTime[index];
    }
    public float TryGetOneRotationSmoothTime(int index)
    {
        return _rotationsmoothTime[index];
    }
    public float TryGetOnePositionSmoothTime(int index)
    {
        return _positionSmoothTime[index];
    }

    public int TryGetMaxIndex()=>_position.Length;

}
