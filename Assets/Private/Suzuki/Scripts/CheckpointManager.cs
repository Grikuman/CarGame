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
            Debug.Log($"[CheckpointManager] {player.name} ���`�F�b�N�|�C���g {cp.checkpointID} �ɓ��B�������Ǐ��Ԃ��Ⴄ�B����: {nextCheckpoint}");
            return;
        }

        Debug.Log($"[CheckpointManager] {player.name} ���`�F�b�N�|�C���g {cp.checkpointID} �ʉ� (Lap {currentLap + 1})");

        nextCheckpoint++;

        // ���ׂẴ`�F�b�N�|�C���g��ʉ߂����ꍇ
        if (nextCheckpoint >= checkpoints.Count)
        {
            nextCheckpoint = 0;
            currentLap++;

            // �`�F�b�N�|�C���g�̒ʉ߃t���O�����Z�b�g�i�����b�v�p�j
            foreach (var checkpoint in checkpoints)
                checkpoint.passed = false;

            Debug.Log($"[CheckpointManager] {player.name} �������b�v��: {currentLap}/{totalLaps}");

            if (currentLap >= totalLaps)
            {
                Debug.Log($"[CheckpointManager] {player.name} �S�[���I");
                // �S�[�����̏����������ɒǉ��\
            }
        }
    }
}
