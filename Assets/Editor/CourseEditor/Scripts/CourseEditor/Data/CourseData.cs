using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーシングコースのデータを保存するScriptableObject
/// スプライン制御点、設定、将来的にはチェックポイントや障害物データも含む
/// </summary>
[CreateAssetMenu(fileName = "NewCourse", menuName = "Racing/Course Data")]
public class CourseData : ScriptableObject
{
    [Header("スプライン制御点")]
    [SerializeField]
    private List<SplinePoint> m_splinePoints = new List<SplinePoint>();
    
    [Header("コース設定")]
    [SerializeField]
    private CourseSettings m_settings = new CourseSettings();
    
    
    /// <summary>
    /// スプライン制御点のリストを取得
    /// </summary>
    public List<SplinePoint> SplinePoints => m_splinePoints;
    
    /// <summary>
    /// コース設定を取得
    /// </summary>
    public CourseSettings Settings => m_settings;
    
    /// <summary>
    /// 制御点の数を取得
    /// </summary>
    public int PointCount => m_splinePoints.Count;
    
    /// <summary>
    /// コースが有効（最低限の制御点がある）かどうか
    /// </summary>
    public bool IsValidCourse => m_splinePoints.Count >= 2;
    
    /// <summary>
    /// 制御点を追加
    /// </summary>
    /// <param name="point">追加する制御点</param>
    public void AddPoint(SplinePoint point)
    {
        if (point != null)
        {
            m_splinePoints.Add(point);
        }
    }
    
    /// <summary>
    /// 指定インデックスに制御点を挿入
    /// </summary>
    /// <param name="index">挿入位置</param>
    /// <param name="point">挿入する制御点</param>
    public void InsertPoint(int index, SplinePoint point)
    {
        if (point != null && index >= 0 && index <= m_splinePoints.Count)
        {
            m_splinePoints.Insert(index, point);
        }
    }
    
    /// <summary>
    /// 指定インデックスの制御点を削除
    /// </summary>
    /// <param name="index">削除する制御点のインデックス</param>
    public void RemovePoint(int index)
    {
        if (index >= 0 && index < m_splinePoints.Count)
        {
            m_splinePoints.RemoveAt(index);
        }
    }
    
    /// <summary>
    /// 指定インデックスの制御点を取得
    /// </summary>
    /// <param name="index">制御点のインデックス</param>
    /// <returns>制御点、インデックスが無効な場合はnull</returns>
    public SplinePoint GetPoint(int index)
    {
        if (index >= 0 && index < m_splinePoints.Count)
        {
            return m_splinePoints[index];
        }
        return null;
    }
    
    /// <summary>
    /// すべての制御点をクリア
    /// </summary>
    public void ClearPoints()
    {
        m_splinePoints.Clear();
    }
    
    /// <summary>
    /// デフォルトのコースを作成
    /// </summary>
    public void CreateDefaultStraightCourse()
    {
        ClearPoints();
        
        
        AddPoint(CourseDefaults.CreateDefaultPoint(new Vector3(0, 0, 0)));
        AddPoint(CourseDefaults.CreateDefaultPoint(new Vector3(-30, 0, 80)));
        AddPoint(CourseDefaults.CreateDefaultPoint(new Vector3(60, 0, 170)));
        AddPoint(CourseDefaults.CreateDefaultPoint(new Vector3(220, 0, 100)));
        AddPoint(CourseDefaults.CreateDefaultPoint(new Vector3(210, 0, -40)));
        AddPoint(CourseDefaults.CreateDefaultPoint(new Vector3(90, 0, -60)));

        m_settings.m_courseName = "デフォルトコース";
        m_settings.m_courseDescription = "";

        RecalculateAllTangents();
    }
    
    /// <summary>
    /// 指定位置に新しい制御点を追加（前後の制御点から自動計算）
    /// </summary>
    /// <param name="index">挿入位置（この位置の前に挿入）</param>
    /// <returns>作成された制御点のインデックス</returns>
    public int AddPointAt(int index)
    {
        if (m_splinePoints.Count < 2)
        {
            // 制御点が少ない場合は末尾に追加
            Vector3 position = Vector3.zero;
            if (m_splinePoints.Count == 1)
            {
                position = m_splinePoints[0].position + Vector3.forward * 10f;
            }
            
            SplinePoint simplePoint = CourseDefaults.CreateDefaultPoint(position);
            AddPoint(simplePoint);
            return m_splinePoints.Count - 1;
        }
        
        // 挿入位置を調整
        index = Mathf.Clamp(index, 0, m_splinePoints.Count);
        
        Vector3 newPosition;
        
        if (index == 0)
        {
            // 最初に挿入する場合
            Vector3 direction = (m_splinePoints[1].position - m_splinePoints[0].position).normalized;
            newPosition = m_splinePoints[0].position - direction * 10f;
        }
        else if (index >= m_splinePoints.Count)
        {
            // 最後に挿入する場合
            int lastIndex = m_splinePoints.Count - 1;
            Vector3 direction = (m_splinePoints[lastIndex].position - m_splinePoints[lastIndex - 1].position).normalized;
            newPosition = m_splinePoints[lastIndex].position + direction * 10f;
        }
        else
        {
            // 中間に挿入する場合：前後の制御点の中点
            newPosition = (m_splinePoints[index - 1].position + m_splinePoints[index].position) * 0.5f;
        }
        
        // 前後の制御点の幅とバンク角を補間
        float width = 10f;
        float banking = 0f;
        
        if (index > 0 && index < m_splinePoints.Count)
        {
            width = (m_splinePoints[index - 1].width + m_splinePoints[index].width) * 0.5f;
            banking = (m_splinePoints[index - 1].banking + m_splinePoints[index].banking) * 0.5f;
        }
        else if (index > 0)
        {
            width = m_splinePoints[index - 1].width;
            banking = m_splinePoints[index - 1].banking;
        }
        else if (index < m_splinePoints.Count)
        {
            width = m_splinePoints[index].width;
            banking = m_splinePoints[index].banking;
        }
        
        SplinePoint newPoint = CourseDefaults.CreateDefaultPoint(newPosition);
        newPoint.width = width;
        newPoint.banking = banking;
        
        // 接線を自動設定
        CalculateAutoTangents(newPoint, index, newPosition);
        
        InsertPoint(index, newPoint);
        return index;
    }
    
    /// <summary>
    /// 制御点を安全に削除（最低限の制御点数を保持）
    /// </summary>
    /// <param name="index">削除する制御点のインデックス</param>
    /// <returns>削除に成功したかどうか</returns>
    public bool RemovePointSafely(int index)
    {
        // 最低限の制御点数をチェック
        if (m_splinePoints.Count <= 2)
        {
            // 最低2つの制御点が必要なため削除を拒否
            return false;
        }
        
        if (index >= 0 && index < m_splinePoints.Count)
        {
            RemovePoint(index);
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 制御点の接線を自動計算
    /// </summary>
    /// <param name="point">対象の制御点</param>
    /// <param name="index">制御点のインデックス</param>
    /// <param name="position">制御点の位置</param>
    public void CalculateAutoTangents(SplinePoint point, int index, Vector3 position)
    {
        CalculateSmartTangents(point, index, position, 5f);
    }
    
    /// <summary>
    /// 前後のポイントから自動で接線ハンドルを調整（改良版）
    /// </summary>
    /// <param name="point">対象の制御点</param>
    /// <param name="index">制御点のインデックス</param>
    /// <param name="position">制御点の位置</param>
    /// <param name="tangentStrength">接線の強度（距離の割合）</param>
    public void CalculateSmartTangents(SplinePoint point, int index, Vector3 position, float tangentStrength = 5f)
    {
        Vector3 inDirection = Vector3.zero;
        Vector3 outDirection = Vector3.zero;
        float inLength = tangentStrength;
        float outLength = tangentStrength;
        
        // 前後の制御点の存在確認
        bool hasPrev = index > 0;
        bool hasNext = index < m_splinePoints.Count - 1;
        
        Vector3 prevPos = Vector3.zero;
        Vector3 nextPos = Vector3.zero;
        
        if (hasPrev) prevPos = m_splinePoints[index - 1].position;
        if (hasNext) nextPos = m_splinePoints[index + 1].position;
        
        // 入力接線と出力接線の計算
        if (hasPrev && hasNext)
        {
            // 前後の制御点がある場合：滑らかな曲線を作るために前後を通る方向を使用
            Vector3 throughDirection = (nextPos - prevPos).normalized;
            
            inDirection = -throughDirection;  // 入力接線（制御点に入ってくる方向）
            outDirection = throughDirection;  // 出力接線（制御点から出ていく方向）
            
            // 距離に応じて接線の長さを調整
            float prevDistance = Vector3.Distance(position, prevPos);
            float nextDistance = Vector3.Distance(position, nextPos);
            
            inLength = prevDistance * 0.3f;   // 前の制御点との距離の30%
            outLength = nextDistance * 0.3f;  // 次の制御点との距離の30%
        }
        else if (hasPrev)
        {
            // 前の制御点のみある場合
            inDirection = (prevPos - position).normalized;
            outDirection = -inDirection;
            
            float distance = Vector3.Distance(position, prevPos);
            inLength = outLength = distance * 0.3f;
        }
        else if (hasNext)
        {
            // 次の制御点のみある場合
            outDirection = (nextPos - position).normalized;
            inDirection = -outDirection;
            
            float distance = Vector3.Distance(position, nextPos);
            inLength = outLength = distance * 0.3f;
        }
        else
        {
            // 前後の制御点がない場合はデフォルト方向
            inDirection = Vector3.back;
            outDirection = Vector3.forward;
            inLength = outLength = tangentStrength;
        }
        
        // 接線を設定
        point.inTangent = inDirection * inLength;
        point.outTangent = outDirection * outLength;
    }
    
    /// <summary>
    /// すべての制御点の接線を自動調整
    /// </summary>
    public void RecalculateAllTangents()
    {
        for (int i = 0; i < m_splinePoints.Count; i++)
        {
            SplinePoint point = m_splinePoints[i];
            CalculateSmartTangents(point, i, point.position);
        }
        
        // 閉じたコースの場合、最初と最後の制御点の接線を連続にする
        if (m_settings.m_isClosedLoop && m_splinePoints.Count > 2)
        {
            EnsureLoopTangentContinuity();
        }
    }
    
    /// <summary>
    /// 指定された制御点とその前後の接線を再計算
    /// </summary>
    /// <param name="centerIndex">中心となる制御点のインデックス</param>
    public void RecalculateAdjacentTangents(int centerIndex)
    {
        // 前の制御点
        if (centerIndex > 0)
        {
            SplinePoint prevPoint = m_splinePoints[centerIndex - 1];
            CalculateSmartTangents(prevPoint, centerIndex - 1, prevPoint.position);
        }
        
        // 中心の制御点
        if (centerIndex >= 0 && centerIndex < m_splinePoints.Count)
        {
            SplinePoint centerPoint = m_splinePoints[centerIndex];
            CalculateSmartTangents(centerPoint, centerIndex, centerPoint.position);
        }
        
        // 次の制御点
        if (centerIndex < m_splinePoints.Count - 1)
        {
            SplinePoint nextPoint = m_splinePoints[centerIndex + 1];
            CalculateSmartTangents(nextPoint, centerIndex + 1, nextPoint.position);
        }
    }
    
    /// <summary>
    /// 閉じたコースの接線連続性を確保
    /// </summary>
    private void EnsureLoopTangentContinuity()
    {
        if (m_splinePoints.Count < 3) return;
        
        SplinePoint firstPoint = m_splinePoints[0];
        SplinePoint lastPoint = m_splinePoints[m_splinePoints.Count - 1];
        SplinePoint secondPoint = m_splinePoints[1];
        SplinePoint secondLastPoint = m_splinePoints[m_splinePoints.Count - 2];
        
        // 最初の制御点の入力接線を、最後から最初への方向ベクトルから計算
        Vector3 lastToFirstDirection = (firstPoint.position - lastPoint.position).normalized;
        float lastToFirstDistance = Vector3.Distance(lastPoint.position, firstPoint.position);
        firstPoint.inTangent = -lastToFirstDirection * (lastToFirstDistance * 0.3f);
        
        // 最後の制御点の出力接線を、最後から最初への方向ベクトルから計算
        lastPoint.outTangent = lastToFirstDirection * (lastToFirstDistance * 0.3f);
        
        // 接線の長さを隣接する制御点との距離に基づいて調整
        float firstToSecondDistance = Vector3.Distance(firstPoint.position, secondPoint.position);
        float secondLastToLastDistance = Vector3.Distance(secondLastPoint.position, lastPoint.position);
        
        // 出力接線の長さを調整
        Vector3 firstToSecondDirection = (secondPoint.position - firstPoint.position).normalized;
        firstPoint.outTangent = firstToSecondDirection * (firstToSecondDistance * 0.3f);
        
        // 入力接線の長さを調整
        Vector3 secondLastToLastDirection = (lastPoint.position - secondLastPoint.position).normalized;
        lastPoint.inTangent = -secondLastToLastDirection * (secondLastToLastDistance * 0.3f);
    }
    
    /// <summary>
    /// データの整合性をチェックし、必要に応じて修正
    /// </summary>
    public void ValidateData()
    {
        // 設定値を検証
        if (m_settings != null)
        {
            m_settings.ValidateSettings();
        }
        else
        {
            m_settings = new CourseSettings();
        }
        
        // 制御点リストがnullの場合は初期化
        if (m_splinePoints == null)
        {
            m_splinePoints = new List<SplinePoint>();
        }
        
        // 制御点が少なすぎる場合はデフォルトコースを作成
        if (m_splinePoints.Count < 2)
        {
            CreateDefaultStraightCourse();
        }
    }
    
    /// <summary>
    /// ScriptableObjectが作成された時の初期化
    /// </summary>
    private void OnEnable()
    {
        if (m_splinePoints == null)
        {
            m_splinePoints = new List<SplinePoint>();
        }
        
        if (m_settings == null)
        {
            m_settings = new CourseSettings();
        }
    }
}