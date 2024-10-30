using GGG.Tool;
using Hua_yEnA.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

[RequireComponent(typeof(TP_Camera))]
public class ExecuteCamera : MonoBehaviour
{
    [SerializeField]private Camera _camera;
    [SerializeField]private ExecuteCameraData _executeCameraData;


    private Vector3 currentRotation;
    private int _currentIndex;

    private bool _isRun;


    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening("IntoExecuteTime", IntoExecuteTime);
    }

    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent("IntoExecuteTime", IntoExecuteTime);

    }

    private void LateUpdate()
    {
        if (_isRun)
        {
            UpdateCameraRotation();
            UpdateCameraPosition();
        }
        
    }

    [SerializeField] private List<Transform> _looktargets = new List<Transform>();
    [SerializeField] private List<Transform> _positions = new List<Transform>();

    private void UpdateCameraRotation()
    {
        transform.LookAtTarget(_looktargets[_currentIndex].position, _executeCameraData.TryGetOneRotationSmoothTime(_currentIndex));
    }

    private void UpdateCameraPosition()
    {
        transform.position = Vector3.Lerp(transform.position, _positions[_currentIndex].position, _executeCameraData.TryGetOnePositionSmoothTime(_currentIndex) * Time.deltaTime);
    }


    private void IntoExecuteTime()
    {

        GetComponent<TP_Camera>().SetCameraDistance(0.5f);

        GetComponent<TP_Camera>()._isEnable = false;
        _currentIndex = 0;
        transform.position = Camera.main.transform.position;
        _isRun = true;
        Invoke("NextIndex", _executeCameraData.TryGetOneStopTime(_currentIndex));

    }

    private void NextIndex()
    {
        _currentIndex++;
        if (_currentIndex < _executeCameraData.TryGetMaxIndex())
        {
            Invoke("NextIndex", _executeCameraData.TryGetOneStopTime(_currentIndex));
        }
        else
        {
            _currentIndex = 0;
            _isRun = false;
            GetComponent<TP_Camera>()._isEnable = true;
            GetComponent<TP_Camera>().SetCameraDistance(3f);

        }
    }

}
