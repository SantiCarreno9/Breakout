using TMPro;
using UnityEngine;

public enum GameScreen
{
    None,
    MainMenu,
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
    [Space]
    [SerializeField]
    private GameObject _inGameScreen = default;
    [SerializeField]
    private GameObject _pauseScreen = default;

    [Header("End Screen")]
    [SerializeField]
    private GameObject _endScreen = default;
    [SerializeField]
    private TMP_Text _endScreenText = default;

    [Space]
    [Header("In Game Components")]
    [SerializeField]
    private TMP_Text _scoreText = default;
    [SerializeField]
    private TMP_Text _livesText = default;

    [Space]
    [SerializeField]
    private GameSoundEffects _soundEffects = default;

    private bool _isGamePaused = false;

    private void Start()
    {
        //UpdateAmountOfPlayers(_playersDropdown.value);
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

        _inGameScreen.SetActive(screen == GameScreen.InGame);

        _pauseScreen.SetActive(screen == GameScreen.Pause);
        _endScreen.SetActive(screen == GameScreen.Win);
    }

    public void ShowWinScreen()
    {
        _soundEffects.PlayWinSound();
        ShowScreen(GameScreen.Win);
        ResetScore();
        _endScreenText.text = "Congratulations! You've finished the game!";
        Invoke("GoToMainMenu", 3);
        GoToMainMenu();
    }

    public void ShowGameOverScreen()
    {
        _soundEffects.PlayLoseSound();
        ShowScreen(GameScreen.GameOver);
        ResetScore();
        Invoke("GoToMainMenu", 3);
    }

    #endregion

    #region MENU

    //public void UpdateAmountOfPlayers(int amountOfPlayers)
    //{
    //    amountOfPlayers += 2;
    //    for (int i = 0; i < _playerSelectionItems.Length; i++)
    //        _playerSelectionItems[i].SetActive(i < amountOfPlayers);

    //    for (int i = 0; i < _scoreTexts.Length; i++)
    //        _scoreTexts[i].gameObject.SetActive(i < amountOfPlayers);

    //    //GameManager.Instance.UpdateAmountOfPlayers(amountOfPlayers);
    //}

    public void StartGame()
    {
        ResumeGame();
        ResetScore();
        GameManager.Instance.StartGame();
    }

    public void ResumeGame()
    {
        ShowScreen(GameScreen.InGame);
        GameManager.Instance.ResumeGame();
        _isGamePaused = false;
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

    public void ReshootBall()
    {
        ResumeGame();
        GameManager.Instance.ReshootBall();
    }

    public void RestartGame()
    {
        ShowScreen(GameScreen.InGame);
        ResetScore();
        GameManager.Instance.StartGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    public void UpdateLives(int lives)
    {
        _livesText.text = lives.ToString();
        _soundEffects.PlayScoreSound();
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
