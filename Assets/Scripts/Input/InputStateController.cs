using UnityEngine;

public class InputStateController
{
    private readonly GameplayInput _gameplayInput;
    private readonly GameUIInput _gameUIInput;

    public InputStateController(GameplayInput gameplayInput, GameUIInput gameUIInput)
    {
        _gameplayInput = gameplayInput;
        _gameUIInput = gameUIInput;
    }

    public void EnableGameplayInput()
    {
        _gameUIInput.Disable();
        _gameplayInput.Enable();
    }

    public void EnableUIInput()
    {
        _gameplayInput.Disable();
        _gameUIInput.Enable();
    }
}
