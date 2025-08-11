using UnityEngine;

/// <summary>
/// スプライン曲線の制御点データ
/// レーシングコースの各制御点の位置、接線、道路幅、バンク角を定義
/// </summary>
[System.Serializable]
public class SplinePoint
{
    [Header("位置情報")]
    public Vector3 position;        // 制御点の3D位置
    
    [Header("接線情報")]
    public Vector3 inTangent;       // 入力接線ベクトル
    public Vector3 outTangent;      // 出力接線ベクトル
    
    [Header("道路設定")]
    [Range(1f, 100f)]
    public float width = 30f;       // この点での道路幅（メートル）
    
    [Range(-45f, 45f)]
    public float banking = 0f;      // バンク角（度）
    
    /// <summary>
    /// デフォルト値でSplinePointを作成
    /// </summary>
    public SplinePoint()
    {
        position = Vector3.zero;
        inTangent = Vector3.forward;
        outTangent = Vector3.forward;
        width = 30f;
        banking = 0f;
    }
    
    /// <summary>
    /// 指定位置でSplinePointを作成
    /// </summary>
    /// <param name="pos">制御点の位置</param>
    public SplinePoint(Vector3 pos)
    {
        position = pos;
        inTangent = Vector3.forward;
        outTangent = Vector3.forward;
        width = 30f;
        banking = 0f;
    }
    
    /// <summary>
    /// 完全指定でSplinePointを作成
    /// </summary>
    /// <param name="pos">制御点の位置</param>
    /// <param name="inTan">入力接線</param>
    /// <param name="outTan">出力接線</param>
    /// <param name="roadWidth">道路幅</param>
    /// <param name="bankAngle">バンク角</param>
    public SplinePoint(Vector3 pos, Vector3 inTan, Vector3 outTan, float roadWidth = 30f, float bankAngle = 0f)
    {
        position = pos;
        inTangent = inTan;
        outTangent = outTan;
        width = Mathf.Clamp(roadWidth, 1f, 100f);
        banking = Mathf.Clamp(bankAngle, -45f, 45f);
    }
    
    /// <summary>
    /// SplinePointの深いコピーを作成
    /// </summary>
    /// <returns>コピーされたSplinePoint</returns>
    public SplinePoint Clone()
    {
        return new SplinePoint(position, inTangent, outTangent, width, banking);
    }
}