using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Interactables;

namespace Gameplay.Current._99Balls.InteractablesHandlers
{
    public class BallBonusInteractableHandler : IInteractableHandler
    {
        private readonly BallsManager _ballsManager;
        private readonly InteractablesManager _interactablesManager;

        public BallBonusInteractableHandler(BallsManager ballsManager, InteractablesManager interactablesManager)
        {
            _ballsManager = ballsManager;
            _interactablesManager = interactablesManager;
        }
        
        public void Handle(IInteractable interactable, Ball ball)
        {
            _ballsManager.AddBallLater();
            
            _interactablesManager.RemoveInteractable(interactable);
        }
    }
}