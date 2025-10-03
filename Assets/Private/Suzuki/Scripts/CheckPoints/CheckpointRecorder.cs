using System.Collections.Generic;
using UnityEngine;

public class CheckpointRecorder : MonoBehaviour
{
    public GameObject checkpointPrefab; // Prefab
    public float interval = 5f;         // 秒ごとに記録

    [HideInInspector]
    public List<CheckpointData> recordedData = new List<CheckpointData>();

    private float timer = 0f;
    private bool isRecording = false;

    void Update()
    {
        if (!isRecording) return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer -= interval;
            recordedData.Add(new CheckpointData(transform.position, transform.rotation));
            Debug.Log($"[Recorder] #{recordedData.Count - 1} recorded at {transform.position}");
        }
    }

    public void StartRecording()
    {
        recordedData.Clear();
        timer = 0f;
        isRecording = true;
        Debug.Log("[Recorder] StartRecording");
    }

    public void StopRecording()
    {
        isRecording = false;
        Debug.Log($"[Recorder] StopRecording ({recordedData.Count} points)");
    }

    public void GenerateCheckpointsPlayMode()
    {
        if (checkpointPrefab == null || recordedData.Count == 0)
        {
            Debug.LogWarning("Prefab未設定または記録データなし");
            return;
        }

        GameObject parent = new GameObject("GeneratedCheckpoints_PlayPreview");

        for (int i = 0; i < recordedData.Count; i++)
        {
            var data = recordedData[i];
            GameObject cp = Instantiate(checkpointPrefab, data.position, data.rotation, parent.transform);
            cp.GetComponent<Checkpoint>().checkpointID = i;
        }

        Debug.Log($"[Recorder] Play中プレビュー生成 ({recordedData.Count}個)");
    }

    // Gizmosで位置と向きを可視化
    void OnDrawGizmosSelected()
    {
        if (recordedData == null || recordedData.Count == 0) return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < recordedData.Count; i++)
        {
            var data = recordedData[i];

            // 位置表示
            Gizmos.DrawSphere(data.position, 0.4f);

            // 前の点と線で繋ぐ
            if (i > 0)
                Gizmos.DrawLine(recordedData[i - 1].position, data.position);

            // 向きを矢印で表示
            Vector3 forward = data.rotation * Vector3.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(data.position, data.position + forward);
            Gizmos.color = Color.yellow;
        }
    }
}
