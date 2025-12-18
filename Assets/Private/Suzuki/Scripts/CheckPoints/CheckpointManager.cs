using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CheckpointManager : MonoBehaviour
{
    public int totalLaps = 3;

    private List<Checkpoint> checkpoints;
    private int nextCheckpointIndex;
    private int currentLap;
    private bool finished;

    private RaceManager raceManager;

    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();

        checkpoints = FindObjectsOfType<Checkpoint>()
            .OrderBy(cp => cp.transform.GetSiblingIndex())
            .ToList();

        for (int i = 0; i < checkpoints.Count; i++)
            checkpoints[i].checkpointID = i;

        nextCheckpointIndex = 0;
        currentLap = 0;
        finished = false;
    }

    public void PassCheckpoint(Checkpoint cp)
    {
        if (finished) return;

        if (raceManager.CurrentState != RaceManager.RaceState.Racing)
        {
            Debug.Log("レース中じゃないので無視");
            return;
        }

        if (cp.checkpointID != nextCheckpointIndex)
        {
            Debug.Log($"順番違い: {cp.checkpointID} / 期待 {nextCheckpointIndex}");
            return;
        }

        Debug.Log($"Checkpoint {cp.checkpointID} 通過");

        nextCheckpointIndex++;

        if (nextCheckpointIndex >= checkpoints.Count)
        {
            currentLap++;
            nextCheckpointIndex = 0;
            Debug.Log($"Lap {currentLap}/{totalLaps}");

            if (currentLap >= totalLaps)
            {
                finished = true;
                raceManager.FinishRace();
            }
        }
    }

}
