using UnityEngine;

namespace SimpleInputs
{
    public class PlayerInputs
    {
        public InputLocker inputLocker;
        public InputValue<float> Fire;
        public InputValue<float> FireAlt;
        public InputValue<float> Jump;
        public InputValue<float> Run;
        public InputValue<float> Escape;
        public InputValue<Vector2> Move;
        public InputValue<Vector2> Look;

        public PlayerInputs()
        {
            inputLocker = new InputLocker();

            Fire = new InputValue<float>(inputLocker);
            FireAlt = new InputValue<float>(inputLocker);
            Jump = new InputValue<float>(inputLocker);
            Run = new InputValue<float>(inputLocker);

            Move = new InputValue<Vector2>(inputLocker);
            Look = new InputValue<Vector2>(inputLocker);

            Escape = new InputValue<float>(null);
        }
    }
}