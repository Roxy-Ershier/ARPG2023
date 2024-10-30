using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGG.Tool.Singleton;
public class GameInputManager :Singleton<GameInputManager> 
{

    private GameInputAction _inputAction;
    public Vector2 MovementVec => _inputAction.InputAction.Movement.ReadValue<Vector2>();
    public Vector2 CameraLookVec => _inputAction.InputAction.CameraLook.ReadValue<Vector2>();
    public bool LAttack => _inputAction.InputAction.LAttack.triggered;
    public bool RAttack => _inputAction.InputAction.RAttack.triggered;

    public bool LockUnLock => _inputAction.InputAction.LockUnLock.triggered;
    public bool Run => _inputAction.InputAction.Run.IsPressed();

    public bool Finality=>_inputAction.InputAction.Finality.triggered;

    public bool Dodge => _inputAction.InputAction.Dodge.triggered;
    public bool Defense => _inputAction.InputAction.RAttack.IsPressed();

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }
    protected override void Awake()
    {
        base.Awake();
        _inputAction ??=new GameInputAction();
    }





}
