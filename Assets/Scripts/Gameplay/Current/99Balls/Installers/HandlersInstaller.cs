using Gameplay.Current._99Balls.Interactables;
using Gameplay.Current._99Balls.InteractablesHandlers;
using Zenject;

namespace Gameplay.Current._99Balls.Installers
{
    public class HandlersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInteractableHandler>().WithId(InteractableTypeEnum.PinBox).To<PinInteractableHandler>().AsTransient();
            Container.Bind<IInteractableHandler>().WithId(InteractableTypeEnum.PinCircle).To<PinInteractableHandler>().AsTransient();
            Container.Bind<IInteractableHandler>().WithId(InteractableTypeEnum.PinTriangle).To<PinInteractableHandler>().AsTransient();
            
            Container.Bind<IInteractableHandler>().WithId(InteractableTypeEnum.BallBonus).To<BallBonusInteractableHandler>().AsTransient();
        }
    }
}