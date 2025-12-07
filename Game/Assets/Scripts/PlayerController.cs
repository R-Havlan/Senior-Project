using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Values")]
    public float Speed = 5.0f;
    public float JumpHeight = 1.5f;
    public float Gravity = -9.81f;

    [Header("Input Actions")]
    public InputActionReference LookAction;
    public InputActionReference MoveAction;
    public InputActionReference JumpAction;
    public InputActionReference InteractAction;

    [Header("Camera Object")]
    public GameObject CameraObject;

    [Header("Misc")]
    public LayerMask hitMask;

    private CharacterController controller;
    private Vector3             velocity;
    private float               moveDir;
    private bool                grounded;
    private bool                active;
    private RaycastHit          hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Move();
            CheckInteraction();
        }
    }

    private void Move()
    {
        // Determine grounded
        grounded = controller.isGrounded;

        // Stop falling at ground
        if (grounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        // Get movement input
        Vector2 input = MoveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move.Normalize();

        // Get jump input
        if (JumpAction.action.triggered && grounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);
        }

        // Apply gravity
        velocity.y += Gravity * Time.deltaTime;

        // Final movement
        moveDir = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + CameraObject.transform.eulerAngles.y;
        Vector3 targetDirection = Quaternion.Euler(0.0f, moveDir, 0.0f) * Vector3.forward;

        if (input != Vector2.zero)
        {
            controller.Move(targetDirection.normalized * (Speed * Time.deltaTime) + new Vector3(0.0f, velocity.y, 0.0f) * Time.deltaTime);
        }
        else
        {
            controller.Move(new Vector3(0.0f, velocity.y, 0.0f) * Time.deltaTime);
        }
    }

    private void CheckInteraction()
    {
        if (InteractAction.action.WasPressedThisFrame())
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit, 100, hitMask))
            {
                hit.collider.gameObject.SendMessage("Focus");
                ToggleActive();
            }
        }
    }

    private void ToggleActive()
    {
        if(active)
        {
            active = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            active = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
