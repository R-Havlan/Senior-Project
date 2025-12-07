using System.Collections;
using UnityEngine;

public class ColumnsPuzzle : PuzzleController
{
    [Header("Objects")]
    [SerializeField] private GameObject[] lines;

    [Header("Materials")]
    [SerializeField] private Material[] tile_materials;

    [Header("Row Icons")]
    [SerializeField] private Texture2D[] top_order = new Texture2D[5];
    [SerializeField] private Texture2D[] mid_order = new Texture2D[5];
    [SerializeField] private Texture2D[] bot_order = new Texture2D[5];

    private Texture2D[][] orders;
    private int[] row_indices;
    private int[] correct_indices;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orders = new Texture2D[][] { top_order, mid_order, bot_order };
        row_indices = new int[3] { 0, 0, 0 };
        correct_indices = new int[3] { 3, 1, 4 };
        UpdateRows(0);
        UpdateRows(1);
        UpdateRows(2);
    }

    /// <summary>
    /// Performs various button actions depending on ID
    /// </summary>
    /// <param name="buttonID">ID of the clicked button</param>
    private void OnButtonPressed(int buttonID)
    {
        Debug.Log("Button pressed: " + buttonID);
        switch(buttonID)
        {
            case 1:
                row_indices[0]--;
                if(row_indices[0] < 0) { row_indices[0] = 4; }
                UpdateRows(0);
                break;
            case 0:
                row_indices[0]++;
                UpdateRows(0);
                break;
            case 3:
                row_indices[1]--;
                if (row_indices[1] < 0) { row_indices[1] = 4; }
                UpdateRows(1);
                break;
            case 2:
                row_indices[1]++;
                UpdateRows(1);
                break;
            case 5:
                row_indices[2]--;
                if (row_indices[2] < 0) { row_indices[2] = 4; }
                UpdateRows(2);
                break;
            case 4:
                row_indices[2]++;
                UpdateRows(2);
                break;
            case 6:
                StartCoroutine(OnSubmitPressed());
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }
    }

    /// <summary>
    /// Changes the texture on the materials for a given row of icons
    /// </summary>
    /// <param name="row">Which row to update</param>
    private void UpdateRows(int row)
    {
        for(int i = 0; i < 3; i++)
        {
            tile_materials[i + 3 * row].SetTexture("_BaseMap", orders[row][(row_indices[row] + i) % 5]);
            tile_materials[i + 3 * row].SetTexture("_EmissionMap", orders[row][(row_indices[row] + i) % 5]);
        }
    }

    /// <summary>
    /// Checks the puzzle for completeness
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnSubmitPressed()
    {
        holdInput = true;
        for (int i = 0; i < 3; i++)
        {
            if (row_indices[i] % 5 != correct_indices[i])
            {
                yield return StartCoroutine(Fail());
                holdInput = false;
                yield break;
            }
        }
        Win();
    }

    protected override IEnumerator Fail()
    {
        foreach (GameObject line in lines) { line.SendMessage("Fail"); }
        yield return new WaitForSeconds(1f);
        foreach (GameObject line in lines) { line.SendMessage("Default"); }
    }

    protected override void Win()
    {
        puzzleSolved = true;
        foreach (GameObject line in lines) { line.SendMessage("Win"); }
        _room.SendMessage("Complete", this.gameObject);
    }
}
