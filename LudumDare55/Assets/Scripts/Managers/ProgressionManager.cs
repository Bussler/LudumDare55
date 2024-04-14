using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manage progression in a bitflag.
/// Implement a singleton pattern.
/// </summary>
public class ProgressionManager : MonoBehaviour
{

    public static ProgressionManager Instance;

    private ReactiveProperty<bool> gameEnded = new ReactiveProperty<bool>(false);

    public ReadOnlyReactiveProperty<bool> GameEnded => gameEnded.ToReadOnlyReactiveProperty();

    [Flags]
    public enum StoryFlags
    {
        None = 0,
        Flag1 = 1 << 0,  // 1
        Flag2 = 1 << 1,  // 2
        Flag3 = 1 << 2,  // 4
        Flag4 = 1 << 3,  // 8
        Flag5 = 1 << 4,
        Flag6 = 1 << 5,
        Flag7 = 1 << 6,
        Flag8 = 1 << 7,
        FlagBossSpawned = 1 << 8,
    }

    private StoryFlags storyFlags = StoryFlags.None;
    
    private float runStartTime = 0f;
    private int numberOfSummons = 0;

    public int NumberOfSummons { get => numberOfSummons; set => numberOfSummons = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ResetValues();
    }
    
    /// <summary>
    /// Reset all values in the progression.
    /// This includes StoryFlags, runStartTime, numberOfSummons.
    /// </summary>
    public void ResetValues()
    {
        storyFlags = StoryFlags.Flag1;
        runStartTime = Time.time;
        numberOfSummons = 0;
    }

    /// <summary>
    /// Increase the number of summons stat.
    /// By default, it increases by 1.
    /// </summary>
    public void increaseSummonCounter(int counter = 1)
    {
        numberOfSummons += counter;
    }

    /// <summary>
    /// Returns the time since the run started.
    /// </summary>
    /// <returns></returns>
    public float GetRunTime()
    {
        return Time.time - runStartTime;
    }

    /// <summary>
    /// Set a flag in the progression.
    /// </summary>
    /// <param name="flag"></param>
    public void SetStoryFlag(StoryFlags flag)
    {
        storyFlags |= flag;
    }

    /// <summary>
    /// Check if a flag is set in the progression.
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public bool CheckStoryFlag(StoryFlags flag)
    {
        return (storyFlags & flag) == flag;
    }

    /// <summary>
    /// Check if all flags are set in the progression.
    /// </summary>
    /// <returns></returns>
    public bool CheckAllStoryFlags()
    {
        StoryFlags allFlags = StoryFlags.None;
        foreach (StoryFlags value in Enum.GetValues(typeof(StoryFlags)))
        {
            allFlags |= value;
        }

        return (storyFlags & allFlags) == allFlags;
    }

    /// <summary>
    /// Call this method, when the game ends.
    /// E.g. when the player dies or wins.
    /// </summary>
    public void EndGame()
    {
        if (LootLockerPlayermanager.Instance != null)
            LootLockerPlayermanager.Instance.UploadScore();
        // TODO do more stuff when game ends
        gameEnded.Value = true;
    }
}
