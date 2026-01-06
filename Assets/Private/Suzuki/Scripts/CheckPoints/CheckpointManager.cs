using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CheckpointManager : MonoBehaviour
{
    public int totalLaps = 3;
    // コース上のチェックポイント
    private List<Checkpoint> _checkpoints;
    // 現在通ったチェックポイント
    private int _currentCheckpoint;
    // 次に通らないといけないチェックポイント
    private int _nextCheckpointIndex;
    // 現在のラップ数
    private int _currentLap;
    // レース状態
    private bool _finished;

    private RaceManager raceManager;

    public int CurrentCheckpoint => _currentCheckpoint;
    public int NextCheckpoint => _nextCheckpointIndex;
    public int CurrentLap => _currentLap;
    public int TotalLaps => totalLaps;


    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();

        _checkpoints = FindObjectsOfType<Checkpoint>()
            .OrderBy(cp => cp.transform.GetSiblingIndex())
            .ToList();

        for (int i = 0; i < _checkpoints.Count; i++)
            _checkpoints[i].checkpointID = i;

        _currentCheckpoint = 0;
        _nextCheckpointIndex = 0;
        _currentLap = 0;
        _finished = false;
    }

    public void PassCheckpoint(Checkpoint cp)
    {
        if (_finished) return;

        if (raceManager.CurrentState != RaceManager.RaceState.Racing)
        {
            Debug.Log("レース中じゃない");
            return;
        }

        if (cp.checkpointID != _nextCheckpointIndex)
        {
            Debug.Log($"順番違い: {cp.checkpointID} / 目標 {_nextCheckpointIndex}");
            return;
        }

        Debug.Log($"Checkpoint {cp.checkpointID} 通過");

        // 現在通過したチェックポイント
        _currentCheckpoint = cp.checkpointID;
        // 次通るチェックポイント
        _nextCheckpointIndex++;

        if (_nextCheckpointIndex >= _checkpoints.Count)
        {
            _currentLap++;
            _nextCheckpointIndex = 0;

            FindFirstObjectByType<LapUI>()?
                .UpdateLap(_currentLap + 1, totalLaps);

            Debug.Log($"Lap {_currentLap}/{totalLaps}");

            if (_currentLap >= totalLaps)
            {
                _finished = true;
                raceManager.FinishRace();
            }
        }

    }

}
