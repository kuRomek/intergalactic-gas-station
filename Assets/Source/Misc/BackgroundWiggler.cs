using UnityEngine;

public class BackgroundWiggler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundImage;

    private float _timer = 0f;

    public Vector3 Offset { get; private set; }

    private void Update()
    {
        _timer += Time.deltaTime / 10f;

        Offset = new Vector3((Mathf.PerlinNoise(_timer, 0f) - 0.5f) / 80f,
                             (Mathf.PerlinNoise(0f, _timer) - 0.5f) / 80f,
                             0f);

        transform.position += Offset;

        if (transform.position.y <= -_backgroundImage.size.y * 2f)
            transform.position += Vector3.up * _backgroundImage.size.y * 4f;
        else if (transform.position.y >= _backgroundImage.size.y * 2f)
            transform.position += Vector3.down * _backgroundImage.size.y * 4f;
        if (transform.position.x <= -_backgroundImage.size.x)
            transform.position += Vector3.right * _backgroundImage.size.x * 2f;
        else if (transform.position.x >= _backgroundImage.size.x)
            transform.position += Vector3.left * _backgroundImage.size.x * 2f;
    }
}
