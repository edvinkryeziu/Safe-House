using UnityEngine;

public class ProceduralLeg : MonoBehaviour
{
    [Tooltip("Transform of where the target point will be for the IK.")]
    public Transform footTip;
    [Tooltip("Target point of leg.")]
    public Transform target;
    [Tooltip("Point where to raycast from.")]
    public Transform stepOrigin;
    [Tooltip("Distance the raycast can travel from Step Origin.")]
    public float raycastDistance;
    [Tooltip("Layer where the ground is walkable.")]
    public LayerMask groundLayer;
    [Tooltip("How far body moves before stepping.")]
    public float stepDistance;
    [Tooltip("How fast foot moves to new position.")]
    public float stepSpeed;
    public float stepHeight;
    [Tooltip("Stop moving if opposite leg is moving")]
    public ProceduralLeg oppositeLeg;
    public Vector3 SurfaceNormal { get; private set; }
    [Tooltip("Cooldown for it to be able to step to avoid it jittering, recommended value: 0.10-0.30")]
    public float stepCooldownDuration;
    [Tooltip("Step audio sound")]
    public AudioClip[] footsteps;
    public AudioSource audioSource;
    private Vector3 _stepTargetPosition;
    private Vector3 _currentFootPosition;
    private Vector3 _targetFootPosition;
    private RaycastHit rayHit;
    public bool _isStepping {get; private set;}
    private float _stepProgress;

    private float _stepCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentFootPosition = footTip.position;
        target.position = footTip.position;
        _isStepping = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast from well above the step origin so we don't start below the slope surface
        Vector3 rayOrigin = stepOrigin.position + Vector3.up * raycastDistance;
        Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance * 2f, Color.red);

        if (Physics.Raycast(rayOrigin, Vector3.down, out rayHit, raycastDistance * 2f, groundLayer))
        {
            _targetFootPosition = rayHit.point;
            SurfaceNormal = rayHit.normal;
        }

        _stepCooldown -= Time.deltaTime;
        // Compare both ground-level points (current foot vs where raycast says it should be)
        // so standing still is stable and slopes still trigger steps via Y displacement
        if (Vector3.Distance(_currentFootPosition, _targetFootPosition) > stepDistance && !_isStepping && !oppositeLeg._isStepping && _stepCooldown <= 0)
        {
            _isStepping = true;
            _stepTargetPosition = _targetFootPosition;
            _stepProgress = 0;
        }

        if (_isStepping)
        {
            //Debug.Log("Moving!");
            // Add height to the step so the amount of height for each step
            float height = Mathf.Sin(_stepProgress * Mathf.PI) * stepHeight;
            // Smoothly move the leg between current position and target position, _stepProgress is controlled by the speed
            target.position = Vector3.Lerp(_currentFootPosition, _stepTargetPosition, _stepProgress) + Vector3.up * height;
            // Increment the progress each frame
            _stepProgress += Time.deltaTime * stepSpeed;
        }

        // Always have the value between 0, 1
        _stepProgress = Mathf.Clamp(_stepProgress,0,1);

        // If its moving and the progress has been completed
        if (_isStepping && _stepProgress >= 1)
        {
            //  Snap the leg to the target pos since its complete
            target.position = _stepTargetPosition;
            // current foot pos becomes the same as target
            _currentFootPosition = _stepTargetPosition;
            // moving is done
            _isStepping = false;
            // reset progress
            _stepProgress = 0;
            // reset step cooldown
            _stepCooldown = stepCooldownDuration;

            if (audioSource)
            {
                int randIndex = Random.Range(0,footsteps.Length);
                audioSource.PlayOneShot(footsteps[randIndex]);
            }
        }
    }
}
