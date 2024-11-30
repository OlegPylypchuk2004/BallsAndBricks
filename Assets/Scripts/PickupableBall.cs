using System;
using UnityEngine;

public class PickupableBall : MonoBehaviour, IPickupable
{
    public event Action<IPickupable> Picked;

    public void Pickup()
    {
        Picked?.Invoke(this);
    }
}