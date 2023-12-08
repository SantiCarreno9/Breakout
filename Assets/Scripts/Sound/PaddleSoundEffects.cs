using UnityEngine;

public class PaddleSoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;
    [SerializeField]
    private AudioClip _powerUpSound = default;
    [SerializeField]
    private AudioClip _shootSound = default;

    public void PlayPowerUpSound()
    {
        _audioSource.clip = _powerUpSound;
        _audioSource.Play();
    }

    public void PlayShootSound()
    {
        _audioSource.clip = _shootSound;
        _audioSource.Play();
    }    

}
