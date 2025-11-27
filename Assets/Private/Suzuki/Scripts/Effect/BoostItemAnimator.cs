using UnityEngine;

/// <summary>
/// ブーストアイテムの「ゆるやかな上下動（浮遊）」のみを制御するスクリプト。
/// </summary>
public class BoostItemAnimator : MonoBehaviour
{
    [Header("上下動 (浮遊) 設定")]
    [Tooltip("上下の揺れの最大幅（振幅 A）。推奨値: 0.1〜0.2")]
    public float floatAmplitude = 0.15f;

    [Tooltip("上下の動きの速さ（角振動数 ω）。推奨値: 1.5〜2.0")]
    public float floatSpeed = 1.8f;

    private Vector3 startPosition;

    // --- 初期化 ---
    void Start()
    {
        // 1. アイテムの初期位置（基準の高さ）を記録
        startPosition = transform.position;
    }

    // --- 毎フレームの更新 ---
    void Update()
    {
        // 経過時間 T を取得
        float time = Time.time;

        // --- 上下動の計算と適用 ---
        UpdateFloating(time);
    }

    /// <summary>
    /// サイン波に基づき、アイテムの位置を上下に変化させます。
    /// </summary>
    void UpdateFloating(float time)
    {
        // Y軸の新しい位置を計算します。
        // サイン波 (±1) * 振幅 で、時間と共に滑らかに上下する値を作成
        float newY = Mathf.Sin(time * floatSpeed) * floatAmplitude;

        // 初期位置に変化分を加えて、アイテムの位置を更新
        transform.position = startPosition + new Vector3(0, newY, 0);
    }
}