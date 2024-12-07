using UnityEngine.Events;

namespace SimpleInputs
{
    public class InputLocker
    {
        public UnityEvent<bool> OnChanged = new ();

        private bool isEnabled = false;
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnChanged?.Invoke(isEnabled);
                }
            }
        }
    }
}