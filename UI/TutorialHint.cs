using TMPro;
using UnityEngine;

public class TutorialHint : MonoBehaviour
{
    public static TutorialHint Instance {get; private set;}
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public TextMeshProUGUI hintText;

    public void SetText(string text)
    {
        gameObject.SetActive(true);
        hintText.text = text;
    }

    public void DisableHint()
    {
        gameObject.SetActive(false);
    }
}
