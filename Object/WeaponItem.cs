using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class WeaponItem : MonoBehaviour, IUsable
{
    public float weaponCooldown;
    private Animator animator;
    private float cooldownTimer;
    public AudioClip swingSound;
    public LayerMask woodPlanks;
    private AudioSource audioSource;
    private RaycastHit rayHit;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryGetComponent<Animator>(out Animator animatorComponent);

        if (animatorComponent) animator = animatorComponent;

        TryGetComponent<AudioSource>(out AudioSource audioSourceComponent);

        if (audioSourceComponent) audioSource = audioSourceComponent;

        cooldownTimer = 0;

    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public void Use()
    {
        if (GameState.IsInventoryOpen || GameState.IsUIOpen)
        {
            return;
        }
        if (cooldownTimer <= 0)
        {
            animator.SetTrigger("Attack");
            cooldownTimer = weaponCooldown;
            if (audioSource != null)
            {
                audioSource.PlayOneShot(swingSound);
            }
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out rayHit, 5, woodPlanks))
            {
                rayHit.collider.gameObject.layer = LayerMask.NameToLayer("Removed");
                rayHit.rigidbody.isKinematic = false;
                if (rayHit.collider.TryGetComponent<DoorBlock>(out DoorBlock doorBlock))
                {
                    doorBlock.isBlocking = false;
                }
            }
        }
        else
        {
            PlayerHintText.Instance.SetText("Need some time!");
        }
        
    }
}
