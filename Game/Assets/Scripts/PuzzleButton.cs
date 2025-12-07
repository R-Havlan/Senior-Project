using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject controller;

    [Header("Info")]
    [SerializeField] private int buttonID;

    [Header("Materials")]
    [SerializeField] private Material win_material;
    [SerializeField] private Material fail_material;
    [SerializeField] private Material default_material;

    private Renderer _renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public int GetID() {  return buttonID; }

    /// <summary>
    /// Sends the OnButtonPressed signal to the attached PuzzleController and gives it the button ID
    /// </summary>
    private void Interact()
    {
        if (controller != null)
        {
            controller.SendMessage("OnButtonPressed", this.buttonID);
        }
    }

    private void Win() { _renderer.material = win_material; }
    private void Fail() { _renderer.material = fail_material; }
    private void Default() { _renderer.material = default_material; }
}
