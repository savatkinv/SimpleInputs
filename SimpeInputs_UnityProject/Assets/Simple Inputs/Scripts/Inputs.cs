using UnityEngine;

namespace SimpleInputs
{
    public class Inputs : MonoBehaviour
    {
        public bool pointerOverUI;
        public bool currentPointerMouse;
        public bool currentPointerTouchpad;
        public PlayerInputs PlayerInputs { get; private set; }

        private void Awake()
        {
            PlayerInputs = new PlayerInputs();
        }
    }
}