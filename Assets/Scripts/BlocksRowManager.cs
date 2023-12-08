using System;
using UnityEngine;

public class BlocksRowManager : MonoBehaviour
{
    [SerializeField]
    private Color _color = Color.white;

    private Block[] _blocks;

    private int _destroyedBlocksCount = 0;

    public Action OnRowDestroyed = default;

    private int _difficulty = 1;

    public void SetDifficulty(int difficulty)
    {
        _difficulty = difficulty;
    }

    public void SetUp()
    {
        _blocks = GetComponentsInChildren<Block>();
        for (int i = 0; i < _blocks.Length; i++)
        {
            _blocks[i].SetColor(_color);
            _blocks[i].SetMaxHitCount(GetRandomMaxHitCount());
            _blocks[i].OnDestroyed += OnBlockDestroyed;
        }
    }

    private void OnBlockDestroyed(Block block)
    {
        _destroyedBlocksCount++;
        GameManager.Instance.Score(block.GetPoints());
        if (_destroyedBlocksCount < _blocks.Length)
        {
            GameManager.Instance.PowerUpsManager.TryToSpawnPowerUp(block.transform.position);
        }
        else OnRowDestroyed?.Invoke();
    }

    public void Reset()
    {
        for (int i = 0; i < _blocks.Length; i++)
        {
            _blocks[i].gameObject.SetActive(true);
            _blocks[i].Reset();
        }
    }

    private int GetRandomMaxHitCount()
    {
        int random = UnityEngine.Random.Range(0, 100);
        int[] probabilities = new int[4];
        switch (_difficulty)
        {
            case 1:
                probabilities[0] = 70;
                probabilities[1] = 90;
                probabilities[2] = 95;
                probabilities[3] = 100;
                break;
            case 2:
                probabilities[0] = 50;
                probabilities[1] = 80;
                probabilities[2] = 90;
                probabilities[3] = 100;
                break;
            case 3:
                probabilities[0] = 40;
                probabilities[1] = 60;
                probabilities[2] = 80;
                probabilities[3] = 100;
                break;
            default:
                break;
        }

        if (random < probabilities[0])
            random = 1;
        else if (random < probabilities[1])
            random = 2;
        else if (random < probabilities[2])
            random = 3;
        else if (random < probabilities[3])
            random = 4;

        return random;
    }

}
