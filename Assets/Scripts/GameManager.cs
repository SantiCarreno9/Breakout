using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private PaddleController _paddle = default;
    [SerializeField]
    private BallController _ballController = default;

    [SerializeField]
    private PowerUpsManager _powerUpsManager = default;

    [SerializeField]
    private GameUIManager _gameUIManager = default;

    private int _score = 0;

    private int _lastConcedingPlayer = -1;

    private float _timeToShootBall = 3f;
    private int _currentLevel = 1;
    private int _initialLivesCount = 3;
    private int _currentLivesCount = 0;

    public BallController Ball => _ballController;
    public PaddleController Paddle => _paddle;
    public PowerUpsManager PowerUpsManager => _powerUpsManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ReshootBall();
        _currentLivesCount = _initialLivesCount;
        //Time.timeScale = 0;
    }

    #region GAMEPLAY

    public void StartGame()
    {
        SetUpNewGame();
        ReshootBall();
    }

    private void RestartPaddlePosition()
    {
        _paddle.GoToDefaultPosition();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private IEnumerator EndGame(bool playerWon)
    {
        yield return new WaitForSecondsRealtime(2);
        if (playerWon)
            _gameUIManager.ShowWinScreen();
        else _gameUIManager.ShowGameOverScreen();
        Time.timeScale = 0;
        SetUpNewGame();
    }

    private void SetUpNewGame()
    {
        _powerUpsManager.Reset();
        _paddle.DisablePowerUps();
        _ballController.MoveOutOfBounds();
        _ballController.Reset();
        _ballController.Hide();
        _score = 0;
        _currentLivesCount = _initialLivesCount;

        RestartPaddlePosition();
    }

    public void ReshootBall()
    {
        StartCoroutine(ShootBall());
        ResumeGame();
    }

    #endregion

    #region IN GAME ACTIONS

    public void Score(int points)
    {
        _score += points;
        _gameUIManager.UpdateScore(_score);
    }

    private IEnumerator ShootBall()
    {
        yield return new WaitForSeconds(2);
        _ballController.Reset();
        yield return new WaitForSeconds(_timeToShootBall);
        _ballController.Shoot();
    }

    public void ActivatePowerUp(PowerUps powerUp)
    {
        switch (powerUp)
        {
            case PowerUps.Grow:
                _paddle.GrowUp();
                break;
            case PowerUps.Laser:
                _paddle.EnableLaser();
                break;
            default:
                break;
        }        
    }

    public void FinishLevel()
    {
        _currentLevel++;
        if (_currentLevel > 3)
            StartCoroutine(EndGame(true));
    }

    public void LoseLife()
    {
        _currentLivesCount--;
        _gameUIManager.UpdateLives(_currentLivesCount);
        _paddle.DisablePowerUps();
        if (_currentLivesCount == 0)
            StartCoroutine(EndGame(false));
        else ReshootBall();
    }

    #endregion  
}
