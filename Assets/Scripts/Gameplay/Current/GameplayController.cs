using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Interactables;
using Gameplay.General.Game;
using PT.Tools.EventListener;
using Zenject;

namespace Gameplay.Current
{
    public class GameplayController : LevelGameplayController
    {
        [Inject] private BallsManager _ballsManager;
        [Inject] private InteractablesManager _interactablesManager;

        
        protected override void SignUp()
        {
            _ballsManager.OnAllBallsReturned += AllBallsReturned; 
            _interactablesManager.OnInteractableReachedBottom += InteractableReachedBottom;
        }
        
        protected override void SignOut()
        {
            _ballsManager.OnAllBallsReturned -= AllBallsReturned; 
            _interactablesManager.OnInteractableReachedBottom -= InteractableReachedBottom;
        }
        
        private void AllBallsReturned()
        {
            DebugManager.Log(DebugCategory.Gameplay, "All balls returned, starting new game turn");
            
            GameTurn().Forget();
        }
        
        private void InteractableReachedBottom()
        {
            DebugManager.Log(DebugCategory.Gameplay, "interactable reached bottom, starting game over");
            
            GameOver().Forget();
        }
        
        protected override UniTask OnGameTurn(CancellationToken token)
        {
            return GameTurn(token);
        }

        private async UniTask GameTurn(CancellationToken token)
        {
            await _interactablesManager.MoveInteractablesDown(token);

            GlobalEventBus.On(GlobalEventEnum.PlayerTurn);
        }
    }
}