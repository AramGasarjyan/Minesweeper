using UnityEngine;
using Zenject;

public class GameUIInputPerformer : IInitializable, ILateDisposable
{
    private readonly GameUIInput _gameUIInput;
    private readonly GameUIPresenter _gameUIPresenter;
    private readonly GameLifecycleController _gameLifecycleController;

    public GameUIInputPerformer(GameUIInput gameUIInput, GameUIPresenter gameUIPresenter, GameLifecycleController gameLifecycleController)
    {
        _gameUIInput = gameUIInput;
        _gameUIPresenter = gameUIPresenter;
        _gameLifecycleController = gameLifecycleController;
    }

    public void Initialize()
    {
        _gameUIInput.OnRestartAction += HandleRestartAction;
        _gameUIInput.OnRestartAction += HandleRestartAction;
    }

    public void LateDispose()
    {
        _gameUIInput.OnRestartAction -= HandleRestartAction;
        _gameUIInput.OnRestartAction -= HandleRestartAction;
    }

    private void HandleRestartAction()
    {
        _gameUIPresenter.HideEndGamePanel();
        _gameLifecycleController.RestartGame();
    }
}
