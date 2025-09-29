using Gameplay.Current._99Balls.Interactables;

namespace Gameplay.Current._99Balls
{
    public class ElementModel
    {
        public InteractableTypeEnum Type { get; private set; }
        public int Value { get; private set; }

        public ElementModel(InteractableTypeEnum type, int value = 0)
        {
            Type = type;
            Value = value;
        }
    }
}