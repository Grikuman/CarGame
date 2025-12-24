using UnityEngine;
using Cinemachine;

public class CourseProgressManager : MonoBehaviour,IVehicleReceiver
{
    // マシンのオブジェクト
    private GameObject _vehicle;
    // コースのパス
    private CinemachineSmoothPath _path;
    // 一番マシンと近いポイント番号
    public int NearestWaypointIndex { get; private set; } = -1;

    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        // マシンの取得
        _vehicle = vehicle;
    }

    private void Start()
    {
        // コースのパスを取得する
        _path = GetComponent<CinemachineSmoothPath>();
    }

    private void Update()
    {
        if (_path == null || _path.m_Waypoints == null || _path.m_Waypoints.Length == 0)
            return;

        if (_vehicle == null)
            return;

        float minSqrDistance = float.MaxValue;
        int nearestIndex = -1;

        for (int i = 0; i < _path.m_Waypoints.Length; i++)
        {
            // Waypoint はローカル座標なのでワールドに変換
            Vector3 wpWorldPos = _path.transform.TransformPoint(
                _path.m_Waypoints[i].position
            );

            float sqrDist = (_vehicle.transform.position - wpWorldPos).sqrMagnitude;

            if (sqrDist < minSqrDistance)
            {
                minSqrDistance = sqrDist;
                nearestIndex = i;
            }
        }

        NearestWaypointIndex = nearestIndex;
    }
}
