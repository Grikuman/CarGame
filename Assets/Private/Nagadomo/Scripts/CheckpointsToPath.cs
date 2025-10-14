using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class CheckpointsToPath : MonoBehaviour
{
    [SerializeField] private CinemachinePath path;
    [SerializeField] private CheckpointRecording recording;

    private void Start()
    {
        ApplyCheckpoints();
    }

    private void OnValidate()
    {
        if (path != null && recording != null)
        {
            ApplyCheckpoints();
        }
    }

    private void ApplyCheckpoints()
    {
        if (path == null || recording == null || recording.data == null)
            return;

        int count = recording.data.Count;
        var waypoints = new Cinemachine.CinemachinePath.Waypoint[count]; // <- Š®‘S‚ÉPath—p

        for (int i = 0; i < count; i++)
        {
            var cp = recording.data[i];

            waypoints[i] = new Cinemachine.CinemachinePath.Waypoint
            {
                position = cp.position,
                roll = cp.rotation.eulerAngles.z,
                tangent = Vector3.zero
            };
        }

        path.m_Waypoints = waypoints;
        path.InvalidateDistanceCache();

        Debug.Log($"CinemachinePath updated with {count} checkpoints from {recording.name}");
    }
}
