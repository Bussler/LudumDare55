using LootLocker.Requests;
using System.Collections;
using UnityEngine;


/// <summary>
/// Class to manage the player's score and name in the LootLocker database. Implements the singleton pattern.
/// </summary>
public class LootLockerPlayermanager : MonoBehaviour
{
    public static LootLockerPlayermanager Instance;

    [SerializeField]
    private string leaderboardID = "21478";

    [SerializeField]
    private int score; // The player's curent score

    void Awake()
    {
        if (!LootLockerSDKManager.CheckInitialized())
            StartCoroutine(LootLockerGuestLogin());
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    /// <summary>
    /// Add a score to the player's current score.
    /// </summary>
    /// <param name="additionalScore">Score to add</param>
    public void AddScore(int additionalScore)
    {
        this.score += additionalScore;
    }

    /// <summary>
    /// Perform a basic guest login: Quick, but no player name by signup
    /// </summary>
    /// <returns></returns>
    IEnumerator LootLockerGuestLogin()
    {
        bool done = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("LootLockerLogin - success: " + response.text);
                // Save the player identifier, name in LootLocker PlayerPrefs
                PlayerPrefs.SetString("player_identifier", response.player_id.ToString());
                PlayerPrefs.SetString("player_name", response.player_name);
            }
            else
            {
                Debug.Log("LootLockerLogin - error: " + response.text);
            }
            done = true;
        });
        yield return new WaitWhile(() => done == false);
    }


    /// <summary>
    /// Get the player name from the LootLocker database, or the player identifier, if no name is set.
    /// </summary>
    /// <returns></returns>
    public string GetPlayerName()
    {
        return string.IsNullOrEmpty(PlayerPrefs.GetString("player_name")) ?
            PlayerPrefs.GetString("player_identifier") : PlayerPrefs.GetString("player_name");
    }

    /// <summary>
    /// Store the player name in the LootLocker database.
    /// </summary>
    /// <param name="name">Name of the current player. Defaults to "player"</param>
    public void SetPlayerName(string name = "player")
    {
        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success)
            {
                PlayerPrefs.SetString("player_name", name);
            }
        });
    }

    /// <summary>
    /// Upload the current score to the leaderboard.
    /// Assumes the player is already logged in and a leaderboard is set.
    /// </summary>
    /// <returns></returns>
    IEnumerator LootLockerScoreUpload()
    {
        bool done = true;
        yield return new WaitWhile(() => LootLockerSDKManager.CheckInitialized() == false);

        string currentPlayerID = PlayerPrefs.GetString("player_identifier");
        if (string.IsNullOrEmpty(currentPlayerID))
        {
            Debug.LogError("No player identifier found in PlayerPrefs.");
            yield break;
        }

        LootLockerSDKManager.SubmitScore(currentPlayerID, this.score, this.leaderboardID, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful score upload to LootLocker: " + this.score);
            }
            else
            {
                Debug.Log("Failed score upload to LootLocker: " + response.text);
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    /// <summary>
    /// Download the top scoreAmount scores from the leaderboard.
    /// </summary>
    /// <param name="scoreAmount">Amount of entries to load.</param>
    /// <param name="callback">IEnumerator result of type LootLockerLeaderboardMember[] holding the scoreboard results.</param>
    /// <returns></returns>
    public IEnumerator LootLockerScoreDownload(int scoreAmount, System.Action<LootLockerLeaderboardMember[]> callback)
    {
        bool done = false;
        yield return new WaitWhile(() => LootLockerSDKManager.CheckInitialized() == false);

        LootLockerSDKManager.GetScoreList(this.leaderboardID, scoreAmount, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful score download from LootLocker: " + response.text);

                LootLockerLeaderboardMember[] scores = response.items;
                callback(scores);

                done = true;
            }
            else
            {
                Debug.Log("Failed score download from LootLocker: " + response.text);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    /// <summary>
    /// Download the score of a specific player.
    /// </summary>
    /// <param name="playerID">ID of player to download the leaderboard score for.</param>
    /// <param name="callback">IEnumerator result of type int holding the scoreboard result.</param>
    /// <returns></returns>
    public IEnumerator LootLockerPlayerScoreDownload(string playerID, System.Action<int> callback)
    {
        bool done = false;
        yield return new WaitWhile(() => LootLockerSDKManager.CheckInitialized() == false);

        LootLockerSDKManager.GetMemberRank(this.leaderboardID, playerID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successful score download from LootLocker: " + response.text);
                callback(response.score);
                done = true;
            }
            else
            {
                Debug.Log("Failed score download from LootLocker: " + response.text);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }
}
