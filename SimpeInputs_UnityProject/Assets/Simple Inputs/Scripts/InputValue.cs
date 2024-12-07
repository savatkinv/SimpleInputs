using UnityEngine;
using UnityEngine.Events;

namespace SimpleInputs
{
    public class InputValue<TValue> where TValue : struct
    {
        public TValue Value { get; private set; }
        public bool IsPressed { get; private set; }
        public float PressedDuration => (inputLocker == null || !inputLocker.IsEnabled) && IsPressed ? Time.time - PressTime : 0f;
        public float PressTime { get; private set; }

        public UnityEvent OnStarted { get; private set; }
        public UnityEvent OnPerformed { get; private set; }
        public UnityEvent<float> OnCanceled { get; private set; }
        public UnityEvent<TValue> OnChangedValue { get; private set; }

        private readonly InputLocker inputLocker;

        public InputValue(InputLocker inputLocker)
        {
            this.inputLocker = inputLocker;

            OnStarted = new();
            OnPerformed = new();
            OnCanceled = new();
            OnChangedValue = new();

            inputLocker?.OnChanged.AddListener((isLocked) =>
                {
                    if (isLocked)
                    {
                        SetValue(default);
                    }
                });
        }

        public void Clear()
        {
            IsPressed = false;
            OnPerformed.RemoveAllListeners();
            OnCanceled.RemoveAllListeners();
            OnChangedValue.RemoveAllListeners();
        }

        public void SetValue(TValue value)
        {
            if (inputLocker != null && inputLocker.IsEnabled)
            {
                value = default;
            }

            if (!Value.Equals(value))
            {
                Value = value;
                OnChangedValue?.Invoke(Value);
            }

            bool previousIsPressed = IsPressed;
            IsPressed = (inputLocker == null || !inputLocker.IsEnabled) && !Value.Equals(default(TValue));

            if (IsPressed && !previousIsPressed)
            {
                PressTime = Time.time;
                OnStarted?.Invoke();
            }

            if (IsPressed && previousIsPressed)
            {
                OnPerformed?.Invoke();
            }

            if (!IsPressed && previousIsPressed)
            {
                OnCanceled?.Invoke(Time.time - PressTime);
            }
        }
    }
}