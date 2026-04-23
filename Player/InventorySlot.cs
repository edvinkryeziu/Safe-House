using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float textFadeSpeed;
    public bool isFilled;
    public bool isSelected;
    public Image icon;
    public Image border;
    public TextMeshProUGUI label;
    public ItemData _CurrentItem {get; private set;}
    public static event Action<InventorySlot> OnSlotClicked;
    public static event Action<InventorySlot> OnSlotDrop;
    public void SetSlot(ItemData item)
    {
        isFilled = true;
        icon.gameObject.SetActive(true);
        icon.sprite = item.itemSprite;
        _CurrentItem = item;
    }

    public void ClearSlot()
    {
        isFilled = false;
        icon.gameObject.SetActive(false);
        icon.sprite = null;
        _CurrentItem = null;
    }

    public void Select()
    {
        isSelected = true;
        border.gameObject.SetActive(true);
        if (_CurrentItem != null)
        {
            label.text = _CurrentItem.name;
            label.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeOutText());
        }
    }

    public void DeSelect()
    {
        isSelected = false;
        border.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutText()
    {
        label.color = new Color(
            label.color.r,
            label.color.g,
            label.color.b,
            1
        );
        yield return new WaitForSeconds(1f);
        while (label.color.a > 0)
        {
            label.color = new Color(
                label.color.r,
                label.color.g,
                label.color.b,
                label.color.a - Time.deltaTime * textFadeSpeed
            );
            yield return null;
        }
        label.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // On Left click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"SLOT CLICKED! {this}");
            OnSlotClicked?.Invoke(this);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_CurrentItem != null)
            {
                OnSlotDrop?.Invoke(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipPanel.Instance.Show(_CurrentItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipPanel.Instance.Hide();
    }

}
