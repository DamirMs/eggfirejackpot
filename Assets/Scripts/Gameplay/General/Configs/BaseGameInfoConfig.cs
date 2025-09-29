using Gameplay.General.Other;
using Gameplay.General.Other.Orientation;
using UnityEngine;

namespace Gameplay.General.Configs
{
    public class BaseGameInfoConfig : ScriptableObject
    {
        [SerializeField][Min(0.001f)] private float conditionMetDelay = 0.2f;
        [SerializeField][Min(0.001f)] private float gameOverDelay = 1; 
        [Space]
        [SerializeField] private bool resetsInputAfterRelease;
        [Space]
        [SerializeField][Min(0.001f)] private float loadingAnimationDuration;
        [Space]
        [SerializeField][Min(0.001f)] private OrientationSetter.OrientationMode orientationMode = OrientationSetter.OrientationMode.Both;
        
        public float ConditionMetDelay => conditionMetDelay;
        public float GameOverDelay => gameOverDelay;
        public bool ResetsInputAfterRelease => resetsInputAfterRelease;
        public float LoadingAnimationDuration => loadingAnimationDuration;
        public OrientationSetter.OrientationMode OrientationMode => orientationMode;
    }
}