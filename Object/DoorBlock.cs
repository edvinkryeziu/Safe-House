using UnityEngine;

public class DoorBlock : MonoBehaviour
{
    public Door blockDoor;

    public bool isBlocking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blockDoor.isBlocked = true;
        isBlocking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBlocking)
        {
            blockDoor.isBlocked = isBlocking;
            Destroy(this);
        }
        else
        {
            isBlocking = true;
            blockDoor.isBlocked = isBlocking;
        }
    }
}
