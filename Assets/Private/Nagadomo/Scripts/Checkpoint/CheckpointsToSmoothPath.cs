using UnityEngine;
using Cinemachine;

[ExecuteAlways] // �G�f�B�^��ł����f�������ꍇ
public class CheckpointsToSmoothPath : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath smoothPath;
    [SerializeField] private CheckpointRecording recording;

    private void Start()
    {
        ApplyCheckpoints();
    }

    private void OnValidate()
    {
        if (smoothPath != null && recording != null)
        {
            ApplyCheckpoints();
        }
    }

    private void ApplyCheckpoints()
    {
        if (smoothPath == null || recording == null || recording.data == null)
            return;

        int count = recording.data.Count;
        var waypoints = new CinemachineSmoothPath.Waypoint[count];

        for (int i = 0; i < count; i++)
        {
            var cp = recording.data[i];

            waypoints[i] = new CinemachineSmoothPath.Waypoint
            {
                position = cp.position,
                roll = cp.rotation.eulerAngles.z,   // Z���̌X������ roll �ɔ��f
            };
        }

        smoothPath.m_Waypoints = waypoints;
        smoothPath.InvalidateDistanceCache();

        Debug.Log($"CinemachineSmoothPath updated with {count} checkpoints from {recording.name}");
    }
}
