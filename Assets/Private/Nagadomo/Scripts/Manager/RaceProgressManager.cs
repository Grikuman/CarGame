using UnityEngine;

public class RaceProgressManager : MonoBehaviour
{
    private CourseProgressManager _courseProgressManager;
    private CheckpointManager _checkpointManager;

    // 現在のラップ数
    [SerializeField] private int _currentLap;
    // 現在のコース上のポイント
    [SerializeField] private int _currentCoursePoint;

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
        // 現在のコース上のポイントを取得
        _currentCoursePoint = _courseProgressManager.NearestWaypointIndex;

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
    /// 現在のコース上のポイントを取得する
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCoursePoint()
    {
        return _currentCoursePoint;
    }
}
