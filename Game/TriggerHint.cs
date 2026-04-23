using UnityEngine;

public class TriggerHint : MonoBehaviour
{
    public string hintText;

    public void ShowHint()
    {
        TutorialHint.Instance.SetText(hintText);
    }
}
