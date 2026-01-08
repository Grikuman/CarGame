using System.Collections.Generic;
using UnityEngine;

public class RaceProgressManager : MonoBehaviour
{
    private CourseProgressManager _courseProgressManager;
    private CheckpointManager _checkpointManager;

    // 現在のラップ数
    [SerializeField] private int _currentLap;
    // 最大ラップ数
    [SerializeField] private int _maxLap;
    // 現在のコース上のポイント
    [SerializeField] private int _currentCoursePoint;
    // 現在のチェックポイント
    [SerializeField] private int _currentCheckpoint;
    // 次のチェックポイント
    [SerializeField] private int _nextCheckpoint;

    void Start()
    {
        // コンポーネントの取得
        _courseProgressManager = GetComponent<CourseProgressManager>();
        _checkpointManager = GetComponent<CheckpointManager>();
    }

    void Update()
    {
        // どちらかが未取得なら処理しない
        if (_courseProgressManager == null || _checkpointManager == null)
            return;

        // 現在のラップを取得
        _currentLap = _checkpointManager.CurrentLap;
        // 最大ラップ数を取得
        _maxLap = _checkpointManager.TotalLaps;
        // 現在のコース上のポイントを取得
        _currentCoursePoint = _courseProgressManager.NearestWaypointIndex;
        // 現在のチェックポイントを取得
        _currentCheckpoint = _checkpointManager.CurrentCheckpoint;
        // 次のチェックポイント
        _nextCheckpoint = _checkpointManager.NextCheckpoint;

        Debug.Log($"現在のラップ数：{_currentLap}です。コース上のポイントは{_currentCoursePoint}です。");
    }

    /// <summary>
    /// 現在のラップ数を取得する
    /// </summary>
    /// <returns></returns>
    public int GetCurrentLap()
    {
        return _currentLap;
    }

    /// <summary>
    /// 最大ラップ数を取得する
    /// </summary>
    /// <returns></returns>
    public int GetMaxLap()
    {
        return _maxLap;
    }

    /// <summary>
    /// 現在のコース上のポイントを取得する
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCoursePoint()
    {
        return _currentCoursePoint;
    }

    /// <summary>
    /// 現在のチェックポイントを取得する
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCheckpoint()
    {
        return _currentCheckpoint;
    }

    /// <summary>
    /// 次のチェックポイントを取得する
    /// </summary>
    /// <returns></returns>
    public int GetNextCheckpoint()
    {
        return _nextCheckpoint;
    }
}
