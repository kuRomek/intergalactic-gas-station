using UnityEngine;

public class BackgroundWiggler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundImage;
    [SerializeField, Min(0f)] private float _smoothness = 5f;
    [SerializeField, Min(0f)] private float _speed = 0.005f;

    private float _timer = 0f;

    public Vector3 Offset { get; private set; }

    private void Update()
    {
        _timer += Time.deltaTime / _smoothness;

        Offset = new Vector3((Mathf.PerlinNoise(_timer, (int)_timer * 0.01f) - 0.5f) * _speed,
                             (Mathf.PerlinNoise((int)_timer * 0.01f, _timer) - 0.5f) * _speed,
                             0f);

        transform.position += Offset;

        if (transform.position.y <= -_backgroundImage.size.y)
            transform.position += _backgroundImage.size.y * 2f * Vector3.up;
        else if (transform.position.y >= _backgroundImage.size.y)
            transform.position += _backgroundImage.size.y * 2f * Vector3.down;
        if (transform.position.x <= -_backgroundImage.size.x)
            transform.position += _backgroundImage.size.x * 2f * Vector3.right;
        else if (transform.position.x >= _backgroundImage.size.x)
            transform.position += _backgroundImage.size.x * 2f * Vector3.left;
    }
}
