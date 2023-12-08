using System;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    [SerializeField]
    private Laser _laserPrefab = default;
    [SerializeField]
    private Transform _laserContainer = default;

    private List<Laser> _shotLaserList = new List<Laser>();
    private Queue<Laser> _laserPool = new Queue<Laser>();

    public Action OnLaserShot = default;

    private int _defaultMaxLaserShot = 10;
    private int _remainingLaserCount = 0;

    private bool _enabled = false;

    public void EnableLaser()
    {
        _enabled = true;
        _remainingLaserCount += _defaultMaxLaserShot;
        GameManager.Instance.GameUIManager.EnableLaserUI();
        GameManager.Instance.GameUIManager.UpdateLaserCount(_remainingLaserCount);
    }

    public void DisableLaser()
    {
        _enabled = false;
        _remainingLaserCount = 0;
        GameManager.Instance.GameUIManager.DisableLaserUI();
    }

    private Laser InstantiateLaser()
    {
        Laser item;
        if (_laserPool.Count == 0)
        {
            item = Instantiate(_laserPrefab, _laserContainer);
            item.SetManager(this);
        }
        else item = _laserPool.Dequeue();

        item.gameObject.SetActive(true);

        return item;
    }

    public void Shoot(Vector3 initialPosition)
    {
        if (!_enabled)
            return;

        Laser item = InstantiateLaser();
        item.transform.position = initialPosition;
        _remainingLaserCount--;
        OnLaserShot?.Invoke();
        GameManager.Instance.GameUIManager.UpdateLaserCount(_remainingLaserCount);        
        
        if (!_shotLaserList.Contains(item))
            _shotLaserList.Add(item);
        
        if (_remainingLaserCount == 0)
            DisableLaser();
    }

    public void HideAll()
    {
        for (int i = 0; i < _shotLaserList.Count; i++)
            if (_shotLaserList[i].gameObject.activeSelf)
                EnqueueLaser(_shotLaserList[i]);
    }

    public void EnqueueLaser(Laser laser)
    {
        laser.gameObject.SetActive(false);
        if (_laserPool.Contains(laser))
            return;
        _laserPool.Enqueue(laser);
    }

    public void Reset()
    {
        HideAll();
    }
}
