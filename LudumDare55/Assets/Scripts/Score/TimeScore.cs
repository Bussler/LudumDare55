using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TimeScore : MonoBehaviour
{
    [SerializeField] private int totalTimePoints = 1000000;

    [SerializeField] private int decreasePerInterval = 10;
    [SerializeField] private float decreaseIntervalSec = 1.0f;

    private ReactiveProperty<int> timePointState;

    public ReadOnlyReactiveProperty<int> Score => timePointState.ToReadOnlyReactiveProperty();

    void Awake() {
        timePointState = new ReactiveProperty<int>(totalTimePoints);

        Observable.Interval(TimeSpan.FromSeconds(decreasePerInterval))
            .Subscribe(_ => OnTimeScoreTick())
            .AddTo(this);
        
        ProgressionManager.Instance.GameEnded
            .Subscribe(_ => ApplyScore())
            .AddTo(this);

    }

    private void OnTimeScoreTick() {
        timePointState.Value -= decreasePerInterval;
    }

    private void ApplyScore() {
        if (LootLockerPlayermanager.Instance == null) { 
            return; 
        }
        LootLockerPlayermanager.Instance.AddScore(Score.Value);
    }

}
