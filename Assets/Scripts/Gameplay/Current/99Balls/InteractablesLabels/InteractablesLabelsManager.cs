using System.Collections.Generic;
using System.Linq;
using Gameplay.Current._99Balls.Countables;
using Gameplay.Current._99Balls.Interactables;
using PT.Logic.Dependency;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using UnityEngine;
using Zenject;

namespace Gameplay.Current._99Balls.InteractablesLabels
{
    public class InteractablesLabelsManager : MonoBehaviourEventListener
    {
        [SerializeField] private InteractableViewLabel labelPrefab;
        [SerializeField] private Transform labelsParent;
        
        [Inject] private IFactoryZenject _factory;
        
        private Dictionary<IInteractable, InteractableViewLabel> _interactablesLabels = new();
        
        private MonoBehPool<InteractableViewLabel> _interactablesLabelsPool;

        private List<IInteractable> _interactablesToRemove;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });
            
            _interactablesLabelsPool = new(_factory);
            _interactablesLabelsPool.Init(labelPrefab.gameObject, labelsParent, 100);
        }
        
        private void OnGameStarted()
        {
        }
        private void OnGameEnded()
        {
            RemoveAllLabels();
        }
        
        private void LateUpdate()
        {
            _interactablesToRemove = new();

            MoveLabels();
            RemoveLabels();
        }

        public void AddLabel(IInteractable interactable, CountableModel countableModel)
        {
            if (_interactablesLabels.ContainsKey(interactable)) return;

            var label = _interactablesLabelsPool.Get();
            label.Init(countableModel);
            
            _interactablesLabels.Add(interactable, label);
        }
        
        private void MoveLabels()
        {
            foreach (var label in _interactablesLabels)
            {
                if (!label.Key.Transform.gameObject.activeSelf)
                {
                    _interactablesToRemove.Add(label.Key);
                }
                else 
                {
                    label.Value.transform.position = label.Key.Transform.position;
                }
            }
        }
        private void RemoveLabels()
        {
            foreach (var interactable in _interactablesToRemove)
            {
                var label = _interactablesLabels[interactable];
                _interactablesLabelsPool.Set(label);
                _interactablesLabels.Remove(interactable);
            }
        }

        private void RemoveAllLabels()
        {
            _interactablesToRemove = _interactablesLabels.Keys.ToList();
            RemoveLabels();

            _interactablesLabels.Clear();
        }
    }
}