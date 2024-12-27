using UnityEngine;

namespace IntergalacticGasStation
{
    namespace VFX
    {
        [RequireComponent(typeof(Animator))]
        public class TimeAdding : MonoBehaviour
        {
            public readonly int ShipRefueled = Animator.StringToHash(nameof(ShipRefueled));
            private bool _isFirstShow = true;
            private Animator _animator;

            private void Awake()
            {
                _animator = GetComponent<Animator>();
                gameObject.SetActive(false);
            }

            public void Show()
            {
                if (_isFirstShow)
                {
                    gameObject.SetActive(true);
                    _isFirstShow = false;
                }

                _animator.SetTrigger(ShipRefueled);
            }
        }
    }
}
