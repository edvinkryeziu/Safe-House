using UnityEngine;
using UnityEngine.InputSystem;


public class EquipmentSystem : MonoBehaviour
{
    public Transform handPosition;
    private GameObject equippedItem;
    private InputSystem_Actions InputSystemActions;
    public float swayAmount;
    public float swaySpeed;
    public float maxSwayAmount;

    private Vector3 startHandPosition;

    void Awake()
    {
        InputSystemActions = new InputSystem_Actions();
    }

    void Start()
    {
        startHandPosition = handPosition.localPosition;
    }

    void Update()
    {
        // If player carries an usuable item, make sure it can be used
        if (InputSystemActions.Player.Use.triggered && !GameState.IsUIOpen)
        {
            if (equippedItem == null)
            {
                return;
            }
            if (equippedItem.TryGetComponent<IUsable>(out IUsable usable))
            {
                usable.Use();
            }
        }

        // Item sway
        if (equippedItem)
        {
            float mousePositionX = Mouse.current.delta.ReadValue().x;
            float mousePositionY = Mouse.current.delta.ReadValue().y;
            
            Vector3 handOffset = startHandPosition + new Vector3(-mousePositionX * swayAmount, -mousePositionY * swayAmount, 0);

            handOffset.x = Mathf.Clamp(handOffset.x,startHandPosition.x - maxSwayAmount,startHandPosition.x + maxSwayAmount);
            handOffset.y = Mathf.Clamp(handOffset.y,startHandPosition.y - maxSwayAmount,startHandPosition.y + maxSwayAmount);

            handPosition.localPosition = Vector3.Lerp(handPosition.localPosition,handOffset, Time.deltaTime * swaySpeed);

            Debug.Log($"Offset: {handOffset}, Hand: {handPosition.localPosition}");
        }
        else
        {
            handPosition.localPosition = Vector3.Lerp(handPosition.localPosition, startHandPosition, Time.deltaTime * swaySpeed);
        }
        
    }

    // When selecting inventory slot changes
    private void Equip(ItemData item)
    {
        // Wan't to UnEquip previously equipped item
        // we store previously/current in equippedItem
        UnEquip();
        if (GameState.IsInventoryOpen) return;
        // Check if that slot isFilled/Has someting
        if (item != null)
        {
            // Spawn that item in players hand
            GameObject itemEquipped = Instantiate(item.itemPrefab,handPosition.position,Quaternion.identity,handPosition);
            itemEquipped.transform.localRotation = item.itemPrefab.transform.localRotation;

            if (itemEquipped.TryGetComponent<Animator>(out Animator animator))
            {
                animator.enabled = true;
            }

            if (itemEquipped.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
            if (itemEquipped.TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = false;
            }
            // Update equippedItem
            equippedItem = itemEquipped;
            
        }
    }
    private void UnEquip()
    {
        if (equippedItem != null)
        {
            if (equippedItem.TryGetComponent<NoteItem>(out NoteItem note))
            {
                NoteUI.Instance.noteUIPanel.SetActive(false);
                GameState.SetUIOpen(false);
            }
        }
        Destroy(equippedItem);
    }

    void OnEnable()
    {
        InputSystemActions.Enable();
        InventoryUI.OnSelectChange += Equip;

    }

    void OnDisable()
    {
        InputSystemActions.Disable();
        InventoryUI.OnSelectChange -= Equip;
    }
}
