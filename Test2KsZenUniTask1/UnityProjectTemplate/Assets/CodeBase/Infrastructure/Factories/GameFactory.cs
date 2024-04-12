using Codebase.Gameplay.Hero;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.SaveLoadService;
using CodeBase.Services.StaticDataService;
using CodeBase.StaticData;
using CodeBase.UI.HUD;
using CodeBase.UI.Money;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
       /* public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();*/

        private readonly HUDRoot.Factory hudFactory;
        private readonly Money.Factory _moneyFactory;
        private readonly Hero.Factory _heroFactory;
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private GameObject _heroGameObject;


        public GameFactory(HUDRoot.Factory hudFactory, Money.Factory moneyFactory/*, Hero.Factory heroFactory*/, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            this.hudFactory = hudFactory;
            _moneyFactory = moneyFactory;
            //_heroFactory = heroFactory;
            _assets = assetProvider;
            _staticData = staticDataService;
        }
       

        public IHUDRoot CreateHUD() => hudFactory.Create();
        public IMoney CreateMoney() => _moneyFactory.Create();
        //public IHero CreateHero() => _heroFactory.Create();

      public async Cysharp.Threading.Tasks.UniTask<GameObject> CreateHero(Vector3 at) => 
            _heroGameObject = await _assets.Instantiate(InfrastructureAssetPath.Hero, at);

        public async UniTask<GameObject> CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
            EnemyStaticData enemyData = _staticData.ForMonster(typeId);

            GameObject prefab = await _assets.Load<GameObject>(enemyData.PrefabReference);
            GameObject enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            enemy.GetComponent<NavMeshAgent>().speed = enemyData.MoveSpeed;

            Attack attack = enemy.GetComponent<Attack>();
            attack.Construct(_heroGameObject.transform);
            attack.Damage = enemyData.Damage;
            attack.Cleavage = enemyData.Cleavage;
            attack.EffectiveDistance = enemyData.EffectiveDistance;

            enemy.GetComponent<AgentMoveToPlayer>()?.Construct(_heroGameObject.transform);
            enemy.GetComponent<RotateToHero>()?.Construct(_heroGameObject.transform);

            Debug.Log("CreateEnemy");
            return enemy;
        }
        public void Cleanup()
        {
            
        }

        /*private async Cysharp.Threading.Tasks.UniTask<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }*/

        /*private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }*/
    }
}

namespace CodeBase.StaticData
{
}