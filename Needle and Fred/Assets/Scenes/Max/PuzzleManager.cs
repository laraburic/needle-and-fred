using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public int levelNumber;
    public LevelListSO levels;
    public GameObject[] candles;
    public GameObject displaySpawn;
    public GameObject flamePrefab; // Flame particle prefab
    public GameObject smokePrefab; // Smoke particle prefab
    private LevelDataSO currentLevel;
    private int currentStep;
    private int candlesLeft;
    private GameObject displayedStep;
    private bool puzzleEnabled;

    private List<GameObject> candleFlames = new List<GameObject>(); // List to hold flame particle systems
    private List<GameObject> candleSmokes = new List<GameObject>(); // List to hold smoke particle systems

    void Start()
    {
        // Get current level into manager, set currentStep as 0
        levelNumber = 0;
        currentLevel = levels.recipes[levelNumber];
        currentStep = 0;
        candlesLeft = candles.Length; // Initialize candles left based on array length

        // Initialize candles with particle systems
        for (int i = 0; i < candles.Length; i++)
        {
            // Instantiate flame and smoke particles
            GameObject flame = Instantiate(flamePrefab, candles[i].transform);
            GameObject smoke = Instantiate(smokePrefab, candles[i].transform);

            // Position the particle systems (optional, if they need specific offsets)
        flame.transform.localPosition = Vector3.zero; // Adjust based on the prefab setup
        smoke.transform.localPosition = Vector3.zero; // Adjust based on the prefab setup
        
            smoke.SetActive(false); // Disable smoke initially

            candleFlames.Add(flame);
            candleSmokes.Add(smoke);
        }

        // Enable puzzles at the start of game
        puzzleEnabled = true;
        // Show first step of recipe
        DisplayIngredient();
    }

    // Check the selected ingredient matches the current step in the recipe
    public void CheckStep(IngredientsSO selectedIngredient)
    {
        if (puzzleEnabled)
        {
            Debug.Log("Checking ingredient:" + selectedIngredient.ingredientType + " against recipe step:" + currentLevel.recipe[currentStep].ingredientType);
            if (selectedIngredient == currentLevel.recipe[currentStep])
            {
                Debug.Log("CORRECT INGREDIENT");
                currentStep++;
                DestroyImmediate(displayedStep);
                if (currentStep > currentLevel.recipe.Count - 1)
                {
                    Debug.Log("RECIPE COMPLETE!");
                    NewPuzzle();
                }
                else
                {
                    DisplayIngredient();
                }
            }
            else
            {
                Debug.Log("WRONG INGREDIENT");

                // Blow out candle when wrong ingredient selected
                BlowOutCandle(candlesLeft - 1);
                candlesLeft--;
                Debug.Log("One candle has gone out..." + candlesLeft + " candles remaining");

                // If no more candles are lit, trigger game over state
                if (candlesLeft == 0)
                {
                    Debug.Log("No more candles are lit - GAME OVER");
                    puzzleEnabled = false;
                }
            }
        }
    }

    void BlowOutCandle(int candleIndex)
{
    //if (candleIndex >= 0)
    //{
        candleFlames[candleIndex].SetActive(false);
        candleSmokes[candleIndex].SetActive(true);
        
        // Play the audio clip when the candle's flame goes out
        AudioSource audioSource = candles[candleIndex].GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    //}
}

    // Display current step in recipe by spawning that object
    void DisplayIngredient()
    {
        Debug.Log("Displaying next ingredient: " + currentLevel.recipe[currentStep].ingredientType);
        displayedStep = Instantiate(currentLevel.recipe[currentStep].displayPrefab, displaySpawn.transform);
    }

    // Initiate next recipe if still more levels, otherwise trigger win state
    void NewPuzzle()
    {
        // Move to next level
        levelNumber++;
        // Check if there is another level, otherwise game is complete
        if (levelNumber < levels.recipes.Count)
        {
            currentLevel = levels.recipes[levelNumber];
            currentStep = 0;
            DisplayIngredient();
            // Reset candles for the new level
            ResetCandles();
        }
        else
        {
            Debug.Log("All recipes completed - YOU WIN");
            puzzleEnabled = false;
        }
    }

    void ResetCandles()
    {
        candlesLeft = candles.Length;
        for (int i = 0; i < candles.Length; i++)
        {
            candleFlames[i].SetActive(true);
            candleSmokes[i].SetActive(false);
        }
    }
}
