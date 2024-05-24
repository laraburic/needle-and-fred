using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// To add a level, just look for the bits that say "MAKE A CHANGE" by pressing CTRL + F
/// </summary>
/// 
public class LevelLoader : EditorWindow
{
    // MAKE A CHANGE - copy and paste any of these lines for any additional scripts
    public string cameraLightsScene = "Assets/Scenes/Sarah/Camera.unity";
    public string environmentScene = "Assets/Scenes/Sarah/Environment.unity";
    public string mikeScene = "Assets/Scenes/Mike/Mike.unity";
    public string celinaScene = "Assets/Scenes/Celina/Celina.unity";
    
    [MenuItem("TeamSpinnySword/Level Loader")]
    public static void ShowWindow()
    {
        GetWindow<LevelLoader>("Level Loader");
    }
    
    void OnGUI()
    {
        EditorGUILayout.Space(10);
        // MAKE A CHANGE - copy and paste any of these lines for any additional scene
        cameraLightsScene = EditorGUILayout.TextField("Path to camera/lights scene", cameraLightsScene);
        environmentScene = EditorGUILayout.TextField("Path to env scene", environmentScene);
        mikeScene = EditorGUILayout.TextField("Path to Mike's scene", mikeScene);
        celinaScene = EditorGUILayout.TextField("Path to Celina's scene", celinaScene);
        
        EditorGUILayout.Space(10);
        if (GUILayout.Button("Load Scenes"))
        {
            // Check if the user wants to save any changes first
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // Load the main scene
                EditorSceneManager.OpenScene(cameraLightsScene, OpenSceneMode.Single);
            
                // Additionally load everything else 
                // MAKE A CHANGE - Add your new scene to this by copy and pasting and updating an existing one
                EditorSceneManager.OpenScene(environmentScene, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(mikeScene, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(celinaScene, OpenSceneMode.Additive);
            }
        }
    }
}
