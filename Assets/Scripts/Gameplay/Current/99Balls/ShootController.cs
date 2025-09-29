using System;
using Gameplay.General.Input;
using PT.Tools.EventListener;
using UnityEngine;
using Zenject;

namespace Gameplay.Current._99Balls
{
    public class ShootController : MonoBehaviourEventListener
    {
        public event Action<Vector2> OnShoot;
        
        [Inject] private GameInputController _gameInputController;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.PlayerTurn, OnPlayerTurn },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });

        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _gameInputController.OnClick += StartAiming;
            _gameInputController.OnDrag += Aim;
            _gameInputController.OnRelease += Shoot;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _gameInputController.OnClick -= StartAiming;
            _gameInputController.OnDrag -= Aim;
            _gameInputController.OnRelease -= Shoot;
        }
        
        private void OnGameStarted()
        {
            _gameInputController.SetListeningInput();
        }
        
        private void OnPlayerTurn()
        {
            _gameInputController.SetListeningInput();
        }

        private void OnGameEnded()
        {
        }

        private void StartAiming(Vector2 position)
        {
            //draw line from initial position to mouse position
        }

        private void Aim(Vector2 position)
        {
            //draw line from initial position to mouse position
        }

        private void Shoot(Vector2 position)
        {
            
            
            OnShoot?.Invoke(position);
        }
    }
}