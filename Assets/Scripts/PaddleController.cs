using System.Collections;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    [SerializeField]
    private Transform _rangeBox = default;
    [SerializeField]
    private Transform _ballSpawningPoint = default;

    //[SerializeField]
    //private 
    [SerializeField]
    private PaddleVisualController _spaceshipVisualController = default;

    public Transform BallSpawningPoint => _ballSpawningPoint;

    [SerializeField]
    private float _speed = 10;

    private float _paddleWidth = 0;

    private Vector2 _movementBounds;

    private float _input = 0;
    private Vector2 _defaultPosition = Vector2.zero;

    private float _initialScale = 0;
    private float _maxSize = 0;

    private bool _enableLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        _initialScale = transform.localScale.x;
        _maxSize = _initialScale * 4;
        _defaultPosition = transform.position;
        float xHalfScale = _rangeBox.localScale.x / 2;
        _movementBounds = new Vector2(_rangeBox.position.x - xHalfScale, _rangeBox.position.x + xHalfScale);
        _paddleWidth = transform.localScale.x / 2;
    }

    private void Update()
    {
        GetUserInput();
    }

    private void FixedUpdate()
    {
        MovePaddle();
    }

    /// <summary>
    /// Adjusts the paddle position within the screen bounds
    /// </summary>
    private void LateUpdate()
    {
        Vector2 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, _movementBounds.x + _paddleWidth, _movementBounds.y - _paddleWidth);
        transform.position = clampedPosition;
    }

    #region CONTROL

    private void GetUserInput()
    {
        _input = 0;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            _input = 1;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            _input = -1;

        if (_enableLaser)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ShootLaser();
        }
    }

    private void MovePaddle()
    {
        _rigidbody.velocity = Vector2.right * _speed * _input;
    }

    public void GoToDefaultPosition()
    {
        transform.position = _defaultPosition;
    }

    #endregion

    #region POWER-UPS

    public void GrowUp()
    {
        if (transform.localScale.x >= _maxSize)
            return;

        transform.localScale = new Vector2(transform.localScale.x * 2, transform.localScale.y);
        //_spaceshipVisualController.SetNormalState();        
    }

    public void EnableLaser()
    {
        _enableLaser = true;
    }

    public void ShootLaser()
    {

    }

    public void DisablePowerUps()
    {
        transform.localScale = new Vector2(_initialScale, transform.localScale.y);
    }

    #endregion

}
