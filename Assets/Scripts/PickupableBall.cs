using UnityEngine;

public class PickupableBall : PickupableItem
{
    private void Update()
    {
        transform.Rotate(0f, 0f, -180f * Time.deltaTime);
    }
}