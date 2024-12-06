using UnityEngine;

using SimpleInputs;

public class SphereControl : MonoBehaviour
{
    [SerializeField] private Inputs inputs;

    private readonly float moveSpeed = 4f;

    private void Start()
    {
        inputs.PlayerInputs.Fire.OnStarted.AddListener(Fire);
        inputs.PlayerInputs.Jump.OnCanceled.AddListener(Jump);
    }

    private void Update()
    {
        Vector2 moveInput = inputs.PlayerInputs.Move.Value;
        transform.position += moveSpeed * Time.deltaTime * new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void Fire()
    {
        Debug.Log("Fire");
    }

    private void Jump(float duration)
    {
        Debug.Log($"Jump, press duration = {duration}s");
    }
}
