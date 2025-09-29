using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Countables;
using Gameplay.Current._99Balls.Difficulty;
using Gameplay.Current._99Balls.InteractablesLabels;
using Gameplay.Current.Configs;
using Gameplay.General.Game;
using Gameplay.General.Other;
using PT.Tools.EventListener;
using UnityEngine;
using Zenject;

namespace Gameplay.Current._99Balls.Interactables
{
    public class InteractablesManager : MonoBehaviourEventListener
    {
        [SerializeField] private InteractablesLabelsManager interactablesLabelsManager;
        [Space]
        [SerializeField] private Transform interactablesStartPoint;
        [SerializeField] private Transform interactablesEndPoint;
        [SerializeField] private int amountOfInteractablesInLine = 6;
        [SerializeField] private float interactablesYOffset = 2;
        [Space]
        [SerializeField] private InteractablesFactory interactablesFactory;
        [Space]
        [SerializeField] private TargetTrigger bottomTargetTrigger;

        public event Action OnInteractableReachedBottom;

        [Inject] private DiContainer _container;
        [Inject] private GameInfoConfig _gameInfoConfig;
        [Inject] private TurnCounter _turnCounter;
        [Inject] private DifficultyInfoProvider _difficultyInfoProvider;

        private List<IInteractable> _currentInteractables = new();

        private bool _pinReachedBottom;

        private Dictionary<InteractableTypeEnum, IInteractableHandler> _interactablesHandlers;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameTurn, OnGameTurn },
                { GlobalEventEnum.PlayerTurn, OnPlayerTurn },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });
         
            _interactablesHandlers = new()
            {
                // { InteractableTypeEnum.PinBox, _container.ResolveId<IInteractableHandler>(InteractableTypeEnum.PinBox) },
                { InteractableTypeEnum.PinCircle, _container.ResolveId<IInteractableHandler>(InteractableTypeEnum.PinCircle) },
                // { InteractableTypeEnum.PinTriangle, _container.ResolveId<IInteractableHandler>(InteractableTypeEnum.PinTriangle) },
                
                { InteractableTypeEnum.BallBonus, _container.ResolveId<IInteractableHandler>(InteractableTypeEnum.BallBonus) },
            };
        }
        
        private void OnGameStarted()
        {
            RemoveAllInteractables();
            
            bottomTargetTrigger.OnTriggered += InteractableReachedBottom;

            _pinReachedBottom = false;
            
            CreateNextLine();
        }

        private void OnGameTurn()
        {
        }
        private void OnPlayerTurn()
        {
        }

        private void OnGameEnded()
        {
            bottomTargetTrigger.OnTriggered += InteractableReachedBottom;
            
            RemoveAllInteractables();
        }

        public async UniTask MoveInteractablesDown(CancellationToken token)
        {
            DebugManager.Log(DebugCategory.Gameplay, "Moving interactables down");
            
            try
            {
                foreach (var interactable in _currentInteractables)
                {
                    interactable.Transform.position = (Vector2)interactable.Transform.position + Vector2.down * interactablesYOffset;
                }
            
                //dotween move
            
                await UniTask.WaitForSeconds(0.5f, cancellationToken: token);
                
                CreateNextLine();
            }
            catch (Exception e){}
        }
        
        public void RemoveInteractable(IInteractable interactable)
        {
            if (interactable == null) return;
            
            _currentInteractables.Remove(interactable);
            interactablesFactory.Return(interactable);
        }

        private void CreateNextLine()//separate to smaller methods
        {
            DebugManager.Log(DebugCategory.Gameplay, "Creating next line of interactables");
            
            var difficultyInfo = _difficultyInfoProvider.GetDifficultyInfo(_turnCounter.CurrentTurn.Value);
            var interactableSpawnChances = difficultyInfo.InteractableSpawnChances;

            for (int i = 0; i < amountOfInteractablesInLine; i++)
            {
                if (difficultyInfo.SpawnChance < Utils.GetRandomNext(1)) continue;

                var interactablesSpawnChancesList = new List<(InteractableTypeEnum, float)>();
                foreach (var kvp in interactableSpawnChances)
                    interactablesSpawnChancesList.Add((kvp.Key, kvp.Value));
        
                var typeToSpawn = Utils.PickByWeight<InteractableTypeEnum>(interactablesSpawnChancesList);

                var t = (float)i / (amountOfInteractablesInLine - 1);
                var spawnPos = Vector2.Lerp(interactablesStartPoint.position, interactablesEndPoint.position, t);

                var value = difficultyInfo.PinValueRange.GetRandomValue();
                var elementModel = new ElementModel(typeToSpawn, value);

                var interactable = interactablesFactory.Spawn(elementModel, OnInteractionHandle, spawnPos, difficultyInfo);

                _currentInteractables.Add(interactable);
                
                if (interactable is ICountable countable)
                {
                    Debug.Log("123 " + i) ;
                    
                    interactablesLabelsManager.AddLabel(interactable, countable.CountableModel);
                }
            }
        }

        private void OnInteractionHandle(IInteractable interactable, Ball ball)
        {
            _interactablesHandlers[interactable.Type].Handle(interactable, ball);
        }
        
        private void InteractableReachedBottom(GameObject go)
        {
            if (_pinReachedBottom) // rewrite to handle pins, add kill zone / return for other interactables
                return;
            _pinReachedBottom = true;
            
            OnInteractableReachedBottom?.Invoke();
        }

        private void RemoveAllInteractables()
        {
            foreach (var interactable in _currentInteractables)
            {
                interactablesFactory.Return(interactable);
            }

            _currentInteractables.Clear();
        }
    }
}