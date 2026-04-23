using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string doorID;
    public float rotationSpeed;
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip doorLocked;
    public AudioSource audioSource;
    private bool isOpened;
    private Quaternion closedRotation;
    private Quaternion openedRotation;

    public bool isBlocked;
    public void Interact()
    {
        if (!isBlocked)
        {
            audioSource.PlayOneShot(isOpened ? openSound : closeSound);
            isOpened = !isOpened;
        }
        else if (isBlocked)
        {
            PlayerHintText.Instance.SetText("I Need something to open this door");
            audioSource.PlayOneShot(doorLocked);
        }
    }
        

    void Start()
    {
        closedRotation = Quaternion.Euler(0, transform.parent.eulerAngles.y, 0);
        openedRotation = Quaternion.Euler(0, transform.parent.eulerAngles.y + 90, 0);
    }

    private void Update()
    {
        Quaternion targetRotation = isOpened ? openedRotation : closedRotation;
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation,targetRotation,Time.deltaTime * rotationSpeed);
    }
}
