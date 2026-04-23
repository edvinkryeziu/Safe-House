using System;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable
{
    public ItemData itemData;
    public static event Action<ItemData, GameObject> OnPickedUp;

    public void PickUp()
    {
        OnPickedUp?.Invoke(itemData, gameObject);
    }

}
