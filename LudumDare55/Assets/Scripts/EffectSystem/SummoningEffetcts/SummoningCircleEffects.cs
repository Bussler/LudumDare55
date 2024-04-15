using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SummoningCircleEffects : MonoBehaviour
{
    [SerializeField] private float durationSec = 3.0f;
    [SerializeField] private 

    // Start is called before the first frame update
    void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(durationSec))
            .Subscribe(x => OnEffectExpire())
            .AddTo(this);
    }

    private void OnEffectExpire() {
        Destroy(gameObject);
    }
}
