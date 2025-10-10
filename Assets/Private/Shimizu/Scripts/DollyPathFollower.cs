using Cinemachine;
using UnityEngine;

public class DollyPathFollower : MonoBehaviour
{
   

    [SerializeField] private Transform _Target;
    [SerializeField] private float _offset;

    [SerializeField] private int _stepsPerSegment;

    private CinemachineDollyCart _cinemachineDollyCart = null;
    public int _startSegment;
    public int _endSegment;


    private void Start()
    {
        _cinemachineDollyCart = this.GetComponent<CinemachineDollyCart>();
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = _Target.position;

        Vector3 direction = -_Target.forward.normalized;

        targetPosition = targetPosition + (direction * _offset);

        // 現在のカメラの位置を取得
        int currentT = Mathf.FloorToInt(_cinemachineDollyCart.m_Position);

        // 開始ポイント番号
        _startSegment = currentT - 3;
        // 終了ポイント番号
        _endSegment = currentT + 3;

        // 距離ベースで最も近い位置の t を取得
        float t = _cinemachineDollyCart.m_Path.FindClosestPoint(targetPosition, _startSegment, _endSegment, _stepsPerSegment);

        _cinemachineDollyCart.m_Position = t;
    }

    
}
