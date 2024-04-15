using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffect : ScriptableObject
{
    public int Change;
    public GameObject ParticleSystem;
    public Color color;
    public virtual void ApplyEffect(Object obj)
    {
    }
    public virtual void RemoveEffect(Object obj)
    {
    }
}
