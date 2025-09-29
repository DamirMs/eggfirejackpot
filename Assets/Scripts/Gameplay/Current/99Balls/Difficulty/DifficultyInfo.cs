using System;
using System.Collections.Generic;
using Gameplay.Current._99Balls.Interactables;
using NaughtyAttributes;
using PT.Tools.Helper;
using UnityEngine;

namespace Gameplay.Current._99Balls.Difficulty
{
    [Serializable]
    public class DifficultyInfo
    {
        [SerializeField] private SerializableKeyValue<InteractableTypeEnum, float> interactableSpawnChances;
        [SerializeField] private float spawnChance;
        [SerializeField] [MinMaxSlider(1, 120)] private Vector2Int pinValueRange;
        
        public float SpawnChance => spawnChance;
        public Dictionary<InteractableTypeEnum, float> InteractableSpawnChances => interactableSpawnChances.Dictionary;
        public Vector2Int PinValueRange => pinValueRange;
    }
}