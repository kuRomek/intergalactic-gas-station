using System;
using UnityEngine;

namespace Pipes
{
    [CreateAssetMenu(fileName = "Pipe Shapes", menuName = "Pipes", order = 55)]
    public class PipeShapes : ScriptableObject
    {
        [SerializeField] private Mesh[] _shapes;

        private int _twoStraightEntriesIndex = 0;
        private int _twoCurvedEntriesIndex = 1;
        private int _threeEntriesIndex = 2;
        private int _fourEntriesIndex = 3;
        private float _straightAngle = 90f;

        public Mesh GetShape(bool[] entries, out float rotation)
        {
            int entriesCount = 0;

            Mesh mesh = null;
            rotation = 0f;

            foreach (bool entry in entries)
            {
                if (entry == true)
                    entriesCount++;
            }

            if (entriesCount == 4)
                mesh = _shapes[_fourEntriesIndex];

            if (entriesCount == 3)
            {
                mesh = _shapes[_threeEntriesIndex];

                int noConnectionIndex = Array.IndexOf(entries, false);

                rotation = 180f + ((entries.Length - 1f - noConnectionIndex) * _straightAngle);
            }

            if (entriesCount == 2)
            {
                for (int i = 0; i < entries.Length - 1; i++)
                {
                    if (Convert.ToInt32(entries[i]) + Convert.ToInt32(entries[i + 1]) == 2)
                    {
                        mesh = _shapes[_twoCurvedEntriesIndex];

                        rotation = 180f - (_straightAngle * i);
                    }
                }

                if (Convert.ToInt32(entries[0]) + Convert.ToInt32(entries[entries.Length - 1]) == 2)
                {
                    mesh = _shapes[_twoCurvedEntriesIndex];

                    rotation = -_straightAngle;
                }
            }

            if (mesh == null)
            {
                mesh = _shapes[_twoStraightEntriesIndex];

                if (entries[1] == true || entries[3] == true)
                    rotation = _straightAngle;
            }

            return mesh;
        }
    }
}
