using System;
using System.Collections;
using UnityEngine;

public class FlareItem : MonoBehaviour, IUsable
{
    public Light flareLight;
    public ItemData flareData;
    public float flareActiveTime;
    public GameObject smokeVFX;

    private Animator _animator;

    public InventorySlot CurrentSlot {get; private set;}
    public static Action<bool> OnFlareActive;
    public AudioClip flareSound;

    private AudioSource audioSource;



    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        Debug.Log(_animator);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flareLight.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Use()
    {
        if (GameState.IsInventoryOpen || GameState.IsUIOpen)
        {
            return;
        }
        audioSource.PlayOneShot(flareSound);
        OnFlareActive?.Invoke(true);
        InventoryUI.Instance.ConsumeFromSlot(InventoryUI.Instance.slots[InventoryUI.Instance.currentSlotIndex]);
        StopAllCoroutines();
        StartCoroutine(FlareActivate());
    }

    private IEnumerator FlareActivate()
    {
        yield return new WaitForSeconds(0.3f);
        flareLight.gameObject.SetActive(true);
        smokeVFX.SetActive(true);
        _animator.SetTrigger("OnUse");
        yield return new WaitForSeconds(flareActiveTime);
        OnFlareActive?.Invoke(false);
        Destroy(gameObject);
        
    }

}
