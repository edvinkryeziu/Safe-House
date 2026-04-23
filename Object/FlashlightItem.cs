using UnityEngine;

public class FlashlightItem : MonoBehaviour, IUsable
{
    public Light flashLight;
    private ItemData flashData;
    public void Use()
    {
        flashLight.gameObject.SetActive(!flashLight.gameObject.activeSelf);
    }
}
