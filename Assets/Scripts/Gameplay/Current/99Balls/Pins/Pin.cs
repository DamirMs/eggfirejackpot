using System;
using Gameplay.Current._99Balls.Balls;
using Gameplay.Current._99Balls.Countables;
using Gameplay.Current._99Balls.Interactables;
using UniRx;
using UnityEngine;

namespace Gameplay.Current._99Balls.Pins
{
    public class Pin : MonoBehaviour, IInteractable, ICountable
    {
        [SerializeField] private SpriteRenderer sr;
        [Space]
        [SerializeField] private Color minColor = Color.green;
        [SerializeField] private Color maxColor = Color.red;

        public Transform Transform { get => transform; }
        public InteractableTypeEnum Type { get; private set; }

        public CountableModel CountableModel { get; private set; }

        private Action<IInteractable, Ball> _onInteracted;
        
        private IDisposable _valueSub;
        
        public void Init(ElementModel model, Action<IInteractable, Ball> onInteracted)
        {
            Type = model.Type;

            _onInteracted = onInteracted;
        }
        public CountableModel Init(int maxValue, int value)
        {
            CountableModel = new(maxValue);

            CountableModel.Set(value);
         
            _valueSub = CountableModel.CurrentValue
                .Subscribe(UpdateColor);
            
            return CountableModel;
        }
        
        public void Interact(Ball ball)
        {
            _onInteracted?.Invoke(this, ball);
        }

        private void UpdateColor(int value)
        {
            var t = (float)value / CountableModel.MaxValue;
            var midColor = Color.yellow;

            sr.color = t < 0.5f
                ? Color.Lerp(minColor, midColor, t * 2f)
                : Color.Lerp(midColor, maxColor, (t - 0.5f) * 2f);
        }
        
        private void OnDestroy()
        {
            _valueSub?.Dispose();
        }
    }
}