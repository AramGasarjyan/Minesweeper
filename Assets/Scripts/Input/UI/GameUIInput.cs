using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameUIInput : IInitializable, ILateDisposable
{
    private readonly MinesweeperInputActions _inputActions;
    private readonly GameUIPresenter _gameUIPresenter;

    public event System.Action OnRestartAction;

    public GameUIInput(MinesweeperInputActions inputActions, GameUIPresenter gameUIPresenter)
    {
        _inputActions = inputActions;
        _gameUIPresenter = gameUIPresenter;
    }

    public void Initialize()
    {
        _inputActions.UIMap.Restart.performed += HandleRestartAction;
        _gameUIPresenter.OnRestartPerformed += HandleRestartAction;
    }

    public void LateDispose()
    {
        _inputActions.UIMap.Restart.performed -= HandleRestartAction;
        _gameUIPresenter.OnRestartPerformed -= HandleRestartAction;

        _inputActions.Disable();
    }

    public void Enable()
    {
        _inputActions.UIMap.Enable();
    }

    public void Disable()
    {
        _inputActions.UIMap.Disable();
    }

    private void HandleRestartAction()
    {
        OnRestartAction?.Invoke();
    }

    private void HandleRestartAction(InputAction.CallbackContext context)
    {
        HandleRestartAction();
    }
}
