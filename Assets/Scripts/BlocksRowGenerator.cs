using System;
using UnityEngine;

public class BlocksRowGenerator : MonoBehaviour
{
    [SerializeField]
    private Block _blockPrefab = default;
    [SerializeField]
    private BoxCollider2D _boxCollider = default;
    [SerializeField]
    private Color _color = Color.white;

    private int _blocksCount = 0;
    private int _instantiatedBlocks = 0;

    public Action<BlocksRowGenerator> OnBlocksInstantiated = default;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBlocks();
    }

    private void GenerateBlocks()
    {
        float blockWidth = _blockPrefab.transform.localScale.x;
        float rowSize = _boxCollider.size.x;
        _blocksCount = (int)(rowSize / blockWidth);
        float initialPosition = ((rowSize / 2.0f) * -1) + (blockWidth / 2);
        float offset = 0;
        for (int i = 0; i < _blocksCount; i++)
        {
            float blockPosition = initialPosition + (blockWidth + offset) * i;
            Block block = Instantiate(_blockPrefab, transform);
            block.OnInstantiated += OnBlockInstantiated;            
            block.transform.localPosition = Vector3.right * blockPosition;
            block.SetColor(_color);
            block.SetMaxHitCount(GetRandomMaxHitCount());
        }
    }

    private void OnBlockInstantiated(Block block)
    {
        block.OnInstantiated -= OnBlockInstantiated;
        _instantiatedBlocks++;
        if (_instantiatedBlocks == _blocksCount)
            OnBlocksInstantiated?.Invoke(this);
    }

    private int GetRandomMaxHitCount()
    {
        int random = UnityEngine.Random.Range(0, 100);
        if (random < 60)
            random = 1;
        else if (random < 85)
            random = 2;
        else if (random < 100)
            random = 3;        

        return random;
    }

}
