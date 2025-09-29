using UnityEngine;

namespace Gameplay.Current._99Balls.Difficulty
{
    
    [CreateAssetMenu(menuName = "Configs/DifficultyInfo", fileName = "DifficultyInfoConfig")]
    public class DifficultyInfoConfig : ScriptableObject
    {
        [SerializeField] private DifficultyInfo[] difficultyInfos;

        public DifficultyInfo[] DifficultyInfos => difficultyInfos;
    }
}