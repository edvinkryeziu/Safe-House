using Michsky.MUIP;
using UnityEditor.EditorTools;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    private Rigidbody Playerbody;

    private InputSystem_Actions InputSystemActions;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    [Tooltip("Player speed when running/walking.")]
    public float speed = 0.5f;
    [Tooltip("Player speed when holding sprint button.")]
    public float sprintSpeed;
    [Tooltip("How high the player can jump.")]
    public float jumpHeight = 800;
    private float stamina;
    [Tooltip("Maximum Stamina the player can have.")]
    public float maxStamina;
    [Tooltip("Amount of Stamina to drain per second when sprinting.")]
    public float staminaDrain;
    [Tooltip("Amount of Stamina to regenerate per second when stop sprinting.")]
    public float staminaRegen;
    [Tooltip("Amount of seconds to wait when stamin reaches 0 before being able to sprint again.")]
    public float staminaCooldown;
    public ProgressBar staminaProgressBar;
    private bool hasJumped;
    private bool _isGrounded;
    public bool IsSprinting { get; private set; }
    public bool IsMoving { get; private set; }
    private Animator _animator;
    public AudioClip[] footSteps;
    public AudioSource audioSource;
    private float _footstepTimer;
    public float walkStepInterval;
    public float sprintStepInterval;
    private float staminaTimer;
    private bool isStaminaCooldown;


    void Awake()
    {
        InputSystemActions = new InputSystem_Actions();
        _animator = GetComponentInChildren<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Playerbody = GetComponent<Rigidbody>();
        stamina = maxStamina;
        staminaProgressBar.maxValue = maxStamina;
        staminaTimer = staminaCooldown;
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position + Vector3.down, groundCheckRadius, groundLayer);
        staminaProgressBar.currentPercent = stamina;

        if (InputSystemActions.Player.Sprint.IsPressed() && stamina > 0 && isStaminaCooldown == false)
        {
            IsSprinting = true;
            stamina -= Time.deltaTime * staminaDrain;
        }
        else
        {
            IsSprinting = false;
            stamina += Time.deltaTime * staminaRegen;
        }

        // Stamina cooldown handling
        if (isStaminaCooldown)
        {
            staminaTimer -= Time.deltaTime;
            if (staminaTimer <= 0)
            {
                staminaTimer = staminaCooldown;
                isStaminaCooldown = false;
            }
        }
        if (stamina <= 0 && !isStaminaCooldown)
        {
            Debug.Log("Stamina timer reset!");
            staminaTimer = staminaCooldown;
            isStaminaCooldown = true;
        }

        // Clamp the stamina each frame so it wont exceed min/max
        stamina = Mathf.Clamp(stamina,0,maxStamina);

        if (InputSystemActions.Player.Jump.triggered && _isGrounded) 
        {
            hasJumped = true;
        }
        _animator.SetBool("IsSprinting",IsSprinting);
        _animator.SetBool("IsGrounded",_isGrounded);
    }

    void FixedUpdate()
    {
        Vector2 moveInput = InputSystemActions.Player.Move.ReadValue<Vector2>();
        // Vector2 -> Vector3 conversion
        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
        // Move the player
        if (IsSprinting == true)
        {
            Playerbody.linearVelocity = new Vector3(direction.x*sprintSpeed,Playerbody.linearVelocity.y,direction.z * sprintSpeed);
        }
        else
        {
            
            Playerbody.linearVelocity = new Vector3(direction.x*speed,Playerbody.linearVelocity.y,direction.z*speed);
            _animator.SetBool("IsSprinting",false);
        }

        _animator.SetFloat("VelocityX",moveInput.x,0.1f,Time.fixedDeltaTime);
        _animator.SetFloat("VelocityY",moveInput.y,0.1f,Time.fixedDeltaTime);
        

        if (hasJumped == true)
        {
            Playerbody.AddForce(Vector3.up * jumpHeight * Time.fixedDeltaTime, ForceMode.Impulse);
            hasJumped = false;
        }

        /// AUDIO
        if (_isGrounded && moveInput.magnitude > 0)
        {
            IsMoving = true;
            _footstepTimer -= Time.fixedDeltaTime;
            if (_footstepTimer <= 0)
            {
                int audioIndex = Random.Range(0,footSteps.Length);
                audioSource.PlayOneShot(footSteps[audioIndex]);
                _footstepTimer = IsSprinting ? sprintStepInterval : walkStepInterval;
            }
        }
        else
        {
            IsMoving = false;
            _footstepTimer = 0;
            audioSource.Stop();
        }
        


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down, groundCheckRadius);
    }

    void OnEnable()
    {
        InputSystemActions.Enable();
    }

    void OnDisable()
    {
        InputSystemActions.Disable();
    }
}
