using Gameplay.Current._99Balls.Balls;

namespace Gameplay.Current._99Balls.Interactables
{
    public interface IInteractableHandler
    {
        void Handle(IInteractable interactable, Ball ball);
    }
}