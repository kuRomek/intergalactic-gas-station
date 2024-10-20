using System.Collections;
using System.Linq;
using UnityEngine;

public class TankContainerShifter : MonoBehaviour
{
    private TankContainer _tankContainer;
    private Coroutine _shifting;

    private void OnDisable()
    {
        _tankContainer.FirstTankRemoved -= OnTankEmptied;
    }

    public void Init(TankContainer tankContainer)
    {
        _tankContainer = tankContainer;
        _tankContainer.FirstTankRemoved += OnTankEmptied;

        enabled = true;
    }

    private void OnTankEmptied(Vector3 shift)
    {
        if (_shifting != null)
            StopCoroutine(_shifting);

        _shifting = StartCoroutine(ShiftTanks(shift));
    }

    private IEnumerator ShiftTanks(Vector3 shift)
    {
        yield return new WaitForFixedUpdate();

        Vector3[] startPositions = _tankContainer.Select(tank => tank.Position).ToArray();

        while (_tankContainer.Peek().Position != startPositions[0] + shift)
        {
            int i = 0;

            foreach (Tank tank in _tankContainer)
            {
                tank.MoveTo(Vector3.Lerp(tank.Position, startPositions[i] + shift, 10f * Time.deltaTime));
                i++;
            }

            yield return null;
        }
    }
}
