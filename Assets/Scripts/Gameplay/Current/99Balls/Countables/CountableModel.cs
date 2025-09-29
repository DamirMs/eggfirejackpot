using UniRx;

namespace Gameplay.Current._99Balls.Countables
{
    public class CountableModel
    {
        public IReadOnlyReactiveProperty<int> CurrentValue => _currentValue;
        public int MaxValue => _maxValue;
        
        private readonly ReactiveProperty<int> _currentValue = new();
        
        private int _maxValue;
        
        public CountableModel(int maxValue)
        {
            _maxValue = maxValue;
            _currentValue.Value = 0;
        }
        
        public void Set(int value)
        {
            if (value < 0 || value > _maxValue)
                throw new System.ArgumentOutOfRangeException(nameof(value), $"Value must be between 0 and {_maxValue}");
            
            _currentValue.Value = value;
        }
        
        public void Decrease()
        {
            if (_currentValue.Value > 0) _currentValue.Value--;
        }
    }
}