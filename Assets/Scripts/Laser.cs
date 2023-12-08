using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 7;
    private LaserManager _laserManager = default;

    public void SetManager(LaserManager laserManager) => _laserManager = laserManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Block block))
            block.Hit(true);
        _laserManager.EnqueueLaser(this);
    }

    private void Update()
    {
        transform.position += Vector3.up * _speed * Time.deltaTime;
    }
}
