using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] Scoreboardtexts;

    [SerializeField]
    private TMP_InputField PlayerName;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            OnLoadMenuScene();
        }
    }

    public void OnLoadMenuScene()
    {
        StartCoroutine(UpdateScoreboard());
    }

    public void UpdateDisplayedPlayerName()
    {
        if (LootLockerPlayermanager.Instance != null)
        {
            PlayerName.text = LootLockerPlayermanager.Instance.GetPlayerName();
        }
    }

    public void SetPlayerName(string newName)
    {
        if (LootLockerPlayermanager.Instance != null)
        {
            LootLockerPlayermanager.Instance.SetPlayerName(PlayerName.text);
            StartCoroutine(UpdateScoreboard());
        }
    }

    public IEnumerator UpdateScoreboard()
    {
        yield return new WaitForSeconds(0.1f);

        UpdateDisplayedPlayerName();

        if (LootLockerPlayermanager.Instance != null)
        {
            StartCoroutine(LootLockerPlayermanager.Instance.LootLockerScoreDownload(Scoreboardtexts.Length, (response) =>
            {
                if (response != null)
                {
                    for (int i = 0; i < Scoreboardtexts.Length; i++)
                    {
                        if (i >= response.Length)
                        {
                            Scoreboardtexts[i].text = "";
                            continue;
                        }

                        string playername = string.IsNullOrEmpty(response[i].player.name) ? response[i].member_id : response[i].player.name;
                        Scoreboardtexts[i].text = response[i].rank + ". " + playername + " Score: " + response[i].score;
                    }
                }
            }));
        }
    }
}
