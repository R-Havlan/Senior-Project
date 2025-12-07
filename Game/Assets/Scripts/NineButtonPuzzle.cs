using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System.Collections;

public class NineButtonPuzzle : PuzzleController
{
    [Header("Puzzle Setup")]
    private List<int> correctOrder;
    private List<int> currentOrder = new List<int>();


    void Start()
    {
        switch(_room.light_color)
        {
            case 0: correctOrder = new List<int> { 6, 7, 8, 3, 2 }; break; // Red
            case 1: correctOrder = new List<int> { 3, 4, 5, 7, 1 }; break; // Green
            case 2: correctOrder = new List<int> { 0, 5, 3, 8, 4 }; break; // Blue
            default: break;
        }
    }

    /// <summary>
    /// Checks the clicked tile against 
    /// </summary>
    /// <param name="tile"></param>
    public void OnTilePressed(PuzzleTile tile)
    {
        if (puzzleSolved || holdInput) return;
        Debug.Log("clicked tile " + tile.tileIndex);

        tile.SendMessage("Highlight");
        currentOrder.Add(tile.tileIndex);

        // Check if completed
        if (currentOrder.Count == correctOrder.Count)
        {
            holdInput = true;
            CheckOrder();
        }
    }

    void CheckOrder()
    {
        for (int i = 0; i < currentOrder.Count; i++)
        {
            if(currentOrder[i] != correctOrder[i])
            {
                Debug.Log("Incorrect order, resetting");
                StartCoroutine(Fail());
                return;
            }
        }
        Win();
    }

    protected override IEnumerator Fail()
    {
        currentOrder.Clear();
        foreach (PuzzleTile tile in GetComponentsInChildren<PuzzleTile>())
        {
            tile.Error();
        }
        yield return new WaitForSeconds(1);
        foreach (PuzzleTile tile in GetComponentsInChildren<PuzzleTile>())
        {
            tile.ResetTile();
        }
        if (focused) { holdInput = false; }
    }

    protected override void Win()
    {
        // Add your custom logic (e.g., open door, enable next puzzle)
        puzzleSolved = true;
        Debug.Log("The puzzle was completed successfully!");
        foreach (PuzzleTile tile in GetComponentsInChildren<PuzzleTile>())
        {
            tile.Clear();
        }
        _room.SendMessage("Complete", this.gameObject);
    }
}
