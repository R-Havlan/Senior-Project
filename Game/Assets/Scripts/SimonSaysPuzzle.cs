using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SimonSaysPuzzle : PuzzleController
{
    [Header("Objects")]
    public PuzzleTile[] tiles;
    
    [Header("Settings")]
    public Tile[] yellow_order = new Tile[8];
    public Tile[] blue_order = new Tile[8];
    public Tile[] green_order = new Tile[8];
    public Tile[] red_order = new Tile[8];

    private Tile[][] color_orders = new Tile[8][];
    private Tile[] round_order = new Tile[3];
    private Tile[] display_order = new Tile[3];

    private int[] color_order_indices = new int[4];

    private int check_index;
    private int rounds_won;

    private bool awaiting_reset = true;
    
    /// <summary>
    /// The four tile colors for Simon Says
    /// </summary>
    public enum Tile
    {
        YELLOW,
        BLUE,
        GREEN,
        RED
    }

    void Start()
    {
        color_orders[0] = yellow_order;
        color_orders[1] = blue_order;
        color_orders[2] = green_order;
        color_orders[4] = red_order;
    }

    /// <summary>
    /// Checks if the clicked button is correct. If so, flash its color and check for round completed. If not, run Fail
    /// </summary>
    /// <param name="tile">Tile object that was clicked</param>
    /// <returns></returns>
    public IEnumerator OnTilePressed(PuzzleTile tile)
    {
        if (puzzleSolved || holdInput || awaiting_reset) yield break;
        Debug.Log("clicked tile " + (Tile)tile.tileIndex);
        holdInput = true;

        if (tile.tileIndex == (int)round_order[check_index])
        {
            Debug.Log("Correct input");
            yield return StartCoroutine(Flash((Tile)tile.tileIndex));
            if (check_index == 2)
            {
                rounds_won++;
                if(rounds_won == 3)
                {
                    Win();
                }
                else
                {
                    check_index = 0;
                    yield return new WaitForSeconds(0.3f);
                    GenerateRound();
                    yield return StartCoroutine(PlayRound());
                }
            }
            else
            {
                check_index++;
            }
        }
        else
        {
            Debug.Log("Wrong input");
            yield return StartCoroutine(Fail());
        }

        if (!puzzleSolved) { holdInput = false; }
    }
    
    /// <summary>
    /// Resets the puzzle to the beginning
    /// </summary>
    private void OnResetPressed()
    {
        if(puzzleSolved || holdInput) { return; }
        awaiting_reset = false;
        check_index = 0;
        rounds_won = 0;
        color_order_indices = new int[] {0,0,0,0};
        GenerateRound();
        StartCoroutine(PlayRound());
    }

    /// <summary>
    /// Generates a new round by selecting three random colors and setting the
    /// corresponding round order based on the next correct answer for that color
    /// </summary>
    private void GenerateRound()
    {
        Debug.Log("Generating round");
        int color;
        for (int i = 0; i < 3; i++)
        {
            color = Random.Range(0, 4);
            display_order[i] = (Tile)color;
            round_order[i] = color_orders[color][color_order_indices[color]];
            color_order_indices[color]++;
        }
        Debug.Log("\nNew round : " + string.Join(" ", round_order) + "\nDisplay   : " + string.Join(" ", display_order));
    }

    /// <summary>
    /// Flashes the color tiles based on the generated display order
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayRound()
    {
        Debug.Log("Playing round");
        check_index = 0;
        holdInput = true;
        for(int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(Flash(display_order[i]));
            yield return new WaitForSeconds(0.2f);
        }
        if (focused) { holdInput = false; }
    }

    /// <summary>
    /// Flashes a single tile
    /// </summary>
    /// <param name="tile">The color of the tile to flash</param>
    /// <returns></returns>
    private IEnumerator Flash(Tile tile)
    {
        Debug.Log("Flashing + " + tile);
        tiles[(int)tile].SendMessage("Highlight");
        yield return new WaitForSeconds(0.8f);
        tiles[(int)tile].SendMessage("ResetTile");
    }

    protected override IEnumerator Fail()
    {
        holdInput = true;
        foreach(PuzzleTile tile in tiles) { tile.SendMessage("Error"); }
        yield return new WaitForSeconds(1.0f);
        foreach (PuzzleTile tile in tiles) { tile.SendMessage("ResetTile"); }
        check_index = 0;
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(PlayRound());
    }

    protected override void Win()
    {
        puzzleSolved = true;
        holdInput = true;
        foreach (PuzzleTile tile in tiles) { tile.SendMessage("Clear"); }
        _room.SendMessage("Complete", this.gameObject);
    }
}
