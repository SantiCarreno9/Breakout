using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private int _difficultyLevel = 1;

    private BlocksRowManager[] _rows = default;

    private int _destroyedRowsCount = 0;

    private void Start()
    {
        _rows = GetComponentsInChildren<BlocksRowManager>();
        SubscribeToBlocksGenerators();
    }

    private void SubscribeToBlocksGenerators()
    {
        for (int i = 0; i < _rows.Length; i++)
        {
            _rows[i].OnRowDestroyed += OnRowDestroyed;
            _rows[i].SetDifficulty(_difficultyLevel);
            _rows[i].SetUp();
        }
    }

    private void OnRowDestroyed()
    {
        _destroyedRowsCount++;
        if (_destroyedRowsCount == _rows.Length)
        {
            _destroyedRowsCount = 0;
            GameManager.Instance.FinishLevel();
        }        
    }

    public void Reset()
    {
        for (int i = 0; i < _rows.Length; i++)
            _rows[i].Reset();
        _destroyedRowsCount = 0;
    }
}
