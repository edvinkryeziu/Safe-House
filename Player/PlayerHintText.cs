using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHintText : MonoBehaviour
{
    public static PlayerHintText Instance {get; private set;}
    public float typingSpeed;
    public float textFadeSpeed;
    private TextMeshProUGUI content;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        content = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string targetText)
    {

        content.gameObject.SetActive(true);
        content.text = "";
        StopAllCoroutines();
        StartCoroutine(TypeWriteText(targetText));


        
    }

    private IEnumerator TypeWriteText(string targetText)
    {
        content.color = new Color(
            content.color.r,
            content.color.g,
            content.color.b,
            1
        );
        foreach (char character in targetText)
        {
            content.text += character;
            yield return new WaitForSeconds(typingSpeed);
        }
            content.color = new Color(
            content.color.r,
            content.color.g,
            content.color.b,
            1
        );
        yield return new WaitForSeconds(1f);
        while (content.color.a > 0)
        {
            content.color = new Color(
                content.color.r,
                content.color.g,
                content.color.b,
                content.color.a - Time.deltaTime * textFadeSpeed
            );
            yield return null;
        }
    }
}
