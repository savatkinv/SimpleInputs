# SimpleInputs

Package based on new Unity Input System. Include Keyboard, Gamepad and Touch control schemes.

## Integration

1. Import SimpleInputs package to Unity project.
2. Make sure that scene contain `EventSystem` with `InputSystemUIInputModule`
3. Add `Assets/Simple Inputs/Prefabs/Simple Inputs.prefab` to scene

## Using
Link `Inputs` from added prefab to `inputs` field and use input values:
```csharp
[SerializeField] private Inputs inputs;

private void Start()
{
   inputs.PlayerInputs.Fire.OnStarted.AddListener(Fire);
   inputs.PlayerInputs.Jump.OnCanceled.AddListener(Jump);
   inputs.PlayerInputs.Look.OnChangedValue.AddListener(TurnTo);
}

private void Update()
{
   Move(inputs.PlayerInputs.Move.Value);
}
```

## Linking inputs with Zenject
Bind `Inputs` as single in installer:
```csharp
Container.Bind<Inputs>().AsSingle();
```
and inject Inputs for simple linking
```csharp
[Inject] private readonly Inputs input;
```

## Adding new input values
1. Add new Action in `Assets/Simple Inputs/Settings/GameInputActions.inputactions`, for example "Crouch" as Button (Important setup control schemes)
2. Open `PlayerInputs.cs` and add new input value:
```csharp
public InputValue<float> Crouch;

public PlayerInputs()
{
    ...
    Crouch = new InputValue<float>(inputLocker);
}
```
3. For integration to BaseInput (keyboard/mouse and gamepad) open `PlayerInputBinding.cs` in `Assets/Simple Inputs/Scripts/BaseInput` and add:
```csharp
BindInput(playerInput.actions.FindAction("Crouch"), playerInputs.Crouch);
```
4. For integration to MobileInput open `MobileInput.cs` in `Assets/Simple Inputs/Scripts/MobileInput` and add:
```csharp
public void OnCrouch(bool crouch)
{
    inputs.PlayerInputs?.Crouch.SetValue(crouch ? 1f : 0);
}
```
5. Add on canvas `UI Virtual Button` prefab from `Assets/Simple Inputs/Prefabs/VirtualInputs` and attach `OnCrouch` function to button output event

## Docs
`Inputs.cs` (in prefab: `SimpleInputs/Inputs`) contain all input values:
   - `pointerOverUI `- true if pointer over ui
   - `currentPointerMouse` - true if current pointer is mouse
   - `currentPointerTouchpad` - true while current pointer use mobile ui touchpad
   - `GameState` - `Game` or `Menu` states for control lock cursor in game and unlock in menu
   - `PlayerInputs` - basic player input values:
       - `inputLocker` - block input control
       - `InputValue` contain `Value`, `IsPressed`, `PressedDuration`, `PressTime` and events `OnStarted`, `OnCanceled`, `OnChangedValue`
