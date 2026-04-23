using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteUI : MonoBehaviour
{
    public static NoteUI Instance {get; private set;}

    public ButtonManager closeButton;
    public ButtonManager pickupButton;
    public TextMeshProUGUI content;
    public GameObject noteUIPanel;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }
}
