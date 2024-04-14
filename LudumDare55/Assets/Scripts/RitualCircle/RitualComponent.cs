using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualComponent : MonoBehaviour
{
    public int EffectTypeIndex;

    public ObjectEffect Effect;

    public void Start()
    {
        Effect = EffectManager.instance.RandomOrderedEffects[EffectTypeIndex];
    }

}
