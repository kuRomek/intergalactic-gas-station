using System.Collections;
using System.Linq;
using UnityEngine;

public class TankContainerShifter : MonoBehaviour
{
    private const float DistanceTolerance = 0.01f;

    [SerializeField] private float _speed = 10f;

    private TankContainer _tankContainer;
    private Coroutine _shifting;

    private void OnEnable()
    {
        _tankContainer.FirstTankRemoved += OnFirstTankRemoved;
    }

    private void OnDisable()
    {
        _tankContainer.FirstTankRemoved -= OnFirstTankRemoved;
    }

    public void Init(TankContainer tankContainer)
    {
        _tankContainer = tankContainer;
        enabled = true;
    }

    private void OnFirstTankRemoved(Vector3 shift)
    {
        if (_shifting != null)
            StopCoroutine(_shifting);

        _shifting = StartCoroutine(ShiftTanks(shift));
    }

    private IEnumerator ShiftTanks(Vector3 shift)
    {
        yield return null;

        Vector3[] startPositions = _tankContainer.Select(tank => tank.Position).ToArray();

        int i;

        while (Vector3.SqrMagnitude(startPositions[0] + shift - _tankContainer.Peek().Position) > DistanceTolerance)
        {
            i = 0;

            foreach (Tank tank in _tankContainer)
            {
                tank.MoveTo(Vector3.Lerp(tank.Position, startPositions[i] + shift, _speed * Time.deltaTime));
                i++;
            }

            yield return null;

            if (_tankContainer.Peek() == null)
                break;
        }

        i = 0;

        foreach (Tank tank in _tankContainer)
        {
            tank.MoveTo(startPositions[i] + shift);
            i++;
        }

        _tankContainer.StopShifting();
    }
}
