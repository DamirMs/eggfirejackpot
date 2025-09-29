using System;
using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Interactables;
using UnityEngine;

namespace Gameplay.Current._99Balls.Bonuses
{
    public class BallBonus : MonoBehaviour, IInteractable
    {
        public Transform Transform { get => transform; }
        public InteractableTypeEnum Type { get; private set; }
        
        
        private Action<IInteractable, Ball> _onInteracted;
        
        public void Init(ElementModel model, Action<IInteractable, Ball> onInteracted)
        {
            Type = model.Type;

            _onInteracted = onInteracted;
        }

        public void Interact(Ball ball)
        {
            _onInteracted?.Invoke(this, ball);
        }
    }
}