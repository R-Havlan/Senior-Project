using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class WaterCypherPuzzle : PuzzleController
{
    [Header("Objects")]
    [SerializeField] private GameObject   water;
    [SerializeField] private GameObject[] tiles;
    [SerializeField] private GameObject[] faces;

    [Header("Materials")]
    [SerializeField] private Material   clue_material;
    [SerializeField] private Material[] tile_materials;

    [Header("Word Textures")]
    [SerializeField] private Texture2D[] word_textures;


    private int     round_number;
    private int     water_level;
    private int     clue_word_index;
    private int[]   current_buttons = new int[6];
    private int[][] correct_answers = new int[][]
    {
        new int[] { 6, 10, 3 },
        new int[] { 2,  0, 7 },
        new int[] { 11, 9, 4 },
        new int[] { 1,  8, 5 }
    };
    private int current_correct;
    private int correct_tile;
    private List<int> clue_order;
    private List<int> used_words;
    private Animator water_animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        water_animator = water.GetComponent<Animator>();

        
        clue_order = new List<int>() { 0, 1, 2, 3 };
        clue_order.Shuffle();
        

        /* Testing with more than four rounds
        clue_order = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            clue_order.Add((int)Random.Range(0, 4));
        }
        */

        round_number = 0;
        used_words = new List<int>();
        GenerateRound();
        holdInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks if the button is correct
    /// </summary>
    /// <param name="id">Integer ID of the pressed button</param>
    /// <returns></returns>
    private IEnumerator OnButtonPressed(int id)
    {
        Debug.Log("Button pressed: " +  id);
        holdInput = true;
        if (current_buttons[id] == current_correct)
        {
            Debug.Log("Correct");
            if (round_number == clue_order.Count - 1)
            {
                Debug.Log("Win");
                Win();
            }
            else
            {
                yield return StartCoroutine(WinRound());
                GenerateRound();
            }
        }
        else
        {
            Debug.Log("Wrong");
            yield return StartCoroutine(Fail());
        }
        yield break;
    }

    /// <summary>
    /// Generates a new round by setting the water level, the clue word, and the button material textures
    /// </summary>
    private void GenerateRound()
    {
        used_words.Clear();
        water_level = Random.Range(0, 3);
        water_animator.SetInteger("Level", water_level);

        // Get the clue and set the clue tile material
        clue_word_index = (clue_order[round_number] * 3) + 1;
        Texture2D clue_word_image = word_textures[clue_word_index];

        clue_material.SetTexture("_BaseMap", clue_word_image);
        clue_material.SetTexture("_EmissionMap", clue_word_image);

        used_words.Add(clue_word_index);

        // Set a random button to the correct answer
        current_correct = correct_answers[clue_order[round_number]][water_level];
        correct_tile = Random.Range(0, 6);

        tile_materials[correct_tile].SetTexture("_BaseMap", word_textures[current_correct]);
        tile_materials[correct_tile].SetTexture("_EmissionMap", word_textures[current_correct]);
        current_buttons[correct_tile] = current_correct;

        used_words.Add(current_correct);

        // Set the rest of the buttons to random words
        for (int i = 0; i < 6; i++)
        {
            if (i == correct_tile) continue;
            int word_index = current_correct;

            while (used_words.Contains(word_index))
            {
                word_index = Random.Range(0, word_textures.Length);
            }

            tile_materials[i].SetTexture("_BaseMap", word_textures[word_index]);
            tile_materials[i].SetTexture("_EmissionMap", word_textures[word_index]);
            current_buttons[i] = word_index;

            used_words.Add(word_index);
        }
        holdInput = false;
    }

    /// <summary>
    /// Runs a brief win sequence and increments the round
    /// </summary>
    /// <returns></returns>
    private IEnumerator WinRound()
    {
        foreach (GameObject face in faces) { face.SetActive(false); }
        foreach (GameObject tile in tiles) { tile.SendMessage("Win"); }
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject face in faces) { face.SetActive(true); }
        foreach (GameObject tile in tiles) { tile.SendMessage("Default"); }
        round_number++;
    }

    protected override void Win()
    {
        puzzleSolved = true;
        foreach (GameObject face in faces) { face.SetActive(false); }
        foreach (GameObject tile in tiles) { tile.SendMessage("Win"); }
        _room.SendMessage("Complete", this.gameObject);
    }

    protected override IEnumerator Fail()
    {
        foreach (GameObject face in faces) { face.SetActive(false); }
        foreach (GameObject tile in tiles) { tile.SendMessage("Fail"); }
        yield return new WaitForSeconds(1.0f);
        foreach (GameObject face in faces) { face.SetActive(true); }
        foreach (GameObject tile in tiles) { tile.SendMessage("Default"); }
        holdInput = false;
    }
}
