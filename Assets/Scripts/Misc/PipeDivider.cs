using UnityEngine;

namespace IntergalacticGasStation
{
    namespace Misc
    {
        public class PipeDivider : MonoBehaviour
        {
            [SerializeField] private int[] _cell1;
            [SerializeField] private int[] _cell2;

            public int[][] Connection => new int[2][] { _cell1, _cell2 };
        }
    }
}
