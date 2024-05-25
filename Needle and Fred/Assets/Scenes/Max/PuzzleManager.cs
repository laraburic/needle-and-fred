using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public int levelNumber;
    public LevelListSO levels;
    public GameObject[] candles;
    public GameObject displaySpawn;
    private LevelDataSO currentLevel;
    private int currentStep;
    private int candlesLeft;
    private GameObject displayedStep;

    private bool puzzleEnabled;


    void Start()
    {
        // get current level into manager, set currentStep as 0
        levelNumber = 0;
        currentLevel = levels.levels[levelNumber];
        currentStep = 0;
        candlesLeft = 3;
        // make sure all candles are turned on
        // candles[0].GameObject.SetActive(true); etc

        puzzleEnabled = true;

        // show first step of recipe
        DisplayIngredient();
    }

    // attach this to clicking on body
    // change selectedStep to match type on ingredient
    public void CheckStep(IngredientsSO selectedStep)
    {
        if (puzzleEnabled) {

            Debug.Log("Checking ingredient:" + selectedStep.ingredientType + " against recipe step:" + currentLevel.recipe[currentStep].ingredientType);
            // Check if selected ingredient is correct
            if (selectedStep == currentLevel.recipe[currentStep])
            {
                Debug.Log("CORRECT STEP");

                // Move to next step
                currentStep ++;

                // Change displayed ingredient to match current step in recipe
                DestroyImmediate(displayedStep);
                // Initiate next puzzle if current step was the last step of the recipe
                if (currentStep > currentLevel.recipe.Count - 1) {
                    Debug.Log("RECIPE COMPLETE!");
                    NewPuzzle();
                } else {
                    // Display next step
                    DisplayIngredient();
                }

            }
            else
            {
                Debug.Log("WRONG STEP");
                // check if the player can make any more mistakes
                if (candlesLeft == 1)
                {
                    // TRIGGER GAME OVER STATE HERE
                    Debug.Log("No more candles are lit - GAME OVER");
                    puzzleEnabled = false;
                }
                else
                {
                    // turn off candle, player feedback etc.
                    // candles[candlesLeft-1].GameObject.SetActive(false);
                    candlesLeft --;
                    Debug.Log("One candle has gone out...");
                }
            }
        }
    }

    void DisplayIngredient() {
        Debug.Log("Displaying next ingredient: " + currentLevel.recipe[currentStep].ingredientType);
        Instantiate(currentLevel.recipe[currentStep].displayPrefab, displaySpawn.transform);
        displayedStep = displaySpawn.transform.GetChild(0).gameObject;
    }

    void NewPuzzle()
    {
        levelNumber++;
        if (levelNumber < levels.levels.Count) {
            currentLevel = levels.levels[levelNumber];
            currentStep = 0;
            DisplayIngredient();
        } else {
            // TRIGGER WIN STATE HERE
            Debug.Log("All recipes completed - YOU WIN");
            puzzleEnabled = false;
        }
    }
}
