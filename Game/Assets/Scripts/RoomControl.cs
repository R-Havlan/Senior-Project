using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    [Header("Objects")]
    public GameObject _door;
    public GameObject _light;
    public Material[] _lightMaterials;

    [Header("Settings")]
    public int numPuzzles;
    public int difficulty; // Currently unused
    public int light_color;

    private List<GameObject> completed;
    private MeshRenderer lightRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        completed = new List<GameObject>(numPuzzles);

        lightRenderer = _light.GetComponent<MeshRenderer>();
        light_color = Random.Range(0, _lightMaterials.Length);
        lightRenderer.material = _lightMaterials[light_color];
    }

    /// <summary>
    /// Adds a puzzle to the list of completed puzzles and checks completed count.
    /// Called by a PuzzleController when it is successfully completed
    /// </summary>
    /// <param name="puzzle">Puzzle object that is being completed</param>
    public void Complete(GameObject puzzle)
    {
        if (!completed.Contains(puzzle))
        {
            completed.Add(puzzle);
            if(completed.Count == numPuzzles)
            {
                _door.SendMessage("Open");
            }
        }
    }
}
