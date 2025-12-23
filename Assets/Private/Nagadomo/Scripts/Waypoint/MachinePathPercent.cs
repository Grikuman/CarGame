using UnityEngine;
using Cinemachine;

public class MachinePathPercent : MonoBehaviour
{
    [SerializeField] private CinemachinePathBase path;

    public float Percent { get; private set; }

    float _lastDistance;
    bool _initialized;

    void Start()
    {
        if (path == null && CoursePathManager.Instance != null)
            path = CoursePathManager.Instance.Path;

        if (path == null || path.PathLength <= 0f)
        {
            Debug.LogError("Path not found or invalid", this);
            return;
        }

        _lastDistance = 0f;
        _initialized = true;
    }

    void Update()
    {
        if (!_initialized) return;

        // まずは通常の最近点（ここはバージョン差が出にくい）
        float distance = path.FindClosestPoint(transform.position, 0, -1, 10);

        // ---- ループ補正（「前回に近い方の距離」を選ぶ）----
        float len = path.PathLength;

        // distance の候補を3つ作る（同一点を -len, +len したもの）
        float d0 = distance;
        float d1 = distance + len;
        float d2 = distance - len;

        // 前回距離に最も近い候補を採用
        distance = d0;
        if (Mathf.Abs(d1 - _lastDistance) < Mathf.Abs(distance - _lastDistance)) distance = d1;
        if (Mathf.Abs(d2 - _lastDistance) < Mathf.Abs(distance - _lastDistance)) distance = d2;

        // 0～len に正規化して保持
        distance = Mathf.Repeat(distance, len);

        _lastDistance = distance;

        Percent = (distance / len) * 100f;

        Debug.Log($"現在：{Percent:F2}%");
    }
}
