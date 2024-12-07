using UnityEngine;

namespace SimpleInputs
{
    public class Inputs : MonoBehaviour
    {
        public bool pointerOverUI;
        public bool currentPointerMouse;
        public bool currentPointerTouchpad;
        public GameState gameState;
        public PlayerInputs PlayerInputs { get; private set; }

        private void Awake()
        {
            PlayerInputs = new PlayerInputs();
        }
    }
        
    public enum GameState { Game, Menu };
}