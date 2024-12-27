using UnityEngine;
using Zenject;
using IntergalacticGasStation.Misc;

namespace IntergalacticGasStation
{
    namespace LevelGrid
    {
        public class GridInstaller : MonoInstaller
        {
            [SerializeField] private Transform _pipeDividers;

            public override void InstallBindings()
            {
                Container.BindInterfacesAndSelfTo<Grid>().
                    AsSingle().
                    WithArguments(_pipeDividers.GetComponentsInChildren<PipeDivider>(true)).
                    NonLazy();
            }
        }
    }
}