using UnityEngine;
using Zenject;

public class GameUIPresenter : IInitializable, ILateDisposable
{
    private readonly GameUIView _gameUIView;
    private readonly UIConfig _UIConfig;

    public event System.Action OnRestartPerformed;

    public GameUIPresenter(GameUIView gameUIView, UIConfig uIConfig)
    {
        _gameUIView = gameUIView;
        _UIConfig = uIConfig;
    }

    public void Initialize()
    {
        _gameUIView.OnRestartButtonClicked += HandleOnRestartClicked;
    }

    public void LateDispose()
    {
        _gameUIView.OnRestartButtonClicked -= HandleOnRestartClicked;
    }

    public void HandleGameEndedEvent(bool hasWon)
    {
        string uiText = hasWon ? _UIConfig.WinMessage : _UIConfig.LoseMessage;
        _gameUIView.ShowEndGamePanel(uiText);
    }

    public void HideEndGamePanel()
    {
        _gameUIView.HideEndGamePanel();
    }

    private void HandleOnRestartClicked()
    {
        OnRestartPerformed?.Invoke();
    }
}
