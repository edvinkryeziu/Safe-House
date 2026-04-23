using Michsky.MUIP;
using TMPro;
using UnityEngine;

public class NoteItem : MonoBehaviour, IInteractable, IUsable
{
    public ItemData noteData;
    private bool hasInteracted;
    private bool showPickupButton;
    void Start()
    {
        hasInteracted = false;
    }
    public void Interact()
    {
        showPickupButton = true;
        OpenNote(!hasInteracted,true);
    }

    public void Use()
    {
        showPickupButton = false;
        OpenNote(!hasInteracted,false);
    }

    private void OpenNote(bool open, bool showPickup)
    {
        hasInteracted = open;
        GameState.SetUIOpen(open);
        if (open)
        {
            NoteUI.Instance.pickupButton.gameObject.SetActive(showPickup);
            NoteUI.Instance.content.text = noteData.noteContent;
            NoteUI.Instance.pickupButton.onClick.AddListener(AddNote);
            NoteUI.Instance.closeButton.onClick.AddListener(CloseNote);
        }
        else
        {
            NoteUI.Instance.pickupButton.onClick.RemoveListener(AddNote);
            NoteUI.Instance.closeButton.onClick.RemoveListener(CloseNote);
        }
        NoteUI.Instance.noteUIPanel.gameObject.SetActive(open);
    }

    public void CloseNote()
    {
        OpenNote(false,showPickupButton);
    }

    public void AddNote()
    {
        OpenNote(false,false);
        InventorySystem.Instance.AddItem(noteData, gameObject);
    }
}
