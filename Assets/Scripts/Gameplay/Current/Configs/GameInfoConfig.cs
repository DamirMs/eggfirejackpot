using Gameplay.General.Configs;
using UnityEngine;

namespace Gameplay.Current.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameInfo", fileName = "GameInfoConfig")]
    public class GameInfoConfig : BaseGameInfoConfig
    {
        [Space(20)]
        [Header("GAME settings:")]
        [SerializeField] private float ballsShootSpeed = 10f;
        [SerializeField] private float ballsShootDelay = 0.4f;
        [SerializeField] private int maxPinValue = 120;
        
        public float BallsShootSpeed => ballsShootSpeed;
        public float BallsShootDelay => ballsShootDelay;
        public int MaxPinValue => maxPinValue;
    }
}