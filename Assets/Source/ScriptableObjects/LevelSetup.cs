using UnityEngine;

[CreateAssetMenu(fileName="Level", menuName="Level/LevelSetup", order=52)]
public class LevelSetup : ScriptableObject
{
    [SerializeField] private ShipSetup[] _ships;
    [SerializeField] private TankSetup[] _tanks;
    [SerializeField] private int _timeInSeconds = 180;

    public ShipSetup[] Ships => _ships;

    public TankSetup[] Tanks => _tanks;

    public int TimeInSeconds => _timeInSeconds;
}
