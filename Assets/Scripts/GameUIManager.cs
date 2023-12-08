using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum GameScreen
{
    MainMenu,
    LoadLevel,
    InGame,
    Pause,
    Win,
    GameOver
}

public class GameUIManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField]
    private GameObject _mainMenuScreen = default;
    [SerializeField]
    private GameObject _loadLevelScreen = default;
    [Space]
    [SerializeField]
    private GameObject _inGameScreen = default;
    [SerializeField]
    private GameObject _pauseScreen = default;

    [Header("End Screen")]
    [SerializeField]
    private GameObject _winScreen = default;
    [SerializeField]
    private GameObject _gameOverScreen = default;

    [Space]
    [Header("In Game Components")]
    [SerializeField]
    private TMP_Text _scoreText = default;
    [SerializeField]
    private TMP_Text _livesText = default;
    [SerializeField]
    private GameObject _laserUI = default;
    [SerializeField]
    private TMP_Text _laserCountText = default;

    [Space]
    [SerializeField]
    private GameSoundEffects _soundEffects = default;

    private bool _isGamePaused = false;

    private void Start()
    {
        ShowScreen(GameScreen.MainMenu);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_isGamePaused) PauseGame();
            else ResumeGame();
        }
    }

    #region SCREEN

    private void ShowScreen(GameScreen screen)
    {
        _mainMenuScreen.SetActive(screen == GameScreen.MainMenu);
        _loadLevelScreen.SetActive(screen == GameScreen.LoadLevel);

        _inGameScreen.SetActive(screen == GameScreen.InGame);

        _pauseScreen.SetActive(screen == GameScreen.Pause);
        _winScreen.SetActive(screen == GameScreen.Win);
        _gameOverScreen.SetActive(screen == GameScreen.GameOver);
    }

    public void ShowWinScreen()
    {
        _soundEffects.PlayWinSound();
        ShowScreen(GameScreen.Win);
        ResetScore();
        StartCoroutine(GoToMainMenuCoroutine());
    }

    public void ShowGameOverScreen()
    {
        _soundEffects.PlayGameOverSound();
        ShowScreen(GameScreen.GameOver);
        ResetScore();
        StartCoroutine(GoToMainMenuCoroutine());
    }

    private IEnumerator GoToMainMenuCoroutine()
    {
        yield return new WaitForSecondsRealtime(3);
        GoToMainMenu();
    }

    #endregion

    #region MENU

    private void StartGame()
    {
        ResetScore();
        ShowScreen(GameScreen.InGame);
        GameManager.Instance.StartGame();
        _isGamePaused = false;
    }

    public void ResumeGame()
    {
        ShowScreen(GameScreen.InGame);
        GameManager.Instance.ResumeGame();
        _isGamePaused = false;
    }

    public IEnumerator LoadNextLevel(int index)
    {
        GameManager.Instance.PauseGame();
        yield return new WaitForSecondsRealtime(2);
        GameManager.Instance.LoadLevel(index);
        GameManager.Instance.ResumeGame();
        GameManager.Instance.StartNewLevel();
    }

    public void LoadLevel(int index)
    {
        GameManager.Instance.LoadLevel(index);
        StartGame();
    }

    public void PauseGame()
    {
        ShowScreen(GameScreen.Pause);
        GameManager.Instance.PauseGame();
        _isGamePaused = true;
    }

    public void GoToMainMenu()
    {
        ShowScreen(GameScreen.MainMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion    

    public void UpdateLives(int lives)
    {
        _livesText.text = lives.ToString();        
    }

    public void LoseLife()
    {
        _soundEffects.PlayLoseSound();
    }

    public void EnableLaserUI() => _laserUI.SetActive(true);
    public void DisableLaserUI() => _laserUI.SetActive(false);

    public void UpdateLaserCount(int laserCount)
    {
        _laserCountText.text = "x" + laserCount;
    }

    #region SCORE

    public void UpdateScore(int score)
    {
        string scoreText;
        if (score < 10)
            scoreText = "000" + score;
        else if (score < 100)
            scoreText = "00" + score;
        else if (score < 1000)
            scoreText = "0" + score;
        else scoreText = score.ToString();

        _scoreText.text = scoreText;
        _soundEffects.PlayScoreSound();
    }

    public void ResetScore()
    {
        _scoreText.text = "0000";
    }

    #endregion


}
