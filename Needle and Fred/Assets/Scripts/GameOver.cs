using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject gameoverUI;

    public void GameIsOver()
    {
        gameoverUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        AudioListener.pause = true;
    }

    public void Retry()
    {
        gameoverUI.SetActive(false);
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.pause = false;
        FindObjectOfType<AudioManager>().Play("Retry");

    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene(0);
        AudioListener.pause = false;
        FindObjectOfType<AudioManager>().Play("ReturnToMenu");
    }

    public void LoadCredits()
    {
        Debug.Log("Loading Credits...");
        SceneManager.LoadScene(2);
        AudioListener.pause = false;
        FindObjectOfType<AudioManager>().Play("GoToCredits");
    }

    public void QuitGame()
    {
        Debug.Log("Quiting Game...");
        Application.Quit();
    }
}
