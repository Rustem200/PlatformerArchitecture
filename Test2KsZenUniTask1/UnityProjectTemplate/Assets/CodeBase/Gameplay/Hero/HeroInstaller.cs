using Zenject;

public class HeroInstaller : Installer<HeroInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<State>().AsSingle();
    }
}
