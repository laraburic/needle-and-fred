using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// To get the right scenes to play from a standalone build, we need to go to File > Build Settings
/// and make sure that Scenes/Sarah/Game is Scene 0 - this is the first one that will load
/// then everything else will be loaded on top using the logic below
/// </summary>
public class GameManager : MonoBehaviour
{
    // put the name of all of the scenes we want to load additively here
    public List<string> startingScenes;
    
    private void Awake() {
        // By putting Application.isEditor, we only load the scenes if we're not playing through the Editor (like a standalone Windows build)
        if (!Application.isEditor) {
            // for every scene in the list we defined
            foreach (string level in startingScenes) {
                // load the scene additively
                SceneManager.LoadScene(level, LoadSceneMode.Additive);
            }
        }
    }

    /// <summary>
    /// / This is an extra public function that we can use in button presses to load more scenes.
    /// For example, when the player presses a button close a letter, we might want to load a scene.
    /// </summary>
    public void LoadScene(string scene) {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }
}
