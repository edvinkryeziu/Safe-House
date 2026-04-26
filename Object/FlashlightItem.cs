using UnityEngine;

public class FlashlightItem : MonoBehaviour, IUsable
{
    public Light flashLight;
    private ItemData flashData;
    public void Use()
    {
        if (GameState.IsInventoryOpen || GameState.IsUIOpen) return;
        flashLight.gameObject.SetActive(!flashLight.gameObject.activeSelf);
    }
}
