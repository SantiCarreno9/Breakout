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
    private Level[] _levels = default;

    [SerializeField]
    private PowerUpsManager _powerUpsManager = default;

    [SerializeField]
    private GameUIManager _gameUIManager = default;

    private int _score = 0;

    private float _timeToShootBall = 2f;

    private int _initialLivesCount = 3;
    private int _currentLivesCount = 0;

    private int _currentLevel = 0;

    public PaddleController Paddle => _paddle;
    public PowerUpsManager PowerUpsManager => _powerUpsManager;
    public GameUIManager GameUIManager => _gameUIManager;

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

    void Start()
    {
        _currentLivesCount = _initialLivesCount;
        PauseGame();
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
    }

    private void SetUpNewGame()
    {
        _powerUpsManager.Reset();
        _powerUpsManager.Reset();
        _ballController.Reset();
        _score = 0;
        _currentLivesCount = _initialLivesCount;
        _gameUIManager.UpdateLives(_currentLivesCount);
        RestartPaddlePosition();
    }

    public void StartNewLevel()
    {
        _powerUpsManager.Reset();
        _ballController.Reset();
        RestartPaddlePosition();
        ReshootBall();
    }

    public void ReshootBall()
    {
        StartCoroutine(ShootBall());
        ResumeGame();
    }

    public void LoadLevel(int index)
    {
        for (int i = 0; i < _levels.Length; i++)
            _levels[i].gameObject.SetActive(i == index);

        _levels[index].Reset();
        _currentLevel = index;
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
        yield return new WaitForSeconds(1);
        _ballController.Reset();
        yield return new WaitForSeconds(_timeToShootBall);
        _ballController.Shoot();
    }

    public void FinishLevel()
    {
        _currentLevel++;
        if (_currentLevel > 2)
        {
            PauseGame();
            StartCoroutine(EndGame(true));
            return;
        }
        StartCoroutine(_gameUIManager.LoadNextLevel(_currentLevel));
    }

    public void LoseLife()
    {
        _currentLivesCount--;
        _gameUIManager.UpdateLives(_currentLivesCount);
        _gameUIManager.LoseLife();
        _powerUpsManager.Reset();
        if (_currentLivesCount == 0)
            StartCoroutine(EndGame(false));
        else ReshootBall();
    }

    #endregion  
}
