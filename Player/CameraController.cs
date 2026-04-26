using UnityEngine;


public class CameraController : MonoBehaviour
{
    public static CameraController Instance {get; private set;}
    private InputSystem_Actions InputSystemActions;
    public float sensitivity;
    public float cameraClampMax;
    public float cameraClampMin;
    // smoothing to get camera back to normal from bobbing
    public float smoothSpeed;
    // How fast the camera bobs
    public float bobFrequency;
    // How high/low it bobs
    public float bobAmplitude;
    public float walkBobFrequency;
    public float walkBobAmplitude;
    // Accumulates time to feed into the sine function
    private float _bobTimer;
    private float _walkBobTimer;
    private PlayerController _playerController;
    public Vector3 StartPosition {get; private set;}
    private bool inventoryOpened;

    float xRotation;
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InputSystemActions = new InputSystem_Actions();
        _playerController = GetComponentInParent<PlayerController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPosition = transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.IsUIOpen || GameState.IsInventoryOpen)
        {
            return;
        }

        Vector2 lookInput = InputSystemActions.Player.Look.ReadValue<Vector2>();
        xRotation -= lookInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation,cameraClampMin,cameraClampMax);
        transform.localRotation = Quaternion.Euler(xRotation,0,0);
        transform.parent.Rotate(Vector3.up*lookInput.x*sensitivity);

        // Activate camera bobbing
        if (_playerController.IsSprinting)
        {
            _bobTimer += Time.deltaTime * bobFrequency;
            transform.localPosition = StartPosition + Vector3.up * Mathf.Sin(_bobTimer) * bobAmplitude;
        }
        else if (_playerController.IsMoving && !_playerController.IsSprinting)
        {
            _walkBobTimer += Time.deltaTime * walkBobFrequency;
            transform.localPosition = StartPosition + Vector3.up * Mathf.Sin(_walkBobTimer) * walkBobAmplitude;
        }
        // Deactivate it and apply smoothing to original position
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,StartPosition, Time.deltaTime * smoothSpeed);
        }
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value/100;
        PlayerPrefs.SetFloat("Sensitivity", value/100);
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
