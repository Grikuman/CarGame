using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;

namespace KartGame.KartSystems
{
    /// <summary>
    /// アーケードスタイルのカート制御クラス
    /// カートの物理、ドリフト、VFXを管理する
    /// </summary>
    public class ArcadeKart : MonoBehaviour
    {
        // パワーアップによる一時的なステータス変更を管理するクラス
        [System.Serializable]
        public class StatPowerup
        {
            public ArcadeKart.Stats modifiers;  // ステータス修正値
            public string PowerUpID;            // パワーアップのID
            public float ElapsedTime;           // 経過時間
            public float MaxTime;               // 最大持続時間
        }

        // カートのステータスを定義する構造体
        // 全ての物理パラメータを含む
        [System.Serializable]
        public struct Stats
        {
            [Header("カートの動作設定")]
            // 前進時の最高速度
            [Min(0.001f)] public float TopSpeed;
            // 前進加速度
            public float Acceleration;
            [Min(0.001f)]
            // 後進時の最高速度
            public float ReverseSpeed;        
            // 後進加速度
            public float ReverseAcceleration;   
            // 加速カーブ（停止時からの加速の速さ）
            [Range(0.2f, 1)] public float AccelerationCurve;
            // ブレーキ力
            public float Braking;               
            // 慣性による減速
            public float CoastingDrag;          
            // グリップ力（横滑り抵抗）
            [Range(0.0f, 1.0f)] public float Grip;                  
            // ステアリング感度
            public float Steer;                 
            // 空中時の追加重力
            public float AddedGravity;          

            // パワーアップ用のステータス加算オペレーター
            // 全てのステータス値を加算する
            public static Stats operator +(Stats a, Stats b)
            {
                return new Stats
                {
                    Acceleration = a.Acceleration + b.Acceleration,
                    AccelerationCurve = a.AccelerationCurve + b.AccelerationCurve,
                    Braking = a.Braking + b.Braking,
                    CoastingDrag = a.CoastingDrag + b.CoastingDrag,
                    AddedGravity = a.AddedGravity + b.AddedGravity,
                    Grip = a.Grip + b.Grip,
                    ReverseAcceleration = a.ReverseAcceleration + b.ReverseAcceleration,
                    ReverseSpeed = a.ReverseSpeed + b.ReverseSpeed,
                    TopSpeed = a.TopSpeed + b.TopSpeed,
                    Steer = a.Steer + b.Steer,
                };
            }
        }

        // パブリックプロパティ：外部からの読み取り専用アクセス
        public Rigidbody Rigidbody { get; private set; }    // 物理計算用リジッドボディ
        public InputData Input { get; private set; }    // 現在の入力データ
        public float AirPercent { get; private set; }    // 空中にいる割合（0-1）
        public float GroundPercent { get; private set; }    // 地面に接している割合（0-1）

        // カートの基本ステータス（パワーアップなしの状態）
        public ArcadeKart.Stats baseStats = new ArcadeKart.Stats
        {
            TopSpeed = 10f,           // 最高速度
            Acceleration = 5f,        // 加速度
            AccelerationCurve = 4f,   // 加速カーブ
            Braking = 10f,            // ブレーキ力
            ReverseAcceleration = 5f, // 後進加速度
            ReverseSpeed = 5f,        // 後進最高速度
            Steer = 5f,               // ステアリング感度
            CoastingDrag = 4f,        // 慣性減速
            Grip = .95f,              // グリップ力
            AddedGravity = 1f,        // 追加重力
        };

        [Header("視覚オブジェクト")]
        public List<GameObject> m_VisualWheels; // 視覚的なホイールオブジェクト

        [Header("カートの物理設定")]
        // 重心位置
        public Transform CenterOfMass;                      
        // 空中での姿勢制御係数
        [Range(0.0f, 20.0f)] public float AirborneReorientationCoefficient = 3.0f;

        [Header("ドリフト設定")]
        // ドリフト時のグリップ力
        [Range(0.01f, 1.0f)] public float DriftGrip = 0.4f;                      
        // ドリフト時の追加ステアリング
        [Range(0.0f, 10.0f)] public float DriftAdditionalSteer = 5.0f;           
        // ドリフト終了のための最小角度
        [Range(1.0f, 30.0f)] public float MinAngleToFinishDrift = 10.0f;         
        // ドリフト終了のための最小速度割合
        [Range(0.01f, 0.99f)] public float MinSpeedPercentToFinishDrift = 0.5f;   
        // ドリフト制御感度
        [Range(1.0f, 20.0f)]public float DriftControl = 10.0f;                  
        // ドリフトの減衰率
        [Range(0.0f, 20.0f)] public float DriftDampening = 10.0f;                

        [Header("サスペンションの設定")]
        // サスペンションの最大伸長
        [Range(0.0f, 1.0f)] public float SuspensionHeight = 0.2f;
        // サスペンションのばね定数
        [Range(10.0f, 100000.0f)] public float SuspensionSpring = 20000.0f;
        // サスペンションの減衰係数
        [Range(0.0f, 5000.0f)] public float SuspensionDamp = 500.0f;
        // ホイール位置の垂直オフセット
        [Range(-1.0f, 1.0f)] public float WheelsPositionVerticalOffset = 0.0f;   

        [Header("ホイールコライダーの設定")]
        public WheelCollider FrontLeftWheel;  // 前左ホイールコライダー
        public WheelCollider FrontRightWheel; // 前右ホイールコライダー
        public WheelCollider RearLeftWheel;   // 後左ホイールコライダー
        public WheelCollider RearRightWheel;  // 後右ホイールコライダー
        // ホイールが検出するレイヤー
        public LayerMask GroundLayers = Physics.DefaultRaycastLayers;

        // プライベート変数--------------------------------------------------------------------------------------------

        // 入力関連
        IInput[] m_Inputs; // 入力ソースの配列
        // 定数
        const float k_NullInput = 0.01f;          // 入力の閾値（この値以下は0とみなす）
        const float k_NullSpeed = 0.01f;          // 速度の閾値（この値以下は停止とみなす）
        Vector3 m_VerticalReference = Vector3.up; // 垂直方向の基準ベクトル

        // ドリフト関連のプロパティとメンバ変数
        public bool WantsToDrift { get; private set; } = false;    // ドリフトしたいかどうか
        public bool IsDrifting { get; private set; } = false;      // 現在ドリフト中かどうか
        float m_CurrentGrip = 1.0f;                        // 現在のグリップ値
        float m_DriftTurningPower = 0.0f;                  // ドリフト時のターニングパワー
        float m_PreviousGroundPercent = 1.0f;              // 前フレームの地面接触割合

        // ドリフトエフェクトのインスタンスを管理するリスト
        readonly List<(GameObject trailRoot, WheelCollider wheel, TrailRenderer trail)> m_DriftTrailInstances = new List<(GameObject, WheelCollider, TrailRenderer)>();
        readonly List<(WheelCollider wheel, float horizontalOffset, float rotation, ParticleSystem sparks)> m_DriftSparkInstances = new List<(WheelCollider, float, float, ParticleSystem)>();

        // 移動制御とパワーアップ管理
        bool m_CanMove = true; // カートが移動可能かどうか
        List<StatPowerup> m_ActivePowerupList = new List<StatPowerup>();  // アクティブなパワーアップリスト
        ArcadeKart.Stats m_FinalStats; // 最終的なステータス（基本+パワーアップ）

        // 位置と回転の記録
        Quaternion m_LastValidRotation; // 最後の有効な回転
        Vector3 m_LastValidPosition;    // 最後の有効な位置
        Vector3 m_LastCollisionNormal;  // 最後の衝突法線
        bool m_HasCollision;            // 衝突中かどうか
        bool m_InAir = false;           // 空中にいるかどうか

        // ------------------------------------------------------------------------------------------------------------

        // パワーアップを追加する
        public void AddPowerup(StatPowerup statPowerup) => m_ActivePowerupList.Add(statPowerup);

        // カートの移動可能状態を設定する
        public void SetCanMove(bool move) => m_CanMove = move;

        // 現在の最高速度を取得する（前進・後進の大きい方）
        public float GetMaxSpeed() => Mathf.Max(m_FinalStats.TopSpeed, m_FinalStats.ReverseSpeed);

        // ホイールのサスペンションパラメータを更新する
        void UpdateSuspensionParams(WheelCollider wheel)
        {
            wheel.suspensionDistance = SuspensionHeight;    // サスペンションの距離
            wheel.center = new Vector3(0.0f, WheelsPositionVerticalOffset, 0.0f);  // ホイールの中心位置

            // サスペンションのばねと減衰の設定
            JointSpring spring = wheel.suspensionSpring;
            spring.spring = SuspensionSpring;
            spring.damper = SuspensionDamp;
            wheel.suspensionSpring = spring;
        }

        // 初期化処理
        void Awake()
        {
            // 必要なコンポーネントを取得
            Rigidbody = GetComponent<Rigidbody>();
            m_Inputs = GetComponents<IInput>();

            // 全ホイールのサスペンションパラメータを初期化
            UpdateSuspensionParams(FrontLeftWheel);
            UpdateSuspensionParams(FrontRightWheel);
            UpdateSuspensionParams(RearLeftWheel);
            UpdateSuspensionParams(RearRightWheel);

            // 初期グリップ値を設定
            m_CurrentGrip = baseStats.Grip;
        }

        // 物理更新処理（固定フレームレートで実行）
        // カートの全ての物理計算を行う
        void FixedUpdate()
        {
            // サスペンションパラメータを毎フレーム更新
            UpdateSuspensionParams(FrontLeftWheel);
            UpdateSuspensionParams(FrontRightWheel);
            UpdateSuspensionParams(RearLeftWheel);
            UpdateSuspensionParams(RearRightWheel);
            // 入力を収集
            GatherInputs();
            // パワーアップを適用して最終ステータスを計算
            TickPowerups();
            // 重心位置を設定
            Rigidbody.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);

            // 地面に接触しているホイールの数をカウント
            int groundedCount = 0;
            if (FrontLeftWheel.isGrounded && FrontLeftWheel.GetGroundHit(out WheelHit hit))
                groundedCount++;
            if (FrontRightWheel.isGrounded && FrontRightWheel.GetGroundHit(out hit))
                groundedCount++;
            if (RearLeftWheel.isGrounded && RearLeftWheel.GetGroundHit(out hit))
                groundedCount++;
            if (RearRightWheel.isGrounded && RearRightWheel.GetGroundHit(out hit))
                groundedCount++;

            // 地面接触割合と空中割合を計算
            GroundPercent = (float)groundedCount / 4.0f;
            AirPercent = 1 - GroundPercent;
            // 移動可能な場合は車両物理を適用
            if (m_CanMove)
            {
                MoveVehicle(Input.Accelerate, Input.Brake, Input.TurnInput);
            }
            // 空中処理
            GroundAirbourne();
            // 前フレームの地面接触割合を記録
            m_PreviousGroundPercent = GroundPercent;
        }

        // 全ての入力ソースから入力を収集する
        void GatherInputs()
        {
            // 入力をリセット
            Input = new InputData();
            WantsToDrift = false;

            // 全ての入力ソースから非ゼロ入力を収集
            for (int i = 0; i < m_Inputs.Length; i++)
            {
                Input = m_Inputs[i].GenerateInput();
                // ブレーキを押しながら前進している場合はドリフト要求
                WantsToDrift = Input.Brake && Vector3.Dot(Rigidbody.linearVelocity, transform.forward) > 0.0f;
            }
        }

        // パワーアップの管理と最終ステータスの計算
        void TickPowerups()
        {
            // 時間切れのパワーアップを削除
            m_ActivePowerupList.RemoveAll((p) => { return p.ElapsedTime > p.MaxTime; });
            // パワーアップを初期化
            var powerups = new Stats();
            // 全てのアクティブなパワーアップを合計
            for (int i = 0; i < m_ActivePowerupList.Count; i++)
            {
                var p = m_ActivePowerupList[i];

                // 経過時間を加算
                p.ElapsedTime += Time.fixedDeltaTime;

                // パワーアップを合計
                powerups += p.modifiers;
            }
            // 基本ステータスにパワーアップを加算
            m_FinalStats = baseStats + powerups;
            // グリップ値を0-1の範囲にクランプ
            m_FinalStats.Grip = Mathf.Clamp(m_FinalStats.Grip, 0, 1);
        }

        // 空中時の処理
        // 空中では追加重力を適用
        void GroundAirbourne()
        {
            // 完全に空中にいる場合、追加重力を適用してより速く落下
            if (AirPercent >= 1)
            {
                Rigidbody.linearVelocity += Physics.gravity * Time.fixedDeltaTime * m_FinalStats.AddedGravity;
            }
        }

        // カートをリセットする（転倒した時などに使用）
        // X軸とZ軸の回転を0にして正立させる
        public void Reset()
        {
            Vector3 euler = transform.rotation.eulerAngles;
            euler.x = euler.z = 0f;  // X軸とZ軸の回転をリセット
            transform.rotation = Quaternion.Euler(euler);
        }

        // ローカル座標系での速度を正規化して取得
        // 前進は正、後進は負の値を返す
        // <return>正規化された速度（-1.0 〜 1.0）
        public float LocalSpeed()
        {
            if (m_CanMove)
            {
                // 前方向との内積を計算
                float dot = Vector3.Dot(transform.forward, Rigidbody.linearVelocity);
                if (Mathf.Abs(dot) > 0.1f)
                {
                    float speed = Rigidbody.linearVelocity.magnitude;
                    // 前進か後進かで最高速度で正規化
                    return dot < 0 ? -(speed / m_FinalStats.ReverseSpeed) : (speed / m_FinalStats.TopSpeed);
                }
                return 0f;
            }
            else
            {
                // レース開始前のカウントダウン時などにカート音を再生するための値
                return Input.Accelerate ? 1.0f : 0.0f;
            }
        }

        // 衝突開始時のコールバック
        void OnCollisionEnter(Collision collision) => m_HasCollision = true;

        // 衝突終了時のコールバック
        void OnCollisionExit(Collision collision) => m_HasCollision = false;

        // 衝突継続中のコールバック
        // 最も上向きの法線を記録する
        void OnCollisionStay(Collision collision)
        {
            m_HasCollision = true;
            m_LastCollisionNormal = Vector3.zero;
            float dot = -1.0f;

            // 全ての接触点から最も上向きの法線を選択
            foreach (var contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > dot)
                    m_LastCollisionNormal = contact.normal;
            }
        }

        // 車両の移動処理
        // 加速、ブレーキ、ステアリング、ドリフトの全ての処理を行う
        void MoveVehicle(bool accelerate, bool brake, float turnInput)
        {
            // 加速入力を-1〜1の範囲に変換
            float accelInput = (accelerate ? 1.0f : 0.0f) - (brake ? 1.0f : 0.0f);
            // 加速カーブ係数（手動調整用）
            float accelerationCurveCoeff = 5;
            Vector3 localVel = transform.InverseTransformVector(Rigidbody.linearVelocity);
            // 加速方向と現在の速度方向を判定
            bool accelDirectionIsFwd = accelInput >= 0;
            bool localVelDirectionIsFwd = localVel.z >= 0;
            // 現在の移動方向に応じた最高速度を使用
            float maxSpeed = localVelDirectionIsFwd ? m_FinalStats.TopSpeed : m_FinalStats.ReverseSpeed;
            float accelPower = accelDirectionIsFwd ? m_FinalStats.Acceleration : m_FinalStats.ReverseAcceleration;
            // 現在の速度と加速ランプの計算
            float currentSpeed = Rigidbody.linearVelocity.magnitude;
            float accelRampT = currentSpeed / maxSpeed;
            float multipliedAccelerationCurve = m_FinalStats.AccelerationCurve * accelerationCurveCoeff;
            float accelRamp = Mathf.Lerp(multipliedAccelerationCurve, 1, accelRampT * accelRampT);
            // ブレーキ判定（移動方向と逆の入力をした場合）
            bool isBraking = (localVelDirectionIsFwd && brake) || (!localVelDirectionIsFwd && accelerate);
            // ブレーキ中の場合はブレーキ力を使用、そうでなければ通常の加速力
            float finalAccelPower = isBraking ? m_FinalStats.Braking : accelPower;
            // 最終的な加速度を計算
            float finalAcceleration = finalAccelPower * accelRamp;
            // ステアリング力の計算（ドリフト中は専用の値を使用）
            float turningPower = IsDrifting ? m_DriftTurningPower : turnInput * m_FinalStats.Steer;
            // ステアリング角度を適用して前方向ベクトルを回転
            Quaternion turnAngle = Quaternion.AngleAxis(turningPower, transform.up);
            Vector3 fwd = turnAngle * transform.forward;

            // 移動ベクトルを計算（地面に接触しているか衝突中の場合のみ適用）
            Vector3 movement = fwd * accelInput * finalAcceleration * ((m_HasCollision || GroundPercent > 0.0f) ? 1.0f : 0.0f);

            // 最高速度制限の処理
            bool wasOverMaxSpeed = currentSpeed >= maxSpeed;

            // 最高速度を超えている場合は加速を無効化（ブレーキ時は除く）
            if (wasOverMaxSpeed && !isBraking)
                movement *= 0.0f;

            // 新しい速度を計算（Y軸速度は維持）
            Vector3 newVelocity = Rigidbody.linearVelocity + movement * Time.fixedDeltaTime;
            newVelocity.y = Rigidbody.linearVelocity.y;

            // 地面に接触していて最高速度を超えていない場合は速度をクランプ
            if (GroundPercent > 0.0f && !wasOverMaxSpeed)
            {
                newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
            }

            // 慣性による減速処理（入力がない場合）
            if (Mathf.Abs(accelInput) < k_NullInput && GroundPercent > 0.0f)
            {
                newVelocity = Vector3.MoveTowards(newVelocity, new Vector3(0, Rigidbody.linearVelocity.y, 0), Time.fixedDeltaTime * m_FinalStats.CoastingDrag);
            }

            // 計算した速度を適用
            Rigidbody.linearVelocity = newVelocity;
            // ドリフトと回転処理（地面に接触している場合のみ）
            if (GroundPercent > 0.0f)
            {
                // 空中から地面に着地した場合の処理
                if (m_InAir)
                {
                    m_InAir = false;
                }

                // 角速度制御の係数
                float angularVelocitySteering = 0.4f;
                float angularVelocitySmoothSpeed = 20f;

                // 後進中で後進入力の場合は回転を逆にする
                if (!localVelDirectionIsFwd && !accelDirectionIsFwd)
                    angularVelocitySteering *= -1.0f;

                var angularVel = Rigidbody.angularVelocity;

                // Y軸の角速度を目標値に向かって移動
                angularVel.y = Mathf.MoveTowards(angularVel.y, turningPower * angularVelocitySteering, Time.fixedDeltaTime * angularVelocitySmoothSpeed);

                // 角速度を適用
                Rigidbody.angularVelocity = angularVel;

                // 速度ベクトルも回転させて即座に方向転換を実現
                float velocitySteering = 25f;  // 速度ステアリング係数

                // 着地時のドリフト開始判定
                if (GroundPercent >= 0.0f && m_PreviousGroundPercent < 0.1f)
                {
                    Vector3 flattenVelocity = Vector3.ProjectOnPlane(Rigidbody.linearVelocity, m_VerticalReference).normalized;
                    if (Vector3.Dot(flattenVelocity, transform.forward * Mathf.Sign(accelInput)) < Mathf.Cos(MinAngleToFinishDrift * Mathf.Deg2Rad))
                    {
                        IsDrifting = true;
                        m_CurrentGrip = DriftGrip;
                        m_DriftTurningPower = 0.0f;
                    }
                }

                // ドリフト管理
                if (!IsDrifting)
                {
                    // ドリフト開始条件：ドリフト要求またはブレーキ中で、十分な速度がある場合
                    if ((WantsToDrift || isBraking) && currentSpeed > maxSpeed * MinSpeedPercentToFinishDrift)
                    {
                        IsDrifting = true;
                        m_DriftTurningPower = turningPower + (Mathf.Sign(turningPower) * DriftAdditionalSteer);
                        m_CurrentGrip = DriftGrip;
                    }
                }

                if (IsDrifting)
                {
                    float turnInputAbs = Mathf.Abs(turnInput);

                    // ステアリング入力がない場合はドリフトパワーを減衰
                    if (turnInputAbs < k_NullInput)
                        m_DriftTurningPower = Mathf.MoveTowards(m_DriftTurningPower, 0.0f, Mathf.Clamp01(DriftDampening * Time.fixedDeltaTime));

                    // ステアリング入力に基づいてドリフトパワーを更新
                    float driftMaxSteerValue = m_FinalStats.Steer + DriftAdditionalSteer;
                    m_DriftTurningPower = Mathf.Clamp(m_DriftTurningPower + (turnInput * Mathf.Clamp01(DriftControl * Time.fixedDeltaTime)), -driftMaxSteerValue, driftMaxSteerValue);

                    // 車体が速度方向を向いているかチェック
                    bool facingVelocity = Vector3.Dot(Rigidbody.linearVelocity.normalized, transform.forward * Mathf.Sign(accelInput)) > Mathf.Cos(MinAngleToFinishDrift * Mathf.Deg2Rad);

                    // ドリフト終了条件の判定
                    bool canEndDrift = true;
                    if (isBraking)
                        canEndDrift = false;  // ブレーキ中は終了不可
                    else if (!facingVelocity)
                        canEndDrift = false;  // 速度方向を向いていない場合は終了不可
                    else if (turnInputAbs >= k_NullInput && currentSpeed > maxSpeed * MinSpeedPercentToFinishDrift)
                        canEndDrift = false;  // ステアリング入力があり速度が十分な場合は継続

                    // ドリフト終了処理
                    if (canEndDrift || currentSpeed < k_NullSpeed)
                    {
                        IsDrifting = false;
                        m_CurrentGrip = m_FinalStats.Grip;  // 通常のグリップに戻す
                    }
                }

                // 現在のグリップ値とステアリングに基づいて速度ベクトルを回転
                Rigidbody.linearVelocity = Quaternion.AngleAxis(turningPower * Mathf.Sign(localVel.z) * velocitySteering * m_CurrentGrip * Time.fixedDeltaTime, transform.up) * Rigidbody.linearVelocity;
            }
            else
            {
                // 空中状態に設定
                m_InAir = true;
            }

            // 地面検出と垂直基準ベクトルの更新
            bool validPosition = false;

            // レイキャストで地面を検出（Ground, Environment, Track レイヤー）
            if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out RaycastHit hit, 3.0f, 1 << 9 | 1 << 10 | 1 << 11))
            {
                // 衝突法線とレイキャストの法線の良い方を選択
                Vector3 lerpVector = (m_HasCollision && m_LastCollisionNormal.y > hit.normal.y) ? m_LastCollisionNormal : hit.normal;
                // 垂直基準を新しい法線方向に補間（地面にいる場合は高速補間）
                m_VerticalReference = Vector3.Slerp(m_VerticalReference, lerpVector, Mathf.Clamp01(AirborneReorientationCoefficient * Time.fixedDeltaTime * (GroundPercent > 0.0f ? 10.0f : 1.0f)));
            }
            else
            {
                // 地面が検出できない場合は衝突法線またはVector.upを使用
                Vector3 lerpVector = (m_HasCollision && m_LastCollisionNormal.y > 0.0f) ? m_LastCollisionNormal : Vector3.up;
                m_VerticalReference = Vector3.Slerp(m_VerticalReference, lerpVector, Mathf.Clamp01(AirborneReorientationCoefficient * Time.fixedDeltaTime));
            }

            // 有効位置の判定（地面接触が多く、衝突なし、垂直基準が上向き）
            validPosition = GroundPercent > 0.7f && !m_HasCollision && Vector3.Dot(m_VerticalReference, Vector3.up) > 0.9f;

            // 空中または半分地面にいる場合の姿勢制御
            if (GroundPercent < 0.7f)
            {
                // Y軸角速度を減衰
                Rigidbody.angularVelocity = new Vector3(0.0f, Rigidbody.angularVelocity.y * 0.98f, 0.0f);

                // 垂直基準に基づいて姿勢を補正
                Vector3 finalOrientationDirection = Vector3.ProjectOnPlane(transform.forward, m_VerticalReference);
                finalOrientationDirection.Normalize();
                if (finalOrientationDirection.sqrMagnitude > 0.0f)
                {
                    Rigidbody.MoveRotation(Quaternion.Lerp(Rigidbody.rotation, Quaternion.LookRotation(finalOrientationDirection, m_VerticalReference), Mathf.Clamp01(AirborneReorientationCoefficient * Time.fixedDeltaTime)));
                }
            }
            else if (validPosition)
            {
                // 有効な位置と回転を記録（リセット用）
                m_LastValidPosition = transform.position;
                m_LastValidRotation.eulerAngles = new Vector3(0.0f, transform.rotation.y, 0.0f);
            }
        }
    }
}