using UnityEngine;

/// <summary>
/// スプライン曲線の数学的計算を行うユーティリティクラス
/// キュービックベジェ曲線ベースのスプライン計算機能を提供
/// </summary>
public static class SplineMath
{
    /// <summary>
    /// キュービックベジェ曲線上の点を計算
    /// </summary>
    /// <param name="p0">開始点</param>
    /// <param name="p1">開始点の制御点</param>
    /// <param name="p2">終了点の制御点</param>
    /// <param name="p3">終了点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>曲線上の点</returns>
    public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        float oneMinusT2 = oneMinusT * oneMinusT;
        float oneMinusT3 = oneMinusT2 * oneMinusT;
        float t2 = t * t;
        float t3 = t2 * t;
        
        return oneMinusT3 * p0 +
               3f * oneMinusT2 * t * p1 +
               3f * oneMinusT * t2 * p2 +
               t3 * p3;
    }
    
    /// <summary>
    /// キュービックベジェ曲線の1次微分（接線ベクトル）を計算
    /// </summary>
    /// <param name="p0">開始点</param>
    /// <param name="p1">開始点の制御点</param>
    /// <param name="p2">終了点の制御点</param>
    /// <param name="p3">終了点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>正規化されていない接線ベクトル</returns>
    public static Vector3 CubicBezierDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        float oneMinusT2 = oneMinusT * oneMinusT;
        float t2 = t * t;
        
        return 3f * oneMinusT2 * (p1 - p0) +
               6f * oneMinusT * t * (p2 - p1) +
               3f * t2 * (p3 - p2);
    }
    
    /// <summary>
    /// SplinePointから制御点を生成してベジェ曲線を計算
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>曲線上の点</returns>
    public static Vector3 EvaluateSpline(SplinePoint point0, SplinePoint point1, float t)
    {
        Vector3 p0 = point0.position;
        Vector3 p1 = point0.position + point0.outTangent;
        Vector3 p2 = point1.position + point1.inTangent;
        Vector3 p3 = point1.position;
        
        return CubicBezier(p0, p1, p2, p3, t);
    }
    
    /// <summary>
    /// SplinePointから制御点を生成してベジェ曲線の接線を計算
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>正規化された接線ベクトル</returns>
    public static Vector3 EvaluateSplineTangent(SplinePoint point0, SplinePoint point1, float t)
    {
        Vector3 p0 = point0.position;
        Vector3 p1 = point0.position + point0.outTangent;
        Vector3 p2 = point1.position + point1.inTangent;
        Vector3 p3 = point1.position;
        
        Vector3 derivative = CubicBezierDerivative(p0, p1, p2, p3, t);
        
        // 微分がゼロの場合の対処
        if (derivative.magnitude < 0.0001f)
        {
            // 前後の点から方向を推定
            Vector3 forward = (point1.position - point0.position).normalized;
            return forward;
        }
        
        return derivative.normalized;
    }
    
    /// <summary>
    /// 接線ベクトルから法線ベクトル（右方向）を計算
    /// </summary>
    /// <param name="tangent">正規化された接線ベクトル</param>
    /// <param name="up">上方向ベクトル（通常はVector3.up）</param>
    /// <returns>正規化された法線ベクトル（右方向）</returns>
    public static Vector3 GetNormalFromTangent(Vector3 tangent, Vector3 up)
    {
        // 接線と上方向ベクトルの外積で右方向を取得
        Vector3 right = Vector3.Cross(up, tangent).normalized;
        
        // 外積がゼロの場合（接線が真上や真下を向いている場合）
        if (right.magnitude < 0.0001f)
        {
            // デフォルトの右方向を使用
            right = Vector3.right;
        }
        
        return right;
    }
    
    /// <summary>
    /// バンク角を適用した法線ベクトルを計算
    /// </summary>
    /// <param name="tangent">接線ベクトル</param>
    /// <param name="up">上方向ベクトル</param>
    /// <param name="bankingAngle">バンク角（度）</param>
    /// <returns>バンク角が適用された法線ベクトル</returns>
    public static Vector3 GetBankedNormal(Vector3 tangent, Vector3 up, float bankingAngle)
    {
        Vector3 right = GetNormalFromTangent(tangent, up);
        
        // バンク角を適用（接線軸周りの回転）
        Quaternion bankRotation = Quaternion.AngleAxis(bankingAngle, tangent);
        return bankRotation * right;
    }
    
    /// <summary>
    /// 2つのSplinePoint間でパラメータtに対応する道路幅を線形補間
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>補間された道路幅</returns>
    public static float InterpolateWidth(SplinePoint point0, SplinePoint point1, float t)
    {
        return Mathf.Lerp(point0.width, point1.width, t);
    }
    
    /// <summary>
    /// キュービックベジェ曲線の2次微分を計算
    /// </summary>
    /// <param name="p0">開始点</param>
    /// <param name="p1">開始点の制御点</param>
    /// <param name="p2">終了点の制御点</param>
    /// <param name="p3">終了点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>2次微分ベクトル</returns>
    public static Vector3 CubicBezierSecondDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        
        return 6f * oneMinusT * (p2 - 2f * p1 + p0) +
               6f * t * (p3 - 2f * p2 + p1);
    }
    
    /// <summary>
    /// ベジェ曲線の曲率を計算
    /// </summary>
    /// <param name="p0">開始点</param>
    /// <param name="p1">開始点の制御点</param>
    /// <param name="p2">終了点の制御点</param>
    /// <param name="p3">終了点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>曲率（0に近いほど直線、大きいほど急カーブ）</returns>
    public static float CalculateCurvature(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 firstDerivative = CubicBezierDerivative(p0, p1, p2, p3, t);
        Vector3 secondDerivative = CubicBezierSecondDerivative(p0, p1, p2, p3, t);
        
        // 曲率の公式: |r'(t) × r''(t)| / |r'(t)|^3
        Vector3 cross = Vector3.Cross(firstDerivative, secondDerivative);
        float crossMagnitude = cross.magnitude;
        float velocityMagnitude = firstDerivative.magnitude;
        
        if (velocityMagnitude < 0.0001f)
            return 0f;
            
        return crossMagnitude / (velocityMagnitude * velocityMagnitude * velocityMagnitude);
    }
    
    /// <summary>
    /// SplinePointを使用して曲率を計算
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>曲率</returns>
    public static float EvaluateSplineCurvature(SplinePoint point0, SplinePoint point1, float t)
    {
        Vector3 p0 = point0.position;
        Vector3 p1 = point0.position + point0.outTangent;
        Vector3 p2 = point1.position + point1.inTangent;
        Vector3 p3 = point1.position;
        
        return CalculateCurvature(p0, p1, p2, p3, t);
    }
    
    /// <summary>
    /// 曲線区間の最大曲率を計算（サンプリングベース）
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="sampleCount">サンプリング数</param>
    /// <returns>区間内の最大曲率</returns>
    public static float GetMaxCurvature(SplinePoint point0, SplinePoint point1, int sampleCount = 10)
    {
        float maxCurvature = 0f;
        
        for (int i = 0; i <= sampleCount; i++)
        {
            float t = (float)i / sampleCount;
            float curvature = EvaluateSplineCurvature(point0, point1, t);
            maxCurvature = Mathf.Max(maxCurvature, curvature);
        }
        
        return maxCurvature;
    }
    
    /// <summary>
    /// 曲率に基づいて適応的セグメント数を計算
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="baseSegments">基本セグメント数</param>
    /// <param name="maxSegments">最大セグメント数</param>
    /// <param name="curvatureThreshold">曲率の閾値</param>
    /// <returns>適応的セグメント数</returns>
    public static int CalculateAdaptiveSegments(SplinePoint point0, SplinePoint point1, 
                                              int baseSegments, int maxSegments, float curvatureThreshold = 0.1f)
    {
        float maxCurvature = GetMaxCurvature(point0, point1);
        
        // 曲率が閾値以下なら基本セグメント数
        if (maxCurvature <= curvatureThreshold)
            return baseSegments;
        
        // 曲率に応じてセグメント数を増加
        float curvatureRatio = Mathf.Min(maxCurvature / curvatureThreshold, 4f); // 最大4倍まで
        int adaptiveSegments = Mathf.RoundToInt(baseSegments * curvatureRatio);
        
        return Mathf.Clamp(adaptiveSegments, baseSegments, maxSegments);
    }
    
    /// <summary>
    /// 2つのSplinePoint間でパラメータtに対応するバンク角を線形補間
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="t">パラメータ (0.0-1.0)</param>
    /// <returns>補間されたバンク角（度）</returns>
    public static float InterpolateBanking(SplinePoint point0, SplinePoint point1, float t)
    {
        return Mathf.Lerp(point0.banking, point1.banking, t);
    }
    
    /// <summary>
    /// ベジェ曲線の概算長さを計算（分割近似）
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="subdivisions">分割数（多いほど正確）</param>
    /// <returns>曲線の概算長さ</returns>
    public static float ApproximateCurveLength(SplinePoint point0, SplinePoint point1, int subdivisions = 20)
    {
        if (subdivisions < 2) subdivisions = 2;
        
        float length = 0f;
        Vector3 previousPoint = EvaluateSpline(point0, point1, 0f);
        
        for (int i = 1; i <= subdivisions; i++)
        {
            float t = (float)i / subdivisions;
            Vector3 currentPoint = EvaluateSpline(point0, point1, t);
            length += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
        
        return length;
    }
    
    /// <summary>
    /// 指定された距離に対応するパラメータtを概算で求める
    /// </summary>
    /// <param name="point0">開始制御点</param>
    /// <param name="point1">終了制御点</param>
    /// <param name="targetDistance">目標距離</param>
    /// <param name="iterations">反復計算回数</param>
    /// <returns>距離に対応するパラメータt</returns>
    public static float DistanceToParameter(SplinePoint point0, SplinePoint point1, float targetDistance, int iterations = 10)
    {
        float totalLength = ApproximateCurveLength(point0, point1);
        
        if (targetDistance <= 0f) return 0f;
        if (targetDistance >= totalLength) return 1f;
        
        // 初期推定値
        float t = targetDistance / totalLength;
        
        // ニュートン法的な反復改善
        for (int i = 0; i < iterations; i++)
        {
            float currentDistance = ApproximateCurveLength(point0, point1, Mathf.RoundToInt(t * 20));
            float error = currentDistance - targetDistance;
            
            if (Mathf.Abs(error) < 0.01f) break;
            
            // 微小変化でパラメータを調整
            float dt = 0.01f;
            float nextDistance = ApproximateCurveLength(point0, point1, Mathf.RoundToInt((t + dt) * 20));
            float gradient = (nextDistance - currentDistance) / dt;
            
            if (gradient > 0.001f)
            {
                t -= error / gradient;
                t = Mathf.Clamp01(t);
            }
        }
        
        return Mathf.Clamp01(t);
    }
}