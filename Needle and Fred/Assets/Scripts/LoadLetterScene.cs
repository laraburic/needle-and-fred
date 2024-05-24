using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLetterScene : MonoBehaviour
{
    public void LoadScene() {
        SceneManager.LoadSceneAsync("IntroLetter", LoadSceneMode.Additive);
    }
}
