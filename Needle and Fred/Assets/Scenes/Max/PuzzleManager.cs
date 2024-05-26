using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public List<Level> levels;
    public GameObject[] candles;
    public GameObject displaySpawn;
    public GameObject flamePrefab; // Flame particle prefab
    public GameObject smokePrefab; // Smoke particle prefab
    private int currentLevel;
    private int currentStep;
    private int candlesLeft;
    private int numberOfSteps;
    private GameObject displayedStep;
    private bool puzzleEnabled;
    private List<GameObject> candleFlames = new List<GameObject>(); // List to hold flame particle systems
    private List<GameObject> candleSmokes = new List<GameObject>(); // List to hold smoke particle systems
    public GameObject riddleText;
    public GameObject zombieModel;
    public GameObject humanModel;
    public GameOver gameOver;

    void Start()
    {
        // Get current level into manager, set currentStep as 0
        currentLevel = 0;
        currentStep = 0;
        candlesLeft = candles.Length; // Initialize candles left based on array length

        // Initialize candles with particle systems
        for (int i = 0; i < candles.Length; i++)
        {
            // Instantiate flame and smoke particles
            GameObject flame = Instantiate(flamePrefab, candles[i].transform);
            GameObject smoke = Instantiate(smokePrefab, candles[i].transform);

            // Position the particle systems with a slight offset along the Y-axis
            flame.transform.localPosition = new Vector3(0f, 1.5f, 0f); // Adjust the Y value as needed
            smoke.transform.localPosition = new Vector3(0f, 1.5f, 0f); // Adjust the Y value as needed
        
            smoke.SetActive(false); // Disable smoke initially

            candleFlames.Add(flame);
            candleSmokes.Add(smoke);
        }

        numberOfSteps = levels[currentLevel].steps.Count;

        // Enable puzzles at the start of game
        puzzleEnabled = true;
        // Show first step of recipe
        DisplayRiddle();
    }

    // Check the selected ingredient matches the current step in the recipe
    public void CheckStep(string selectedIngredient)
    {
        if (puzzleEnabled)
        {
            Debug.Log("Checking ingredient:" + selectedIngredient + " against recipe step:" + levels[currentLevel].steps[currentStep].ingredient);
            if (selectedIngredient == levels[currentLevel].steps[currentStep].ingredient)
            {
                Debug.Log("CORRECT INGREDIENT");
                currentStep++;
                //TODO : Remove - DestroyImmediate(displayedStep);
                FindObjectOfType<AudioManager>().Play("CandleComplete");
                if (currentStep > numberOfSteps - 1)
                {
                    Debug.Log("RECIPE COMPLETE!");
                    SwapModels();
                    NewPuzzle();
                    FindObjectOfType<AudioManager>().Play("RecipeSuccess");
                }
                else
                {
                    DisplayRiddle();
                }
            }
            else
            {
                Debug.Log("WRONG INGREDIENT");

                // Blow out candle when wrong ingredient selected
                BlowOutCandle(candlesLeft - 1);
                FindObjectOfType<AudioManager>().Play("CandleFail");
                candlesLeft--;
                Debug.Log("One candle has gone out..." + candlesLeft + " candles remaining");

                // If no more candles are lit, trigger game over state
                if (candlesLeft == 0)
                {
                    gameOver.GameIsOver();
                    Debug.Log("No more candles are lit - GAME OVER");
                    puzzleEnabled = false;
                    FindObjectOfType<AudioManager>().Play("GameOver");
                }
            }
        }
    }

    void SwapModels()
    {
        // Instantiate the new model in the same position, rotation, and scale as the current model
        GameObject humanModelInstance = Instantiate(humanModel, zombieModel.transform.position, zombieModel.transform.rotation);
        humanModelInstance.transform.localScale = zombieModel.transform.localScale;

        //Add the new model to the scene
        humanModelInstance.SetActive(true);

        // Destroy the current model after a short delay to ensure a smooth transition
        Destroy(zombieModel, 0.1f);
    }


    void BlowOutCandle(int candleIndex)
    {
        candleFlames[candleIndex].SetActive(false);
        candleSmokes[candleIndex].SetActive(true);
        // Play the audio clip when the candle's flame goes out
        AudioSource audioSource = candles[candleIndex].GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Display current step in recipe by spawning that object
    void DisplayRiddle() {
        Debug.Log("Displaying next riddle: " + levels[currentLevel].steps[currentStep].riddle);
        riddleText.GetComponent<TMP_Text>().SetText(levels[currentLevel].steps[currentStep].riddle);
    }

    // Initiate next recipe if still more levels, otherwise trigger win state
    void NewPuzzle()
    {
        // Move to next level
        currentLevel++;
        // If there are more levels
        if (currentLevel < levels.Count) {
            currentStep = 0;
            DisplayRiddle();
            ResetCandles();
        }
        else {
            Debug.Log("All recipes completed - YOU WIN");
            puzzleEnabled = false;
        }
    }

    void ResetCandles() {
        candlesLeft = candles.Length;
        for (int i = 0; i < candles.Length; i++) {
            candleFlames[i].SetActive(true);
            candleSmokes[i].SetActive(false);
        }
    }
}
