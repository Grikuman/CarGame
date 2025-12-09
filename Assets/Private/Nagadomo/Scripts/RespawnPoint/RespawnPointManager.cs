using UnityEngine;
using System.Linq;

public class RespawnPointManager : MonoBehaviour
{
    public static RespawnPointManager Instance { get; private set; }

    private Transform[] _points;

    private void Awake()
    {
        // シングルトン
        Instance = this;

        // シーン中のRespawnPointを全て収集する
        _points = FindObjectsOfType<RespawnPoint>()
                    .Select(p => p.transform)
                    .ToArray();
    }

    /// <summary>
    /// 指定した位置から最も近いリスポーン地点を返す
    /// </summary>
    public Transform FindClosest(Vector3 fromPosition)
    {
        Transform closest = null;
        float minSqr = float.MaxValue;

        foreach (var point in _points)
        {
            float sqr = (point.position - fromPosition).sqrMagnitude;

            if (sqr < minSqr)
            {
                minSqr = sqr;
                closest = point;
            }
        }

        return closest;
    }
}
