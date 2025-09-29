using System;
using System.Collections.Generic;
using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Difficulty;
using Gameplay.Current._99Balls.Pins;
using Gameplay.Current.Configs;
using PT.Tools.Helper;
using PT.Tools.ObjectPool;
using UnityEngine;
using Zenject;

namespace Gameplay.Current._99Balls.Interactables
{
    public class InteractablesFactory : MonoBehaviour
    {
        [SerializeField] private SerializableKeyValue<InteractableTypeEnum, GameObject> interactablesPrefabs;
        [SerializeField] private Transform interactablesParent;

        [Inject] private GameInfoConfig _gameInfoConfig;
        
        private Dictionary<InteractableTypeEnum, ObjectPool> _interactablesPools = new();
        
        private void Awake()
        {
            foreach (var interactablePrefab in interactablesPrefabs.Dictionary)
            {
                _interactablesPools.Add(interactablePrefab.Key, new());
                _interactablesPools[interactablePrefab.Key].Init(interactablePrefab.Value, interactablesParent, 25);
            }
        }

        public IInteractable Spawn(
            ElementModel model, Action<IInteractable, Ball> onInteracted, 
            Vector2 position, DifficultyInfo difficultyInfo)
        {
            if (!_interactablesPools.ContainsKey(model.Type))
            {
                DebugManager.Log(DebugCategory.Errors, $"Interactable type {model.Type} not found in pools.", LogType.Error); 
                return null;
            }

            var interactableObj = _interactablesPools[model.Type].Get();
            interactableObj.transform.position = position;
            
            var interactable = interactableObj.GetComponent<IInteractable>();
            
            switch (model.Type)
            {
                case InteractableTypeEnum.PinBox:
                case InteractableTypeEnum.PinCircle:
                case InteractableTypeEnum.PinTriangle:
                case InteractableTypeEnum.PinRhombus:
                    var pin = interactable.Transform.gameObject.GetComponent<Pin>();
                    pin.Init(
                        _gameInfoConfig.MaxPinValue, 
                        difficultyInfo.PinValueRange.GetRandomValue());
                    break;
            }

            interactable.Init(model, onInteracted);

            return interactable;
        }
        
        public void Return(IInteractable interactable)
        {
            if (!_interactablesPools.ContainsKey(interactable.Type))
            {
                DebugManager.Log(DebugCategory.Errors, $"Interactable type {interactable.Type} not found in pools.", LogType.Error); 
                return;
            }

            _interactablesPools[interactable.Type].Set(interactable.Transform.gameObject);
        }
    }
}