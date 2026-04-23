using UnityEngine;

public class KeyItem : MonoBehaviour, IUsable
{
    public string matchID;
    private RaycastHit rayHit;
    public LayerMask interactable;
    public AudioClip unlockSound;
    public AudioClip lockedSound;

    private AudioSource audioSource;
    private bool hasUnlocked;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hasUnlocked = false;
    }

    public void Use()
    {
        if (GameState.IsInventoryOpen || GameState.IsUIOpen)
        {
            return;
        }

        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out rayHit, 5, interactable))
        {
            if (rayHit.collider.gameObject.TryGetComponent<Door>(out Door door))
            {
                if (hasUnlocked && !door.isBlocked)
                {
                    PlayerHintText.Instance.SetText("It's already unlocked.");
                    return;
                }

                if (door.doorID == matchID)
                {
                    hasUnlocked = true;
                    door.isBlocked = false;
                    audioSource.PlayOneShot(unlockSound);
                    PlayerHintText.Instance.SetText("Door is unlocked.");
                }
                else
                {
                    PlayerHintText.Instance.SetText("Don't think this is the right key");
                    audioSource.PlayOneShot(lockedSound);
                }
            }
        }
    }
}
