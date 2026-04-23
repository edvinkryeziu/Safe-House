using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public GameObject itemPrefab;
    public AudioClip pickupSound;
    public AudioClip dropSound;
    public string noteContent;
}
