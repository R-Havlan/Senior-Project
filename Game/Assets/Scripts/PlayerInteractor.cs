using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    [Header("Input")]
    public InputActionAsset inputActions;

    private InputActionMap _map;
    private InputAction _interact;
    private Camera _camera;

    void Awake()
    {
        _map = inputActions.FindActionMap("Player", true);
        _interact = _map.FindAction("Interact", true);
        _camera = Camera.main;
    }

    void OnEnable() => _map.Enable();
    void OnDisable() => _map.Disable();

    void Update()
    {
        if (_interact.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }
}
