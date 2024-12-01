using System;
using UnityEngine;

public abstract class PickupableItem : MonoBehaviour
{
    public event Action<PickupableItem> Picked;

    public virtual void Pickup()
    {
        Picked?.Invoke(this);
    }
}