using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    private BlocksRowGenerator[] _rows = default;
    private Block[] _blocks = default;

    private int _destroyedBlocksCount = 0;
    private int _rowsInstantiated = 0;

    private void Awake()
    {
        _rows = GetComponentsInChildren<BlocksRowGenerator>();
        SubscribeToBlocksGenerators();
    }

    private void SubscribeToBlocksGenerators()
    {
        for (int i = 0; i < _rows.Length; i++)
        {
            _rows[i].OnBlocksInstantiated += OnRowInstantiated;
        }
    }

    private void OnRowInstantiated(BlocksRowGenerator blocksRowGenerator)
    {
        blocksRowGenerator.OnBlocksInstantiated -= OnRowInstantiated;
        _rowsInstantiated++;
        if (_rowsInstantiated == _rows.Length)
        {
            _blocks = GetComponentsInChildren<Block>();
            SubscribeToBlocksEvent();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToBlocksEvent();
    }

    private void SubscribeToBlocksEvent()
    {
        for (int i = 0; i < _blocks.Length; i++)
            _blocks[i].OnDestroyed += OnBlockDestroyed;
    }

    private void UnsubscribeToBlocksEvent()
    {
        for (int i = 0; i < _blocks.Length; i++)
            _blocks[i].OnDestroyed -= OnBlockDestroyed;
    }

    private void OnBlockDestroyed(Block block)
    {
        _destroyedBlocksCount++;
        GameManager.Instance.Score(block.GetPoints());
        if (_destroyedBlocksCount < _blocks.Length)
            GameManager.Instance.PowerUpsManager.TryToSpawnPowerUp(block.transform.position);
        else GameManager.Instance.FinishLevel();
    }

    public void ShowAllBlocks()
    {
        for (int i = 0; i < _blocks.Length; i++)
            _blocks[i].gameObject.SetActive(true);
    }
}
