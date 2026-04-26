using System.Collections;
using TMPro;
using UnityEngine;

public class ItemPickupUI : MonoBehaviour
{
    private TextMeshProUGUI itemPickupText;
    public float textFadeDelay;
    public float textFadeSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemPickupText = GetComponent<TextMeshProUGUI>();
        itemPickupText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnItemAddedUpdate(ItemData item)
    {
        itemPickupText.text = $"[+] {item.name}";
        StopAllCoroutines();
        StartCoroutine(FadeOutText());
    }

    private void OnItemDroppedUpdate(ItemData item)
    {
        itemPickupText.text = $"[-] {item.name}";
        StopAllCoroutines();
        StartCoroutine(FadeOutText());
    }

    void OnEnable()
    {
        InventorySystem.OnItemAdded += OnItemAddedUpdate;
        InventorySystem.OnItemDropped += OnItemDroppedUpdate;
    }

    void OnDisable()
    {
        InventorySystem.OnItemAdded -= OnItemAddedUpdate;
        InventorySystem.OnItemDropped -= OnItemDroppedUpdate;
    }

    private IEnumerator FadeOutText()
    {
        itemPickupText.color = new Color(
            itemPickupText.color.r,
            itemPickupText.color.g,
            itemPickupText.color.b,
            1
        );
        yield return new WaitForSeconds(textFadeDelay);
        while (itemPickupText.color.a > 0)
        {
            itemPickupText.color = new Color(
                itemPickupText.color.r,
                itemPickupText.color.g,
                itemPickupText.color.b,
                itemPickupText.color.a - Time.deltaTime * textFadeSpeed
            );
            yield return null;
        }
    }
}
