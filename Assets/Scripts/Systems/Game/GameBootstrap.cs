using UnityEngine;
using Zenject;

public class GameBootstrap : MonoBehaviour
{
    private GameLifecycleController _gameLifecycleController;

    [Inject]
    public void Construct(GameLifecycleController gameLifecycleController)
    {
        _gameLifecycleController = gameLifecycleController;
    }

    private void Start()
    {
        _gameLifecycleController.StartGame();
    }
}
