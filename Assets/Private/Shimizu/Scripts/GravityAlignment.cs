using UnityEngine;

public class GravityAlignment
{
    private static Vector3 RECOVER_RIGHT_RAY_DIRECTION = Quaternion.Euler(0, 0, 135) * Vector3.up;
    private static Vector3 RECOVER_LEFT_RAY_DIRECTION  = Quaternion.Euler(0, 0, -135) * Vector3.up;


    // 地面を探す距離
    public float _rayLength { get; set; } = 20.0f;
    // コースに戻す力
    public float _recoverPower { get; set; } = 2.0f;
    // 地面レイヤー指定（必要に応じて）
    public LayerMask _layerMask { get; set; }


    // 地面に接しているか
    public bool _isGrounded { get; private set; } = false;
    // 地面の法線（初期値は上方向）
    public Vector3 _groundNormal { get;  set; } = Vector3.up;

    // 重力のアクティブ設定
    public bool _isGravity { get; set; } = true;


    private Rigidbody _rb        = null;
    private Transform _transform = null;
    private Vector3 _currentDirection = Vector3.zero;

    public GravityAlignment(Rigidbody rb)
    {
        _rb = rb;
        _transform = rb.transform;
    }

    // 地面を検知
    public void UpdateGravity()
    {
        RaycastHit hit;

        // 下方向にレイを飛ばす
        if (Physics.Raycast(_transform.position, -_transform.up, out hit, _rayLength, _layerMask))
        {
            _isGrounded = true;
            // 地面の向きを取得
            _groundNormal = -hit.normal;

            // 前フレームとの地面との向きを比較
            if(_currentDirection != _groundNormal)
            {
                // this.ResetVerticalVelocity(-_groundNormal);
            }

            if(_isGravity)
            _rb.linearVelocity += _groundNormal * 30.81f * Time.fixedDeltaTime;

            // 現在の地面の向きを保存
            _currentDirection = _groundNormal;
        }
        else
        {
            // コースに戻す処理
           this.RecoverOnTrack();
        }
    }


    private void ResetVerticalVelocity(Vector3 newGravityDirection)
    {
        // 現在の速度
        Vector3 velocity = _rb.linearVelocity;

        // 新しい重力方向（正規化）
        Vector3 gravityDir = newGravityDirection.normalized;

        // 重力方向の速度成分
        float verticalSpeed = Vector3.Dot(velocity, gravityDir);

        // 重力方向の速度だけを除去
        Vector3 adjustedVelocity = velocity - gravityDir * verticalSpeed;

        // 修正後の速度を代入
        _rb.linearVelocity = adjustedVelocity;

        Debug.Log("前の重力方向の速度を除去しました。");
    }

    private void RecoverOnTrack()
    {
        Debug.Log("リカバー中");

        RaycastHit hit;
        Vector3 direction = Vector3.down;

        // 左斜め下にレイを飛ばす
        if (Physics.Raycast(_transform.position, RECOVER_LEFT_RAY_DIRECTION, out hit, _rayLength))
        {
            Vector3 origin = _transform.position;
            // ヒット地点への方向ベクトル
            direction = (hit.point - origin).normalized;

            direction = new Vector3(direction.x * _recoverPower, direction.y , direction.z * _recoverPower);
        }
        else
        {
            // 右斜め下にレイを飛ばす
            if(Physics.Raycast(_transform.position, RECOVER_RIGHT_RAY_DIRECTION, out hit, _rayLength))
            {
                Vector3 origin = _transform.position;
                // ヒット地点への方向ベクトル
                direction = (hit.point - origin).normalized;

                direction = new Vector3(direction.x * _recoverPower, direction.y, direction.z * _recoverPower);
            }
        }

        _rb.linearVelocity += direction * 9.81f * Time.fixedDeltaTime;
    }
}
