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
        Flag8 = 1 << 7
    }

    private StoryFlags storyFlags = StoryFlags.None;

    public StoryFlags StoryFlags1 { get => storyFlags; set => storyFlags = value; }

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
        storyFlags = StoryFlags.None;
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
