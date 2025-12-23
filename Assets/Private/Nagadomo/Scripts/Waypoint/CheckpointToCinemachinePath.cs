using UnityEngine;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CheckpointToCinemachinePath : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private CheckpointRecording checkpointRecording;

    [Header("Target")]
    [SerializeField] private CinemachineSmoothPath smoothPath;

    public void ApplyCheckpointsToPath()
    {
        if (checkpointRecording == null || smoothPath == null)
        {
            Debug.LogWarning("CheckpointRecording or SmoothPath is missing");
            return;
        }

        var checkpoints = checkpointRecording.data;
        if (checkpoints == null || checkpoints.Count < 2)
        {
            Debug.LogWarning("Not enough checkpoints");
            return;
        }

#if UNITY_EDITOR
        Undo.RecordObject(smoothPath, "Apply Checkpoints To Cinemachine Path");
#endif

        var waypoints = new CinemachineSmoothPath.Waypoint[checkpoints.Count];

        for (int i = 0; i < checkpoints.Count; i++)
        {
            waypoints[i] = new CinemachineSmoothPath.Waypoint
            {
                position = checkpoints[i].position,
                roll = 0f
            };
        }

        smoothPath.m_Waypoints = waypoints;
        smoothPath.InvalidateDistanceCache();

#if UNITY_EDITOR
        EditorUtility.SetDirty(smoothPath);
#endif
    }
}
