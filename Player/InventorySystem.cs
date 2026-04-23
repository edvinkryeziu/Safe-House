using System;
using System.Collections.Generic;
using UnityEngine;


public class InventorySystem : MonoBehaviour
{
    // Publicly accessible to all Scripts
    public static InventorySystem Instance {get; private set; }
    private List<ItemData> _items = new List<ItemData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static event Action<ItemData> OnItemAdded;
    public static event Action<ItemData> OnItemDropped;
    public float dropForce;
    public float dropUpwardForce;
    public int maxSlots;
    private Camera playerCamera;
    private AudioSource playerAudio;

    void Awake()
    {
        // Make this class a singleton
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Try get the AudioSource for item drop and pickup sounds
        TryGetComponent<AudioSource>(out AudioSource audioSource);
        if (audioSource)
        {
            playerAudio = audioSource;
            playerAudio.ignoreListenerPause = true;
        }
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Fires when picked up
    public void AddItem(ItemData item, GameObject itemObject)
    {
        if (_items.Count < maxSlots)
        {
            _items.Add(item);
            Debug.Log($"Picked up: {item.itemName}");
            // Fire signal that an item was added to the inventory
            OnItemAdded?.Invoke(item);
            // Play Pickup sound if item has it
            if (item.pickupSound)
            {
                playerAudio.PlayOneShot(item.pickupSound);
            }
            // Remove the item from the world since its in inventory
            Destroy(itemObject);
        }
        else
        {
            Debug.Log("Not Enough Space");
        }
        
    }

    public void DropItem(ItemData item)
    {
        _items.Remove(item);
        GameObject droppedItem = Instantiate(item.itemPrefab,transform.position,Quaternion.identity);
        droppedItem.TryGetComponent<Rigidbody>(out Rigidbody rb);
        if (rb)
        {
            rb.AddForce(playerCamera.transform.forward * dropForce + Vector3.up * dropUpwardForce,ForceMode.Impulse);
        }
        // Play Drop Sound if item has it
        if (item.dropSound)
        {
            playerAudio.PlayOneShot(item.dropSound);
        }
        OnItemDropped?.Invoke(item);
    }

    void OnEnable()
    {
        PickableItem.OnPickedUp += AddItem;
    }

    void OnDisable()
    {
        PickableItem.OnPickedUp -= AddItem;
    }
}
