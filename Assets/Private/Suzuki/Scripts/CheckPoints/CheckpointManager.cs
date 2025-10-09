using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckpointManager : MonoBehaviour
{
    [Header("設定")]
    public int totalLaps = 3;

    [Header("チェックポイント")]
    public List<Checkpoint> checkpoints = new List<Checkpoint>();

    private int nextCheckpointIndex = 0;
    private int currentLap = 0;
    private bool raceFinished = false;

    private RaceManager raceManager;

    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();

        checkpoints.Clear();
        checkpoints.AddRange(FindObjectsOfType<Checkpoint>());

        // Transform の階層順にソート（親の子順を優先）
        checkpoints = checkpoints
            .OrderBy(cp => cp.transform.GetSiblingIndex())
            .ToList();

        // checkpointID を自動で振る
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].checkpointID = i;
        }

        nextCheckpointIndex = 0;
        currentLap = 0;
        raceFinished = false;
    }
    //チェックポイント判定
    public void PassCheckpoint(GameObject player, Checkpoint cp)
    {
        if (raceFinished) return;
        if (raceManager != null && raceManager.currentState != RaceManager.RaceState.Racing) return;

        //逆走、ショートカット対策
        if (cp.checkpointID != nextCheckpointIndex)
        {
            Debug.Log($"[CheckpointManager] {player.name} がチェックポイント {cp.checkpointID} に到達したけど順番が違う。期待: {nextCheckpointIndex}");
            return;
        }

        //正規ルートの場合
        Debug.Log($"[CheckpointManager] チェックポイント {cp.checkpointID} 通過！");

        nextCheckpointIndex++;

        // ラップ完了
        if (nextCheckpointIndex >= checkpoints.Count)
        {
            currentLap++;
            nextCheckpointIndex = 0;

            Debug.Log($"[CheckpointManager] ラップ {currentLap}/{totalLaps} 完了！");

            // ゴール判定
            if (currentLap >= totalLaps)
            {
                raceFinished = true;
                OnRaceFinished(player);
            }
        }
    }

    //レース終了処理
    private void OnRaceFinished(GameObject player)
    {
        Debug.Log($"🏁 [CheckpointManager] {player.name} がゴールしました！！");

        if (raceManager != null)
        {
            raceManager.FinishRace(); // RaceManager に通知してレース終了
        }
    }
}
