using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = default;
    [SerializeField]
    private Sprite[] _states = default;

    public Action<Block> OnInstantiated;
    public Action<Block> OnDestroyed;

    private int _hitCount = 0;
    private int _maxHitCount = 0;

    private void Start()
    {
        OnInstantiated?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hitCount++;
        UpdateBlockState();
    }

    public void SetMaxHitCount(int count)
    {
        _maxHitCount = count;
        if (count > _states.Length)
            _maxHitCount = _states.Length;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    private void UpdateBlockState()
    {
        if (_hitCount == _maxHitCount)
        {
            OnDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }
        else
        {
            int spriteIndex = _hitCount + (_states.Length - _maxHitCount);
            _spriteRenderer.sprite = _states[spriteIndex];
        }
    }

    public void Reset()
    {
        _spriteRenderer.sprite = _states[0];
    }

    public int GetPoints()
    {
        return _hitCount;
    }
}
