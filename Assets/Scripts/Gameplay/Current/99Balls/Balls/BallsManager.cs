using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Configs;
using Gameplay.General.Game;
using PT.Logic.Dependency;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using UnityEngine;
using Zenject;

namespace Gameplay.Current._99Balls.Balls
{
    public class BallsManager : MonoBehaviourEventListener
    {
        [SerializeField] private TargetTrigger ballsTargetTrigger;
        [Space]
        [SerializeField] private Transform initialPoint;
        [Space]
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private Transform ballsParent;

        public event Action OnAllBallsReturned;
        
        [Inject] private GameInfoConfig _gameInfoConfig;
        [Inject] private ShootController _shootController;
        [Inject] private IFactoryZenject _factory;
        
        private MonoBehPool<Ball> _ballsPool;

        private List<Ball> _currentBalls = new();

        private int _currentFallenBallsAmount;
        private int _ballsToAddAmount;

        private Vector2 _currentReturnedBallsPosition;
        
        private CancellationTokenSource _ballsSendingCts;
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameTurn, OnGameTurn },
                { GlobalEventEnum.PlayerTurn, OnPlayerTurn },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });

            _ballsPool = new(_factory);
            _ballsPool.Init(ballPrefab.gameObject, ballsParent, 100);
        }
        
        private void OnGameStarted()
        {
            _currentReturnedBallsPosition = initialPoint.position;
            
            _currentFallenBallsAmount = _ballsToAddAmount = 0;
            
            AddBall();
            
            _shootController.OnShoot += SendBalls;
            ballsTargetTrigger.OnTriggered += ReturnBall;
        }

        private void OnGameTurn()
        {
            // ballsTargetTrigger.OnTriggered -= ReturnBall;
            
            AddLaterBalls();
            
            _currentFallenBallsAmount = _ballsToAddAmount = 0;
        }
        private void OnPlayerTurn()
        {
            // ballsTargetTrigger.OnTriggered += ReturnBall;
        }

        private void OnGameEnded()
        {
            _shootController.OnShoot -= SendBalls;
            ballsTargetTrigger.OnTriggered -= ReturnBall;

            RemoveBalls();
            
            _currentFallenBallsAmount = _ballsToAddAmount = 0;

			_ballsSendingCts?.Cancel(); _ballsSendingCts?.Dispose(); _ballsSendingCts = null;
        }

        public void AddBallLater()
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Picked ball bonus, adding ball later");
            
            _ballsToAddAmount++;
        }

        private async void SendBalls(Vector2 finalPosition)
        {
			_ballsSendingCts = new();

            var curPos = _currentReturnedBallsPosition;

            try
            {
                foreach (var ball in _currentBalls)
                {
                    ball.Push((finalPosition - curPos).normalized, _gameInfoConfig.BallsShootSpeed);
                
                    await UniTask.WaitForSeconds(_gameInfoConfig.BallsShootDelay, cancellationToken: _ballsSendingCts.Token);
                }
            }
            catch (Exception e){}
        }

        private void ReturnBall(GameObject obj)
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Ball returned");
            
            var ball = obj.GetComponent<Ball>();
            
            ball.StopMoving();

            if (_currentFallenBallsAmount == 0)
            {
                _currentReturnedBallsPosition = new (ball.transform.position.x, initialPoint.position.y);
            }
            
            //move ball cleaner
            ball.transform.position = _currentReturnedBallsPosition;
            
            _currentFallenBallsAmount++;
            
            if (_currentFallenBallsAmount == _currentBalls.Count)
            {
                OnAllBallsReturned?.Invoke();
            }
        }

        private void AddLaterBalls()
        {
            for (int i = 0; i < _ballsToAddAmount; i++)
            {
                AddBall();
            }
        }

        private void AddBall()
        {
            var newBall = _ballsPool.Get();
            newBall.StopMoving();
            newBall.transform.position = _currentReturnedBallsPosition;
                    
            _currentBalls.Add(newBall);
        }

        private void RemoveBalls()
        {
            foreach (var ball in _currentBalls)
            {
                _ballsPool.Set(ball);
            }
            
            _currentBalls.Clear();
        }
    }
}