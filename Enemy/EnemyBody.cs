using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBody : MonoBehaviour
{

    public float bodyHeightOffset;
    [Tooltip("How fast body adjusts to new height.")]
    public float bodyAdjustSpeed;
    [Tooltip("Walkable objects")]
    public LayerMask groundLayer;
    [Tooltip("Minimum amount of time before it plays audio randomly")]
    public float minAudioTimer;
    [Tooltip("Maximum amount of time before it plays audio randomly")]
    public AudioClip[] noises;
    public float maxAudioTimer;
    public ProceduralLeg[] legs;
    public Transform bodyRotation;
    public float rotationSpeed;
    [Tooltip("Max degrees body can tilt from world up. Prevents over-tilting from step asynchrony.")]
    [Range(0f, 90f)] public float maxTiltAngle = 25f;

    [Header("Leg Groups (for body orientation)")]
    public ProceduralLeg[] frontLegs;
    public ProceduralLeg[] backLegs;
    public ProceduralLeg[] leftLegs;
    public ProceduralLeg[] rightLegs;

    private float _audioTimer;
    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        TryGetComponent<AudioSource>(out audioSource);
    }

    void Update()
    {
        // --- BODY ROTATION (from foot positions) ---
        Vector3 frontAvg = GetPlantedAverage(frontLegs);
        Vector3 backAvg  = GetPlantedAverage(backLegs);
        Vector3 leftAvg  = GetPlantedAverage(leftLegs);
        Vector3 rightAvg = GetPlantedAverage(rightLegs);

        Vector3 forwardDir = (frontAvg - backAvg).normalized;
        Vector3 rightDir   = (rightAvg - leftAvg).normalized;

        Vector3 bodyUp = Vector3.Cross(forwardDir, rightDir).normalized;
        if (bodyUp.y < 0) bodyUp = -bodyUp;

        // Clamp tilt from world up so step asynchrony can't push body past IK-safe angle
        float tiltAngle = Vector3.Angle(Vector3.up, bodyUp);
        if (tiltAngle > maxTiltAngle)
        {
            Vector3 axis = Vector3.Cross(Vector3.up, bodyUp).normalized;
            bodyUp = Quaternion.AngleAxis(maxTiltAngle, axis) * Vector3.up;
        }

        Quaternion targetRotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(transform.forward, bodyUp), bodyUp);
        bodyRotation.rotation = Quaternion.Lerp(
            bodyRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // --- BODY HEIGHT (via baseOffset, using average of ALL planted feet) ---
        float ySum = 0;
        int plantedCount = 0;
        foreach (ProceduralLeg leg in legs)
        {
            if (!leg._isStepping)
            {
                ySum += leg.target.position.y;
                plantedCount++;
            }
        }

        if (plantedCount > 0)
        {
            float avgY = ySum / plantedCount;

            float navMeshSurfaceY = transform.position.y - navMeshAgent.baseOffset;
            float targetOffset = (avgY - navMeshSurfaceY) + bodyHeightOffset;
            float currentOffset = navMeshAgent.baseOffset;

            // Smooth equally in both directions so slopes don't bias the body low
            navMeshAgent.baseOffset = Mathf.Lerp(currentOffset, targetOffset, bodyAdjustSpeed * Time.deltaTime);
        }

        if (audioSource)
        {
            _audioTimer -= Time.deltaTime;
            if (_audioTimer <= 0)
            {
                int randIndex = Random.Range(0,noises.Length);
                _audioTimer = Random.Range(minAudioTimer,maxAudioTimer);

                audioSource.PlayOneShot(noises[randIndex]);

            }
        }
    }

    private Vector3 GetPlantedAverage(ProceduralLeg[] legGroup)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (ProceduralLeg leg in legGroup)
        {
            if (!leg._isStepping)
            {
                sum += leg.target.position;
                count++;
            }
        }
        // Fallback: if all legs in group are stepping, use their positions anyway
        if (count == 0)
        {
            foreach (ProceduralLeg leg in legGroup)
            {
                sum += leg.target.position;
                count++;
            }
        }
        return sum / count;
    }
}
