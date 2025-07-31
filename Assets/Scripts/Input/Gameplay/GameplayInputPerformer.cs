using UnityEngine;
using Zenject;

public class GameplayInputPerformer : IInitializable, ILateDisposable
{
    private readonly GameplayInput _gameInput;
    private readonly BoardView _boardView;
    private readonly Board _board;

    private readonly GameLifecycleController _gameLifecycleController;
    private readonly Camera _mainCamera;


    public GameplayInputPerformer(GameplayInput gameInput, GameLifecycleController gameLifecycleController,
        BoardView boardView, Board board, Camera mainCamera)
    {
        _gameInput = gameInput;
        _boardView = boardView;
        _board = board;
        _gameLifecycleController = gameLifecycleController;
        _mainCamera = mainCamera;
    }


    public void Initialize()
    {
        _gameInput.OnRevealAction += HandleRevealAction;
        _gameInput.OnFlagAction += HandleFlagAction;
        _gameInput.OnRestartAction += HandleRestartAction;
    }

    public void LateDispose()
    {
        _gameInput.OnRevealAction -= HandleRevealAction;
        _gameInput.OnFlagAction -= HandleFlagAction;
        _gameInput.OnRestartAction -= HandleRestartAction;
    }


    private void HandleRevealAction(Vector2 mousePosition)
    {
        ConvertScreenPosToWorld(ref mousePosition);

        if (_boardView.TryGetTileCoordinates(mousePosition, out Vector2Int coordinates))
        {
            _board.RevealTile(coordinates.y, coordinates.x);
        }
    }

    private void HandleFlagAction(Vector2 mousePosition)
    {
        ConvertScreenPosToWorld(ref mousePosition);

        if (_boardView.TryGetTileCoordinates(mousePosition, out Vector2Int coordinates))
        {
            _board.PlaceFlag(coordinates.y, coordinates.x);
        }
    }

    private void HandleRestartAction()
    {
        _gameLifecycleController.RestartGame();
    }

    private void ConvertScreenPosToWorld(ref Vector2 screenPosition)
    {
        screenPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
    }
}
