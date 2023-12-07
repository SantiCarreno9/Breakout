using UnityEngine;

public class GameSoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;

    [Header("UI")]
    [SerializeField]
    private AudioClip _buttonSound = default;

    [Space]
    [Header("Game sound")]
    [SerializeField]
    private AudioClip _scoreSound = default;
    [SerializeField]
    private AudioClip _winSound = default;
    [SerializeField]
    private AudioClip _loseSound = default;
    [SerializeField]
    private AudioClip _gameOverSound = default;

    public void PlayButtonSound()
    {
        _audioSource.clip = _buttonSound;
        _audioSource.Play();
    }

    public void PlayScoreSound()
    {
        _audioSource.clip = _scoreSound;
        _audioSource.Play();
    }

    public void PlayWinSound()
    {
        _audioSource.clip = _winSound;
        _audioSource.Play();
    }

    public void PlayLoseSound()
    {
        _audioSource.clip = _loseSound;
        _audioSource.Play();
    }

    public void PlayGameOverSound()
    {
        _audioSource.clip = _gameOverSound;
        _audioSource.Play();
    }
}
