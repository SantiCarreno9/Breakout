using UnityEngine;

public class BallSoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;
    [SerializeField]
    private AudioClip _collisionSound = default;

    public void PlayCollisionSound()
    {
        _audioSource.clip = _collisionSound;
        _audioSource.Play();
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayCollisionSound();
    }    
}
