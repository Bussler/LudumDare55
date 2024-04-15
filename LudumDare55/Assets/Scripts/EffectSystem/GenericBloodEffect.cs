using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GenericBloodEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(_ => Destroy(this)).AddTo(this);
    }

}
