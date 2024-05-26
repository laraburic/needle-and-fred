using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private bool puzzleEnabled;
    
    // Level tracking
    public int currentLevel;
    public List<Level> levels;
    
    // Riddle logic
    public GameObject riddleText;
    
    // Candle logic
    public GameObject[] candles;
    public GameObject displaySpawn;
    public GameObject flamePrefab; // Flame particle prefab
    public GameObject smokePrefab; // Smoke particle prefab
    private int currentStep;
    private int candlesLeft;
    private List<GameObject> candleFlames = new List<GameObject>(); // List to hold flame particle systems
    private List<GameObject> candleSmokes = new List<GameObject>(); // List to hold smoke particle systems
    
    // Steps
    private GameObject displayedStep;
    private int numberOfSteps;
    
    private void Start()
    {
        // Puzzle Logic ----
        puzzleEnabled = true; // Enable puzzles at the start of game
        // TODO : Show first step of recipe
        //DisplayIngredient();
        
        // Levels and steps ----
        currentLevel = 0; // Set the current level
        numberOfSteps = levels[currentLevel].steps.Count; // Get how many steps are in current level
        currentStep = 0; // Set currentStep as 0
        
        // Candles ----
        candlesLeft = candles.Length; // Initialize candles left based on array length
        for (int i = 0; i < candles.Length; i++) {
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
        // Riddle ----
        DisplayRiddle();
        //Debug.Log("Current level: " + currentLevel + " current step: " + currentStep);
        //Debug.Log("Current ingredient: " + levels[currentLevel].steps[currentStep].ingredient);
        //Debug.Log("Current riddle: " + levels[currentLevel].steps[currentStep].riddle);
        //Debug.Log("Completed?: " + levels[currentLevel].steps[currentStep].complete);
    }

    void DisplayRiddle() {
        riddleText.GetComponent<TMP_Text>().SetText(levels[currentLevel].steps[currentStep].riddle);
    }
    
    public void CheckStep(IngredientComponent ingredientComponent) {
        if (puzzleEnabled) {
            // If CORRECT ingredient
            if (ingredientComponent.transform.name == levels[currentLevel].steps[currentStep].ingredient) {
                Debug.Log("CORRECT INGREDIENT");
                currentStep++;
                // If LEVEL COMPLETE
                if (currentStep > numberOfSteps - 1) {
                    levels[currentLevel].levelComplete = true;
                    Debug.Log("LEVEL COMPLETE!");
                }
                // IF LEVEL NOT COMPLETE
                else {
                    DisplayRiddle(); // display next riddle
                    Debug.Log("Displaying next step");
                }
            }
            // If WRONG ingredient
            else {
                Debug.Log("WRONG INGREDIENT");
                BlowOutCandles(candlesLeft -1);
                candlesLeft--;
                Debug.Log("One candle has gone out..." + candlesLeft + " candles remaining");
                // Game over if all candles go out
                if (candlesLeft == 0) {
                    Debug.Log("No more candles are lit - GAME OVER");
                    puzzleEnabled = false;
                }

            }
            
        }
        
    }

    // TODO - Add more levels if time
    /*public void NextLevel() {
        // Move to next level
        currentLevel++;
        // Check if there is another level, otherwise game is complete
        if (currentLevel < levels.Count) {
            currentStep = 0;
            //TODO : DisplayIngredient();
            ResetCandles(); // Reset candles for the new level
        }
        else {
            Debug.Log("All recipes completed - YOU WIN");
            puzzleEnabled = false;
        }
    }*/

    void BlowOutCandles(int candleIndex) {
        candleFlames[candleIndex].SetActive(false);
        candleSmokes[candleIndex].SetActive(true);
        // Play the audio clip when the candle's flame goes out
        AudioSource audioSource = candles[candleIndex].GetComponent<AudioSource>();
        if (audioSource != null) {
            audioSource.Play();
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
