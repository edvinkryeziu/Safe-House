using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipPanel : MonoBehaviour
{
    public static TooltipPanel Instance {get; private set;}
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public RectTransform canvasRectTransform;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    // Get Mouse position of ToolTip
    void Update()
    {
        if (gameObject.activeSelf)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                Mouse.current.position.ReadValue(),
                null,
                out Vector2 localPoint
            );
        // Follow the mouse with an offset so the mouse isnt at the center of the tooltip
        transform.localPosition = localPoint + new Vector2(0f,50f);
        }

    }

    public void Show(ItemData item)
    {
        if (item != null)
        {
            gameObject.SetActive(true);
            itemName.text = item.itemName;
            itemDescription.text = item.itemDescription;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
