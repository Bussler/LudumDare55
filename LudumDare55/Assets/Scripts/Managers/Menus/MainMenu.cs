using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    public static MenuManager Instance { get; private set; }

    private Canvas startMenu;

    private Canvas optionsMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startMenu = GameObject.Find("StartMenu").GetComponent<Canvas>();

        startMenu.enabled = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        startMenu.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        if (Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
    }
}