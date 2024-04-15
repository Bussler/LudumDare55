using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class EndGameMenu : MonoBehaviour
{
    public static EndGameMenu Instance;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject canvas;
    
    public void ShowEndGameMenu() {
        Time.timeScale = 0;
        var score = -1;
        if (LootLockerPlayermanager.Instance != null)
            score = LootLockerPlayermanager.Instance.Score;
        text.text = "Game Over!\n > " + score + " <";
        canvas.SetActive(true);
    }

    private void Awake()
    {
        canvas.SetActive(false);
        
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start() {
        //ProgressionManager.Instance.GameEnded.Subscribe(_ => {
        //    ShowEndGameMenu();
        //})
        //.AddTo(this);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        canvas.SetActive(false);
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }

}
