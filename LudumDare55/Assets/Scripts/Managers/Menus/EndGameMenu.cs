using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject canvas;
    
    public void ShowEndGameMenu(int score) {
        text.text = "Game Over!\n > " + score + " <";
        canvas.SetActive(true);
    }

    void Awake() {
        canvas.SetActive(false);

        ProgressionManager.Instance.GameEnded.Subscribe(_ => {
            ShowEndGameMenu(LootLockerPlayermanager.Instance.Score);
        })
        .AddTo(this);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        canvas.SetActive(false);
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }

}
