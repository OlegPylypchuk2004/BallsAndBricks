using UnityEngine;

[CreateAssetMenu(fileName = "NewBallsColorGradient", menuName = "Data/BallsColorGradient")]
public class BallsColorGradientData : ScriptableObject
{
    [field: SerializeField] public Gradient Gradient { get; private set; }
}