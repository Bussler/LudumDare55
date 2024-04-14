using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public List<ObjectEffect> Effects;
    public List<ObjectEffect> RandomOrderedEffects;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GenerateEfffects();
    }


    public void GenerateEfffects()
    {
        RandomOrderedEffects = new List<ObjectEffect>();
        List<ObjectEffect> copiedEffects =new List<ObjectEffect>(Effects);
        int effectAmount = copiedEffects.Count;

        for(int i=0; i < effectAmount;i++)
        {
            int x = Random.Range(0, copiedEffects.Count);
            RandomOrderedEffects.Add(copiedEffects[x]);
            copiedEffects.RemoveAt(x);
        }
    }


    
}
