using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.UI.LoadingCurtain;
using CodeBase.Services.LogService;
using CodeBase.Services.PlayerProgressService;
using CodeBase.Services.SaveLoadService;
using CodeBase.Services.StaticDataService;
using CodeBase.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameplayState : IState
    {
        private readonly ILoadingCurtain loadingCurtain;
        private readonly ISceneLoader sceneLoader;
        private ILogService log;
        private IAssetProvider assetProvider;
        private IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        [Inject] private readonly IGameFactory gameFactory;

        public GameplayState(ILoadingCurtain loadingCurtain, ISceneLoader sceneLoader, ILogService log, IAssetProvider assetProvider, StaticDataService staticDataService, IPersistentProgressService progressService)
        {
            this.loadingCurtain = loadingCurtain;
            this.sceneLoader = sceneLoader;
            this.log = log;
            this.assetProvider = assetProvider;
            _staticData = staticDataService;
            _progressService = progressService;
            if (_staticData == null)
                Debug.Log("7777777777777");
        }

        public async UniTask Enter()
        {
            log.Log("Game mode 1 state enter");
            loadingCurtain.Show();
            await assetProvider.WarmupAssetsByLabel(AssetLabels.GameplayState);
            await sceneLoader.Load(InfrastructureAssetPath.GameMode1Scene);
            loadingCurtain.Hide();
            //gameFactory.CreateMoney();
            LevelStaticData levelData = LevelStaticData();
            GameObject hero = await gameFactory.CreateHero(levelData.InitialHeroPosition);
            await gameFactory.CreateEnemy(EnemyTypeId.Evrey, hero.transform);
            //InformProgressReaders();
            
        }

        private LevelStaticData LevelStaticData() =>
     _staticData.ForLevel(SceneManager.GetActiveScene().name);

       /* private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }*/

        public async UniTask Exit()
        {
            loadingCurtain.Show();
            await assetProvider.ReleaseAssetsByLabel(AssetLabels.GameplayState);
        }
    }
}