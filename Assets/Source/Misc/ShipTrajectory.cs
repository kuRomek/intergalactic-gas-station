using UnityEngine;

public class ShipTrajectory : MonoBehaviour
{
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _refuelingPoint;
    [SerializeField] private Transform _finish;

    public Transform Start => _start;
    public Transform RefuelingPoint => _refuelingPoint;
    public Transform Finish => _finish;
}
