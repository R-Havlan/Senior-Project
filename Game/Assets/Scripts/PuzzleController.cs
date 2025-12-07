using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleController : MonoBehaviour
{

    [Header("Controller Settings")]
    public bool puzzleSolved = false;
    public RoomControl _room;

    protected bool holdInput = true;
    protected bool focused = false;

    /// <summary>
    /// Executes the reset sequence
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator ResetPuzzle() { yield break; }

    public bool getInputAllowed() { return !holdInput; }

    /// <summary>
    /// Waits for a moment to avoid errant mouse input and then enables input unless the puzzle is already completed
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator OnFocus()
    { 
        yield return new WaitForSeconds(0.1f);
        if(!puzzleSolved) holdInput = false;
        focused = true;
    }

    /// <summary>
    /// Holds input. Called when the player exits this puzzle box
    /// </summary>
    public virtual void OnUnfocus() {
        holdInput = true;
        focused = false;
    }

    /// <summary>
    /// Executes the failure sequence
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Fail() { yield break; }

    /// <summary>
    /// Executes the win sequence and notifies the room that this puzzle is complete
    /// </summary>
    protected virtual void Win() { }
}
