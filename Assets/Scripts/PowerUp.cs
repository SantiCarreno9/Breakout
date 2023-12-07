using UnityEngine;

public enum PowerUps
{
    Grow,
    Laser
}

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private PowerUps _powerUp = PowerUps.Grow;
    private float _speed = 2;

    private PowerUpsManager _powerUpsManager;
    public PowerUps Power => _powerUp;

    public void SetManager(PowerUpsManager powerUpsManager) => _powerUpsManager = powerUpsManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _powerUpsManager.ActivatePowerUp(this);
    }

    private void Update()
    {
        transform.position += Vector3.down * _speed * Time.deltaTime;
    }

}
