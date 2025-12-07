using UnityEngine;

public class RandomLightColor : MonoBehaviour{
    // Reference to the Light component
    private Light puzzleLight;

    // Possible colors for the light
    private Color[] possibleColors = { Color.red, Color.green, Color.blue };

    void Start()
    {
        /*
        puzzleLight = GetComponent<Light>();

        if (puzzleLight != null){
            // Pick a random color from the array
            Color randomColor = possibleColors[Random.Range(0, possibleColors.Length)];
            puzzleLight.color = randomColor;

            // Optional: Log which color was chosen (for debugging)
            Debug.Log("Puzzle Light color: " + randomColor);
        }
        else{
            Debug.LogWarning("No Light component found on this GameObject!");
        }
        */
    }
}
