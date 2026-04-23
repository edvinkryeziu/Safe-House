using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float maxDistance;
    public LayerMask interactableLayer;
    public TextMeshProUGUI interactPrompt;
    private CameraController playerCamera;
    private RaycastHit rayHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputSystem_Actions InputSystemActions;
    void Awake()
    {
        InputSystemActions = new InputSystem_Actions();
    }
    void Start()
    {
        playerCamera = GetComponentInChildren<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool playerPressInteract = InputSystemActions.Player.Interact.triggered;
        if (Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward,out rayHit, maxDistance, interactableLayer))
        {
            if (rayHit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                // Enable "press E" prompt when hitting interactable
                interactPrompt.gameObject.SetActive(true);
                if (playerPressInteract)
                {
                    interactable.Interact();
                }
            }
            else if (rayHit.collider.TryGetComponent<IPickable>(out IPickable pickable))
            {
                interactPrompt.gameObject.SetActive(true);
                if (playerPressInteract)
                {
                    pickable.PickUp();
                }
            }
        }
        else
        {
            // If it doesnt hit anything disable "press E" prompt
            interactPrompt.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        //Raycast visbility in editor
        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(playerCamera.transform.position,playerCamera.transform.forward*maxDistance);
    }

    void OnEnable()
    {
        InputSystemActions.Enable();
    }

    void OnDisable()
    {
        InputSystemActions.Disable();
    }
}
