using UnityEngine;

public static class GameState
{
    public static bool IsUIOpen {get; private set;}
    public static bool IsInventoryOpen {get; private set;}

    public static void SetUIOpen(bool open)
    {
        IsUIOpen = open;
        UpdateState();
        
    }

    public static void SetInventoryOpen(bool open)
    {
        IsInventoryOpen = open;
        UpdateState();
    }
    
    private static void UpdateState()
    {
        bool anyUIOpen = IsUIOpen || IsInventoryOpen;
        Time.timeScale = anyUIOpen ? 0 : 1;
        Cursor.lockState = anyUIOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = anyUIOpen;
    }
}
