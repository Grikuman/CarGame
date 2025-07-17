using UnityEngine;
using System.Collections.Generic;

public class CarMove : MonoBehaviour
{
    // 車軸情報（前輪・後輪の設定など）をまとめたリスト
    public List<AxleInfo> axleInfos;

    // モーターによる最大トルク（前進・後退の強さ）
    public float maxMotorTorque = 1500f;

    // 最大操舵角（ハンドルをどれだけ切れるか）
    public float maxSteeringAngle = 30f;

    // ドリフト時のリアグリップ（0に近いほど滑りやすい）
    public float rearGrip = 0.5f;

    // ドリフト操作にShiftキーを使うか
    public bool useShiftForDrift = true;

    void Start()
    {
        // 車の重心を少し下げて、横転しにくくする
        GetComponent<Rigidbody>().centerOfMass += Vector3.down * 0.5f;
    }

    void FixedUpdate()
    {
        // 前後移動の入力（W/Sキーなど）に応じてトルクを計算
        float motor = maxMotorTorque * Input.GetAxis("Vertical");

        // 左右移動の入力（A/Dキーなど）に応じて操舵角を計算
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        // Shiftキーが押されていればドリフト状態とする（1.0）、押されていなければ通常（0.0）
        float driftInput = (useShiftForDrift && Input.GetKey(KeyCode.LeftShift)) ? 1f : 0f;

        // ドリフト入力に応じてリアグリップの補正値を計算（1→通常グリップ、rearGrip→ドリフト）
        float rearGripFactor = Mathf.Lerp(1f, rearGrip, driftInput);

        // 各アクスル（車軸）ごとに処理
        foreach (AxleInfo axleInfo in axleInfos)
        {
            // ハンドルが有効な車軸なら操舵角を設定
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            // 駆動が有効な車軸ならトルクを設定
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            // 後輪なら、ドリフト用に横グリップを調整
            if (axleInfo.isRear)
            {
                SetRearGrip(axleInfo, rearGripFactor);
            }
        }
    }

    // リアタイヤの横グリップ（stiffness）を設定
    void SetRearGrip(AxleInfo axleInfo, float gripFactor)
    {
        WheelFrictionCurve friction;

        // 左後輪の横グリップを調整
        friction = axleInfo.leftWheel.sidewaysFriction;
        friction.stiffness = gripFactor;
        axleInfo.leftWheel.sidewaysFriction = friction;

        // 右後輪の横グリップを調整
        friction = axleInfo.rightWheel.sidewaysFriction;
        friction.stiffness = gripFactor;
        axleInfo.rightWheel.sidewaysFriction = friction;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;   // 左側のホイール
    public WheelCollider rightWheel;  // 右側のホイール
    public bool motor;                // 駆動力を持つ車軸かどうか
    public bool steering;             // ハンドル操作が可能な車軸かどうか
    public bool isRear;               // 後輪かどうか（グリップ調整に使用）
}
