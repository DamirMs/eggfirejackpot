using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Countables;
using Gameplay.Current._99Balls.Interactables;
using Gameplay.General.Other;
using Gameplay.General.Score;

namespace Gameplay.Current._99Balls.InteractablesHandlers
{
    public class PinInteractableHandler : IInteractableHandler
    {
        private readonly ScoreManager _scoreManager;
        private readonly SoundManager _soundManager;
        private readonly InteractablesManager _interactablesManager;

        public PinInteractableHandler(ScoreManager scoreManager, SoundManager soundManager, InteractablesManager interactablesManager)
        {
            _scoreManager = scoreManager;
            _soundManager = soundManager;
            _interactablesManager = interactablesManager;
        }

        public void Handle(IInteractable interactable, Ball ball)
        {
            if (interactable is ICountable countable)
            {
                countable.CountableModel.Decrease();

                if (countable.CountableModel.CurrentValue.Value <= 0)
                {
                    _interactablesManager.RemoveInteractable(interactable);
                }
            }

            _scoreManager.UpdateScore(1);
            // _soundManager.PlaySound(SoundManager.SoundEventEnum.FinishReached);
        }
    }
}