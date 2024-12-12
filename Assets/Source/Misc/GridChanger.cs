using System.Linq;
using UnityEngine;

public class GridChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] _gridLayouts;
    [SerializeField] private ParticleSystem _changingEffect;

    private GameObject _currentLayout = null;

    public void Change()
    {
        GameObject[] gridLayouts = _gridLayouts.Where(layout => layout != _currentLayout).ToArray();

        if (_currentLayout != null)
        {
            _currentLayout.SetActive(false);

            foreach (Transform child in _currentLayout.transform)
                child.gameObject.SetActive(false);
        }

        _currentLayout = gridLayouts[Random.Range(0, gridLayouts.Length)];

        _currentLayout.SetActive(true);

        foreach (Transform child in _currentLayout.transform)
            child.gameObject.SetActive(true);

        _changingEffect.Play();
    }
}