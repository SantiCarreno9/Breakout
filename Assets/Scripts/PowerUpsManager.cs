using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField]
    private PowerUp _growPowerUp;
    [SerializeField]
    private PowerUp _doubleBallPowerUp;

    private Queue<PowerUp> _growPowerUpPool = new Queue<PowerUp>();
    private Queue<PowerUp> _doublePowerUpPool = new Queue<PowerUp>();

    private PowerUp InstantiatePowerUp(PowerUps powerUp)
    {
        PowerUp item;

        if (powerUp == PowerUps.Grow)
        {
            if (_growPowerUpPool.Count == 0)
            {
                item = Instantiate(_growPowerUp);
                item.SetManager(this);
            }
            else item = _growPowerUpPool.Dequeue();
        }
        else
        {
            if (_doublePowerUpPool.Count == 0)
            {
                item = Instantiate(_growPowerUp);
                item.SetManager(this);
            }
            else item = _doublePowerUpPool.Dequeue();
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
        int randomPower = Random.Range(0, 2);
        PowerUp powerUp = InstantiatePowerUp((PowerUps)randomPower);
        powerUp.transform.position = position;
    }

    //public void HideAllPowerUps()
    //{
    //    for (int i = 0; i < _freezePowerUps.Length; i++)
    //        _freezePowerUps[i].gameObject.SetActive(false);

    //    for (int i = 0; i < _turboPowerUps.Length; i++)
    //        _turboPowerUps[i].gameObject.SetActive(false);
    //}        

    public void ActivatePowerUp(PowerUp powerUp)
    {
        GameManager.Instance.ActivatePowerUp(powerUp.Power);
        EnqueuePowerUp(powerUp);
    }

    public void EnqueuePowerUp(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(false);
        if (powerUp.Power == PowerUps.Grow)
            _growPowerUpPool.Enqueue(powerUp);
        else _doublePowerUpPool.Enqueue(powerUp);
    }

    public void Reset()
    {
        //for (int i = 0; i < length; i++)
        //{

        //}
    }
}
