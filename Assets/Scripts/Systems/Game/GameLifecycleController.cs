using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLifecycleController : IInitializable, ILateDisposable
{
    private readonly InputStateController _inputStateController;

    private readonly BoardView _boardView;
    private readonly Board _board;

    private readonly GameUIPresenter _gameUIPresenter;
    private readonly List<IResetable> _resetables;

    public GameLifecycleController(InputStateController inputStateController,
        Board board, BoardView boardView, GameUIPresenter gameUIPresenter, List<IResetable> resetables)
    {
        _inputStateController = inputStateController;

        _boardView = boardView;
        _board = board;

        _gameUIPresenter = gameUIPresenter;
        _resetables = resetables;
    }

    public void Initialize()
    {
        _board.OnGameEnded += EndGame;
    }

    public void LateDispose()
    {
        _board.OnGameEnded -= EndGame;
    }


    public void StartGame()
    {
        _inputStateController.EnableGameplayInput();

        _boardView.CreateBoardView();
    }

    public void RestartGame()
    {
        foreach (var resetable in _resetables)
        {
            resetable.Reset();
        }

        StartGame();
    }

    public void EndGame(bool hasPlayerWon)
    {
        _inputStateController.EnableUIInput();
        _gameUIPresenter.HandleGameEndedEvent(hasPlayerWon);
    }
}
