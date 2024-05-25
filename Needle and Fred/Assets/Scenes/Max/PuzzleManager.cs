using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public int levelNumber;
    public LevelListSO levels;
    public IngredientsSO selectedStep;
    public GameObject[] candles;
    public GameObject displaySpawn;
    private LevelDataSO currentLevel;
    private int currentStep;
    private int candlesLeft;
    private GameObject displayedStep;
    void Start()
    {
        // get current level into manager, set currentStep as 0
        levelNumber = 0;
        currentLevel = levels.levels[levelNumber];
        currentStep = 0;
        candlesLeft = 3;
        // make sure all candles are turned on
        // candles[0].GameObject.SetActive(true); etc
        // show first step of recipe
        Instantiate(currentLevel.recipe[0].displayPrefab, displaySpawn.transform);
        // reference spawned object to destroy later
        displayedStep = displaySpawn.transform.GetChild(0).gameObject;
    }

    // attach this to clicking on body
    // change selectedStep to match type on ingredient
    public void CheckStep()
    {
        // check if selected ingredient is correct
        if (selectedStep = currentLevel.recipe[currentStep])
        {
            // check if this is the last step of the puzzle, if not move to next step
            if (currentStep != currentLevel.recipe.Count-1)
            {
            // signal correct choice and increase currentStep
            currentStep ++;
            // change displayed object to match
            Destroy(displayedStep);
            Instantiate(currentLevel.recipe[currentStep].displayPrefab, displaySpawn.transform);
            displayedStep = displaySpawn.transform.GetChild(0).gameObject;
            }
            else
            {
                // move to next puzzle
                levelNumber++;
                NewPuzzle();
            }
        }
        // if not:
        else
        {
            // check if the player can make any more mistakes
            if (candlesLeft == 1)
            {
                // game over state
            }
            else
            {
            // turn off candle, player feedback etc.
            // candles[candlesLeft-1].GameObject.SetActive(false);
            candlesLeft --;
            }
        }
    }

    void NewPuzzle()
    {
        currentLevel = levels.levels[levelNumber];
        currentStep = 0;
    }
}
