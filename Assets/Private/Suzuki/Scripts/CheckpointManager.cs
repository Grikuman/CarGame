using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public int totalLaps = 3;

    private int nextCheckpoint = 0;
    private int currentLap = 0;

    void Start()
    {
        if (checkpoints.Count == 0)
        {
            checkpoints = new List<Checkpoint>(FindObjectsOfType<Checkpoint>());
            checkpoints.Sort((a, b) => a.checkpointID.CompareTo(b.checkpointID));
        }
    }

    public void PassCheckpoint(GameObject player, Checkpoint cp)
    {
        if (cp != checkpoints[nextCheckpoint])
        {
            Debug.Log($"[CheckpointManager] {player.name} がチェックポイント {cp.checkpointID} に到達したけど順番が違う。期待: {nextCheckpoint}");
            return;
        }

        Debug.Log($"[CheckpointManager] {player.name} がチェックポイント {cp.checkpointID} 通過 (Lap {currentLap + 1})");

        nextCheckpoint++;

        // すべてのチェックポイントを通過した場合
        if (nextCheckpoint >= checkpoints.Count)
        {
            nextCheckpoint = 0;
            currentLap++;

            // チェックポイントの通過フラグをリセット（次ラップ用）
            foreach (var checkpoint in checkpoints)
                checkpoint.passed = false;

            Debug.Log($"[CheckpointManager] {player.name} 完了ラップ数: {currentLap}/{totalLaps}");

            if (currentLap >= totalLaps)
            {
                Debug.Log($"[CheckpointManager] {player.name} ゴール！");
                // ゴール時の処理をここに追加可能
            }
        }
    }
}
