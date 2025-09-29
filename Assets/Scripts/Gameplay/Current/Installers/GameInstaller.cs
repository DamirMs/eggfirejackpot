using Gameplay.Current._99Balls;
using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Difficulty;
using Gameplay.Current._99Balls.Interactables;
using Gameplay.General.Installers;
using Gameplay.General.Other;
using UnityEngine;

namespace Gameplay.Current.Installers
{
    public class GameInstaller : BaseGameInstaller
    {
        [SerializeField] private DifficultyInfoConfig difficultyInfoConfig;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<InteractablesManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BallsManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TurnCounter>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShootController>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<DifficultyInfoConfig>().FromInstance(difficultyInfoConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<DifficultyInfoProvider>().AsSingle();
        }
    }
}