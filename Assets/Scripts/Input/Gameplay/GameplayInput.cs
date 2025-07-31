using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameplayInput : IInitializable, ILateDisposable
{
    public delegate void MouseActionDelegate(Vector2 mouseScreenPosition);

    public event MouseActionDelegate OnRevealAction;
    public event MouseActionDelegate OnFlagAction;
    public event System.Action OnRestartAction;

    private readonly MinesweeperInputActions _inputActions;

    public GameplayInput(MinesweeperInputActions inputActions)
    {
        _inputActions = inputActions;
    }

    public void Initialize()
    {
        _inputActions.Gameplay.Reveal.performed += HandleRevealInput;
        _inputActions.Gameplay.Flag.performed += HandleFlagInput;
        _inputActions.Gameplay.Restart.performed += HandleRestartInput;
    }

    public void LateDispose()
    {
        _inputActions.Gameplay.Reveal.performed -= HandleRevealInput;
        _inputActions.Gameplay.Flag.performed -= HandleFlagInput;
        _inputActions.Gameplay.Restart.performed -= HandleRestartInput;
    }

    public void Enable()
    {
        _inputActions.Gameplay.Enable();
    }

    public void Disable()
    {
        _inputActions.Gameplay.Disable();
    }

    private void HandleRevealInput(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = GetMouseScreenPosition();
        OnRevealAction?.Invoke(mousePosition);
    }

    private void HandleFlagInput(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = GetMouseScreenPosition();
        OnFlagAction?.Invoke(mousePosition);
    }

    private void HandleRestartInput(InputAction.CallbackContext context)
    {
        OnRestartAction?.Invoke();
    }

    private Vector2 GetMouseScreenPosition()
    {
        return _inputActions.Gameplay.PointerPosition.ReadValue<Vector2>();
    }
}
