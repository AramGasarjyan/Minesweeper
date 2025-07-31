using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private TileViewSettings _tileViewSettings;

    [Header("Scene Objects")]
    [SerializeField] private BoardView _boardView;
    [SerializeField] private Camera _mainCamera;

    public override void InstallBindings()
    {
        BindTilesFactory();
        BindBoard();

        BindInput();
        BindSceneObjects();
        BindGameLifeCycle();
    }

    private void BindBoard()
    {
        Container.BindInterfacesAndSelfTo<Board>().AsSingle().WithArguments(_levelConfig.BoardWidth, _levelConfig.BoardHeight, _levelConfig.BoardMinesCount);
    }

    private void BindTilesFactory()
    {
        Container.Bind<TileViewSettings>().FromInstance(_tileViewSettings).AsSingle();
        Container.BindFactory<int, int, TileData, TileData.Factory>().AsSingle();

        Container.BindFactory<TileData, TileView, TileView.Factory>()
            .FromComponentInNewPrefab(_tileViewSettings.TilePrefab).AsSingle();
    }

    private void BindInput()
    {
        Container.Bind<MinesweeperInputActions>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<GameplayInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayInputPerformer>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<GameUIInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameUIInputPerformer>().AsSingle();

        Container.Bind<InputStateController>().AsSingle();
    }

    private void BindSceneObjects()
    {
        Container.BindInterfacesAndSelfTo<BoardView>().FromInstance(_boardView).AsSingle();
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
    }

    private void BindGameLifeCycle()
    {
        Container.BindInterfacesAndSelfTo<GameLifecycleController>().AsSingle();
    }
}
