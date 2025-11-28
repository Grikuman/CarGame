using UnityEngine;

/// <summary>
/// ブーストアイテムの「ゆるやかな上下動（浮遊）」と「回転」を制御するスクリプト。
/// </summary>
public class BoostItemAnimator : MonoBehaviour
{
    [Header("上下動 (浮遊) 設定")]
    [Tooltip("上下の揺れの最大幅（振幅）。")]
    public float floatAmplitude = 0.15f;

    [Tooltip("上下の動きの速さ。")]
    public float floatSpeed = 1.8f;

    [Header("回転 設定")]
    [Tooltip("Y軸を中心とした回転速度（度/秒）。")]
    public float rotationSpeed = 30f; // 1秒間に30度回転（ゆっくり）

    private Vector3 startPosition;

    // --- 初期化 ---
    void Start()
    {
        // アイテムの初期位置（基準の高さ）を記録
        startPosition = transform.position;
    }

    // --- 毎フレームの更新 ---
    void Update()
    {
        // 経過時間 T を取得
        float time = Time.time;

        // --- 1. 上下動の計算と適用 ---
        UpdateFloating(time);

        // --- 2. 回転の計算と適用 ---
        UpdateRotation();
    }

    /// <summary>
    /// サイン波に基づき、アイテムの位置を上下に変化させます。
    /// </summary>
    void UpdateFloating(float time)
    {
        // サイン波 (±1) * 振幅 で、時間と共に滑らかに上下する値を作成
        float newY = Mathf.Sin(time * floatSpeed) * floatAmplitude;

        // 初期位置に変化分を加えて、アイテムの位置を更新
        transform.position = startPosition + new Vector3(0, newY, 0);
    }

    /// <summary>
    /// Y軸を中心とした回転を適用します。
    /// </summary>
    void UpdateRotation()
    {
        // アイテムを毎フレーム、指定された速度（度/秒）でY軸を中心に回転させる
        // Space.World: シーンのワールド座標系のY軸（上方向）を基準に回転
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}