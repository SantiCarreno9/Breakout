using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _speed = 2.5f;

    private float _timeStuck = 0;
    private float _maxTimeStuck = 5;

    private bool _isMoving = false;
    private bool _sendToSpawningPosition = true;

    private Vector2 _derouteDirection = Vector2.zero;

    private void Update()
    {
        if (_sendToSpawningPosition)
            transform.position = GameManager.Instance.Paddle.BallSpawningPoint.position;

        if (_isMoving)
        {
            bool isXZero = Math.Round(_rigidbody.velocity.x, 1) == 0;
            bool isYZero = Math.Round(_rigidbody.velocity.y, 1) == 0;
            if (isXZero || isYZero)
                _timeStuck += Time.deltaTime;
            else _timeStuck = 0;            

            _derouteDirection=isXZero?Vector2.right:Vector2.up;
        }
    }

    private void Deroute(Vector2 currentDirection)
    {
        Vector2 deroute = (Vector2.Perpendicular(currentDirection) * UnityEngine.Random.Range(-1.0f, 1.0f));
        _rigidbody.AddForce((currentDirection + deroute) * _speed, ForceMode2D.Impulse);
    }

    public void Shoot()
    {
        _sendToSpawningPosition = false;
        _rigidbody.velocity = Vector2.zero;
        Vector2 initialDirection = Vector2.up;
        Vector2 direction = (Vector2.Perpendicular(initialDirection) * UnityEngine.Random.Range(-1.0f, 1.0f));
        _rigidbody.AddForce((initialDirection + direction) * _speed, ForceMode2D.Impulse);
        _isMoving = true;
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
            return;

        Vector2 normalized = _rigidbody.velocity.normalized;
        _rigidbody.velocity = normalized * _speed;
    }

    public void Show()
    {
        _spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }

    public void Reset()
    {
        _rigidbody.velocity = Vector2.zero;
        _isMoving = false;
        _sendToSpawningPosition = true;
    }

    public void MoveOutOfBounds()
    {
        transform.position = new Vector2(100, 100);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            GameManager.Instance.LoseLife();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_timeStuck > _maxTimeStuck)
        {            
            Deroute(_derouteDirection);
            _timeStuck = 0;
        }
    }

}
