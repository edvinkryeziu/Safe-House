using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
