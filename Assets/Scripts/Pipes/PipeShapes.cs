using System;
using UnityEngine;

namespace IntergalacticGasStation
{
    namespace Pipes
    {
        [CreateAssetMenu(fileName = "Pipe Shapes", menuName = "Pipes", order = 55)]
        public class PipeShapes : ScriptableObject
        {
            [SerializeField] private Mesh[] _shapes;

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
                    mesh = _shapes[_shapes.Length - 2];

                if (entriesCount == 3)
                {
                    mesh = _shapes[_shapes.Length - 3];

                    int noConnectionIndex = Array.IndexOf(entries, false);

                    rotation = 180f + ((entries.Length - 1f - noConnectionIndex) * 90f);
                }

                if (entriesCount == 2)
                {
                    for (int i = 0; i < entries.Length - 1; i++)
                    {
                        if (Convert.ToInt32(entries[i]) + Convert.ToInt32(entries[i + 1]) == 2)
                        {
                            mesh = _shapes[1];

                            rotation = 180f - (90f * i);
                        }
                    }

                    if (Convert.ToInt32(entries[0]) + Convert.ToInt32(entries[entries.Length - 1]) == 2)
                    {
                        mesh = _shapes[1];

                        rotation = -90f;
                    }
                }

                if (mesh == null)
                {
                    mesh = _shapes[0];

                    if (entries[1] == true || entries[3] == true)
                        rotation = 90f;
                }

                return mesh;
            }
        }
    }
}
