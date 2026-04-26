using UnityEngine.InputSystem;
using UnityEngine;
using System;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance {get; private set;}
    public InventorySlot[] slots;
    private InputSystem_Actions InputSystemActions;
    public int currentSlotIndex {get; private set;}
    public static event Action<ItemData> OnSelectChange;
    private bool inventoryOpened;
    private ItemData _heldItem;
    public Image floatingSprite;
    public RectTransform canvasRectTransform;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InputSystemActions = new InputSystem_Actions();
        inventoryOpened = false;
    }

    

    private void Update()
    {
        if (GameState.IsUIOpen && !GameState.IsInventoryOpen) return;

        float mouseScroll = InputSystemActions.Player.Scroll.ReadValue<Vector2>().y;
        // Key number 1->6 check if pressed
        for (int i = 0; i < slots.Length; i++)
        {
            Key targetKey = Key.Digit1 + i;
            if (Keyboard.current[targetKey].wasPressedThisFrame)
            {
                SelectSlot(i);
            }
        }
        // Scroll wheel check
        if (mouseScroll > 0)
        {
            currentSlotIndex = (currentSlotIndex + 1) % slots.Length;
            SelectSlot(currentSlotIndex);
        }
        else if (mouseScroll < 0)
        {
            currentSlotIndex = (currentSlotIndex - 1 + slots.Length) % slots.Length;
            SelectSlot(currentSlotIndex);
        }
        // Dropping Item
        if (InputSystemActions.Player.Drop.triggered)
        {
            if (slots[currentSlotIndex].isFilled)
            {
                InventorySystem.Instance.DropItem(slots[currentSlotIndex]._CurrentItem);
                slots[currentSlotIndex].ClearSlot();
                OnSelectChange?.Invoke(null);
            }
            else
            {
                Debug.Log("Nothing to Drop!");
            }
        }


        // Move the sprite with the mouse
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Mouse.current.position.ReadValue(),
            null,
            out Vector2 localPoint
        );
        floatingSprite.rectTransform.localPosition = localPoint;

        
        // Opening Inventory
        
        if (InputSystemActions.Player.Inventory.triggered && inventoryOpened == true)
        {
            if (_heldItem != null) return;
            GameState.SetInventoryOpen(false);
            inventoryOpened = false;
            return;
        }

        if (InputSystemActions.Player.Inventory.triggered && inventoryOpened == false)
        {
            GameState.SetInventoryOpen(true);
            inventoryOpened = true;
        }
    }

    // Event subscription to OnItemAdded from InventorySystem
    private void AddToSlot(ItemData item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (! slot.isFilled)
            {
                slot.SetSlot(item);
                break;
            }
        }
    }

    private void SelectSlot(int index)
    {
        foreach (InventorySlot slot in slots)
        {
            slot.DeSelect();
        }
        slots[index].Select();
        // Emit signal that selection has changed
        OnSelectChange?.Invoke(slots[index]._CurrentItem);
        currentSlotIndex = index;
        
    }

    // Subscribed to OnSlotClicked
    private void ItemPickUp(InventorySlot slotClicked)
    {
        if (GameState.IsUIOpen && !GameState.IsInventoryOpen) return;

        // If nothing was in that slot
        if (slotClicked._CurrentItem == null && _heldItem == null)
        {
            return;
        }
        // If no item is being held
        if (_heldItem == null)
        {
            _heldItem = slotClicked._CurrentItem;
            floatingSprite.sprite = _heldItem.itemSprite;
            floatingSprite.gameObject.SetActive(true);
            slotClicked.ClearSlot();
            // Update so item gets UnEquipped
            if (slotClicked == slots[currentSlotIndex])
            {
                OnSelectChange?.Invoke(null);
            }
        // If item is being held and the slot is empty
        }
        else if (_heldItem != null && slotClicked._CurrentItem == null)
        {
            slotClicked.SetSlot(_heldItem);
            floatingSprite.gameObject.SetActive(false);
            _heldItem = null;
            if (slotClicked == slots[currentSlotIndex])
            {
                OnSelectChange?.Invoke(slotClicked._CurrentItem);
            }
        }
        // If item is being held but slot is not empty swap them
        else if (_heldItem != null && slotClicked._CurrentItem != null)
        {
            ItemData tempItem = slotClicked._CurrentItem;
            slotClicked.SetSlot(_heldItem);
            _heldItem = tempItem;
            floatingSprite.sprite = _heldItem.itemSprite;
            floatingSprite.gameObject.SetActive(true);
            if (slotClicked == slots[currentSlotIndex])
            {
                OnSelectChange?.Invoke(slotClicked._CurrentItem);
            }
        }
        
    }
    // Subscribed to OnSlotDrop (when right clicking on item in inventory)
    public void DropFromSlot(InventorySlot slot)
    {
        InventorySystem.Instance.DropItem(slot._CurrentItem);
        slot.ClearSlot();
        // If the its the same slot the player currently has selected
        if (slots[currentSlotIndex] == slot)
        {
            OnSelectChange?.Invoke(null);
        }
    }

    public void ConsumeFromSlot(InventorySlot slot)
    {
        InventorySystem.Instance.RemoveItem(slot._CurrentItem);
        slot.ClearSlot();
    }

    void OnEnable()
    {

        InputSystemActions.Enable();
        InventorySystem.OnItemAdded += AddToSlot;
        InventorySlot.OnSlotClicked += ItemPickUp;
        InventorySlot.OnSlotDrop += DropFromSlot;
    }

    void OnDisable()
    {
        InputSystemActions.Disable();
        InventorySystem.OnItemAdded -= AddToSlot;
        InventorySlot.OnSlotClicked -= ItemPickUp;
        InventorySlot.OnSlotDrop -= DropFromSlot;
    }
}
