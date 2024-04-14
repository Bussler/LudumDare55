using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameMenu : MonoBehaviour
{
    public GameObject menuPanel; // Assign this in the inspector
    public TextMeshProUGUI gunStats;

    private bool isPaused = false;

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        menuPanel.SetActive(true);
        isPaused = true;
        GenerateGunStats();
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming game");
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        isPaused = false;
    }

    private void GenerateGunStats()
    {
        gunStats.text = "";
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            ShootingComponent shootingComponent = player.GetComponent<ShootingComponent>();
            if (shootingComponent != null)
            {
                Gun currentGun = shootingComponent.currentGun;
                
                // Display gun stats
                gunStats.text += "Type: "+ currentGun.type.ToString() + "\n";
                gunStats.text += "Damage: " + currentGun.Damage.ToString() + "\n";
                gunStats.text += "FireRate: " + currentGun.FireRate.ToString() + "\n";

                gunStats.text += "Accuracy: " + currentGun.Accuracy.ToString() + "\n";
                gunStats.text += "TimeLimit: " + currentGun.TimeLimit.ToString() + "\n";
                gunStats.text += "Range: " + currentGun.Range.ToString() + "\n";
                gunStats.text += "BulletAmount: " + currentGun.BulletAmount.ToString() + "\n";
                gunStats.text += "BulletSpeed: " + currentGun.BulletSpeed.ToString() + "\n";
                gunStats.text += "BulletSize: " + currentGun.BulletSize.ToString() + "\n";
                gunStats.text += "BulletKnockback: " + currentGun.BulletKnockback.ToString() + "\n";
                gunStats.text += "BulletHealth: " + currentGun.BulletHealth.ToString() + "\n";
                gunStats.text += "LifeSteal: " + currentGun.LifeSteal.ToString() + "\n";
                
                gunStats.text += "Effects:\n";
                gunStats.text += "- Amount: " + currentGun.Effects.Count.ToString() + "\n";

                // TODO
                //foreach (ObjectEffect effect in currentGun.Effects)
                //{
                //    gunStats.text += "Effect: " + effect.ToString() + "\n";
                //}
            }
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }
}
