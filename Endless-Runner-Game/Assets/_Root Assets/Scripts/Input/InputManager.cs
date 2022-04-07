
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    // event for jump
    public delegate void JumpEvent();
    public event JumpEvent OnJumpInput;
    
    // event for crouch
    public delegate void CrouchEvent();
    public event CrouchEvent OnCrouchInput;
    
    // event for movement
    public delegate void MovementEvent(float movement);
    public event MovementEvent OnMovementInput;
    
    // event for pause
    public delegate void PauseEvent();
    public event PauseEvent OnPauseInput;
    
    private GameInputActions gameInput;


    private void Awake()
    {
        InitializeSingleton();
        InitializeInputActions();
    }

    private void OnEnable()
    {
        gameInput.Enable();
    }
    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void MovementOnStarted()
    {
        print("movement on stared");
    }
    private void MovementOnCanceled()
    {
        print("movement on canceled");
    }
    private void MovementOnPerformed(float movement)
    {
        if (OnMovementInput == null)
            return;

        OnMovementInput(movement);
        print($"movement: {movement}");
    }

    private void JumpOnPerformed()
    {
        if (OnJumpInput == null)
            return;

        OnJumpInput();
        print("jump on performed");
    }

    private void CrouchOnPerformed()
    {
        if (OnCrouchInput == null)
            return;

        OnCrouchInput();
        print("crouch on performed");
    }

    private void PauseOnPerformed()
    {
        if (OnPauseInput == null)
            return;

        OnPauseInput();
        print("pause on performed");
    }

    private void InitializeInputActions()
    {
        gameInput = new GameInputActions();

        gameInput.Player.Jump.performed += _ => JumpOnPerformed();
        gameInput.Player.Crouch.performed += _ => CrouchOnPerformed();

        gameInput.Player.Movement.started += _ => MovementOnStarted();
        gameInput.Player.Movement.performed += ctx => MovementOnPerformed(ctx.ReadValue<float>());
        gameInput.Player.Movement.canceled += _ => MovementOnCanceled();

        gameInput.UI.Pause.performed += _ => PauseOnPerformed();
    }
    
    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
