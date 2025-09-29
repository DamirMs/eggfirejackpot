using System;
using Gameplay.Current._99Balls.Balls;
using UnityEngine;

namespace Gameplay.Current._99Balls.Interactables
{
    public interface IInteractable
    {
        Transform Transform { get; }
        InteractableTypeEnum Type { get; }

        void Init(ElementModel model, Action<IInteractable, Ball> onInteracted);
        void Interact(Ball ball);
    }
}