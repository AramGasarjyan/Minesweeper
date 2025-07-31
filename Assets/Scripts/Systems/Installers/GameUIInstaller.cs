using UnityEngine;
using Zenject;

public class GameUIInstaller : MonoInstaller
{
    [SerializeField] private UIConfig _UIConfig;
    [SerializeField] private GameUIView _gameUIView;

    public override void InstallBindings()
    {
        Container.Bind<UIConfig>().FromInstance(_UIConfig).AsSingle();
        Container.Bind<GameUIView>().FromInstance(_gameUIView).AsSingle();

        Container.BindInterfacesAndSelfTo<GameUIPresenter>().AsSingle().NonLazy();
    }
}
