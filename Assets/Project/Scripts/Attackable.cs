using System;
using UnityEngine;
using UnityEngine.Events;
public class Attackable : MonoBehaviour
{
    [Serializable]
    public class InflictDamageEvent : UnityEvent<int> { }
    public InflictDamageEvent InflictDamage;
    public void OnInflictDamage(int damage)
    {
        if (InflictDamage != null)
        {
            InflictDamage.Invoke(damage);
        }
    }
}

