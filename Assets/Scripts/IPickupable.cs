using System;

public interface IPickupable
{
    public event Action<IPickupable> Picked;
    public void Pickup();
}