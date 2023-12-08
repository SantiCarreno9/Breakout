using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField]
    private Transform _powerUpContainer = default;
    [SerializeField]
    private PowerUp _growPowerUp = default;
    [SerializeField]
    private PowerUp _laserPowerUp = default;
    [SerializeField]
    private LaserManager _laserManager = default;
    [SerializeField]
    private PaddleSoundEffects _paddleSoundEffects = default;

    private Queue<PowerUp> _growPowerUpPool = new Queue<PowerUp>();
    private Queue<PowerUp> _laserPowerUpPool = new Queue<PowerUp>();

    private List<PowerUp> _spawnedPowerUps = new List<PowerUp>();

    private void Start()
    {
        _laserManager.OnLaserShot += () =>
        {
            _paddleSoundEffects.PlayShootSound();
        };
    }
    private PowerUp InstantiatePowerUp(PowerUps powerUp)
    {
        PowerUp item;

        if (powerUp == PowerUps.Grow)
        {
            if (_growPowerUpPool.Count == 0)
            {
                item = Instantiate(_growPowerUp, _powerUpContainer);
                item.SetManager(this);
            }
            else item = _growPowerUpPool.Dequeue();
        }
        else
        {
            if (_laserPowerUpPool.Count == 0)
            {
                item = Instantiate(_laserPowerUp);
                item.SetManager(this);
            }
            else item = _laserPowerUpPool.Dequeue();
        }
        item.gameObject.SetActive(true);

        return item;
    }

    public void TryToSpawnPowerUp(Vector3 position)
    {
        int randomValue = Random.Range(0, 100);
        if (randomValue > 70)
            SpawnRandomPowerUp(position);
    }

    public void SpawnRandomPowerUp(Vector3 position)
    {
        int random = Random.Range(0, 100);
        PowerUps randomPowerUp = (random < 50) ? PowerUps.Grow : PowerUps.Laser;
        PowerUp powerUp = InstantiatePowerUp(randomPowerUp);
        powerUp.transform.position = position;
        if (!_spawnedPowerUps.Contains(powerUp))
            _spawnedPowerUps.Add(powerUp);
    }

    public void DeactivatePowerUps()
    {
        GameManager.Instance.Paddle.SetNormalSize();
        _laserManager.DisableLaser();
        _laserManager.HideAll();
    }

    public void HidePowerUps()
    {
        for (int i = 0; i < _spawnedPowerUps.Count; i++)
        {
            if (_spawnedPowerUps[i].gameObject.activeSelf)
                EnqueuePowerUp(_spawnedPowerUps[i]);
        }
    }

    public void ActivatePowerUp(PowerUp powerUp)
    {
        switch (powerUp.Power)
        {
            case PowerUps.Grow:
                GameManager.Instance.Paddle.GrowUp();
                break;
            case PowerUps.Laser:
                _laserManager.EnableLaser();
                break;
            default:
                break;
        }
        _paddleSoundEffects.PlayPowerUpSound();
        EnqueuePowerUp(powerUp);
    }

    public void EnqueuePowerUp(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(false);
        if (powerUp.Power == PowerUps.Grow)
        {
            if (!_growPowerUpPool.Contains(powerUp))
                _growPowerUpPool.Enqueue(powerUp);
        }
        else
        {
            if (!_laserPowerUpPool.Contains(powerUp))
                _laserPowerUpPool.Enqueue(powerUp);
        }
    }

    public void Reset()
    {
        DeactivatePowerUps();
        HidePowerUps();
    }
}
