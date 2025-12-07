using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleTile : MonoBehaviour
{
    [Header("Objects")]
    public GameObject controller;

    [Header("Controls")]
    public InputActionReference InteractAction;

    [Header("Misc")]
    public int tileIndex; // 0 to 8 for 3x3 grid, 0 to 3 for 2x2
    public Material defaultMaterial;
    public Material highlightMaterial;
    public Material errorMaterial;
    public Material clearMaterial;

    private MeshRenderer rend;
    
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material = defaultMaterial;
    }

    public void Interact()
    {
        if (controller != null)
        {
            controller.SendMessage("OnTilePressed", this);
        }
    }

    public void ResetTile()
    {
        rend.material = defaultMaterial;
    }

    public void Highlight()
    {
        rend.material = highlightMaterial;
    }

    public void Error()
    {
        rend.material = errorMaterial;
    }

    public void Clear()
    {
        rend.material = clearMaterial;
    }

    /*
    void OnMouseEnter()
    {
        rend.material.color = highlightColor;
    }

    void OnMouseExit()
    {
        rend.material.color = defaultColor;
    }
    */
}
