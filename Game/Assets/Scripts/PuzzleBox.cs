using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleBox : MonoBehaviour
{
    [Header("Objects")]
    public GameObject CameraObject;
    public GameObject PlayerObject;
    public Canvas UI;
    public PuzzleController _PuzzleController;

    [Header("Controls")]
    public InputActionReference Escape;
    public InputActionReference Interact;
    public InputActionReference Point;

    [Header("Misc")]
    public LayerMask hitMask;

    private CinemachineCamera   _camera;
    private bool                active;
    private Ray                 mouseRay;
    private RaycastHit          mouseHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = CameraObject.GetComponent<CinemachineCamera>();
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Escape.action.WasPressedThisFrame())
            {
                _camera.Priority = 0;
                active = false;
                UI.enabled = true;
                _PuzzleController.SendMessage("OnUnfocus");
                PlayerObject.SendMessage("ToggleActive");
                return;
            }

            if (Interact.action.WasPressedThisFrame() && _PuzzleController.getInputAllowed())
            {
                mouseRay = Camera.main.ScreenPointToRay(Point.action.ReadValue<Vector2>());
                if (Physics.Raycast(mouseRay, out mouseHit, 10, hitMask))
                {
                    mouseHit.collider.gameObject.SendMessage("Interact");
                }
            }
        }
    }

    private void Focus()
    {
        if(!active)
        {
            _camera.Priority = 2;
            active = true;
            UI.enabled = false;
            _PuzzleController.SendMessage("OnFocus");
        }
    }
}
