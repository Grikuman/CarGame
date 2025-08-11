using UnityEngine;

/// <summary>
/// レーシングコース全体の設定データ
/// </summary>
[System.Serializable]
public class CourseSettings
{
    [Header("コース基本設定")]
    public string m_courseName = "新しいコース";           // コース名
    public string m_courseDescription = "";               // コースの説明
    public bool m_isClosedLoop = true;                    // 閉じたループコースかどうか
    
    [Header("メッシュ生成設定")]
    [Range(0.1f, 2f)]
    public float m_meshResolution = 1f;                   // メッシュの解像度（低いほど詳細）
    
    [Range(4, 32)]
    public int m_segmentsPerCurve = 16;                   // 曲線1区間あたりのセグメント数
    
    [Header("適応的セグメント分割設定")]
    public bool m_useAdaptiveSegmentation = true;         // 適応的セグメント分割を使用するか
    
    [Range(2, 16)]
    public int m_minSegmentsPerCurve = 4;                 // 最小セグメント数（直線部分）
    
    [Range(8, 64)]
    public int m_maxSegmentsPerCurve = 32;                // 最大セグメント数（急カーブ部分）
    
    [Range(0.01f, 1f)]
    public float m_curvatureThreshold = 0.1f;             // 曲率の閾値（これ以下は直線扱い）
    
    [Header("道路設定")]
    public Material m_roadMaterial;                       // 道路のマテリアル
    
    [Range(0.1f, 5f)]
    public float m_roadThickness = 0.5f;                  // 道路の厚み
    
    [Header("物理設定")]
    public bool m_generateColliders = true;              // コライダーを生成するか
    public PhysicsMaterial m_roadPhysicMaterial;          // 道路の物理マテリアル
    
    [Header("レンダリング設定")]
    public bool m_castShadows = true;                    // 影を落とすか
    public bool m_receiveShadows = true;                 // 影を受けるか
    
    /// <summary>
    /// デフォルト設定でCourseSettingsを作成
    /// </summary>
    public CourseSettings()
    {
        m_courseName = CourseDefaults.Course.DEFAULT_NAME;
        m_courseDescription = CourseDefaults.Course.DEFAULT_DESCRIPTION;
        m_isClosedLoop = CourseDefaults.Course.DEFAULT_IS_CLOSED_LOOP;
        m_meshResolution = CourseDefaults.MeshGeneration.DEFAULT_RESOLUTION;
        m_segmentsPerCurve = CourseDefaults.MeshGeneration.DEFAULT_SEGMENTS_PER_CURVE;
        m_useAdaptiveSegmentation = CourseDefaults.MeshGeneration.DEFAULT_USE_ADAPTIVE;
        m_minSegmentsPerCurve = CourseDefaults.MeshGeneration.DEFAULT_MIN_ADAPTIVE;
        m_maxSegmentsPerCurve = CourseDefaults.MeshGeneration.DEFAULT_MAX_ADAPTIVE;
        m_curvatureThreshold = CourseDefaults.MeshGeneration.DEFAULT_CURVATURE_THRESHOLD;
        m_roadThickness = CourseDefaults.MeshGeneration.DEFAULT_ROAD_THICKNESS;
        m_generateColliders = CourseDefaults.Physics.DEFAULT_GENERATE_COLLIDERS;
        m_castShadows = CourseDefaults.Physics.DEFAULT_CAST_SHADOWS;
        m_receiveShadows = CourseDefaults.Physics.DEFAULT_RECEIVE_SHADOWS;
    }
    
    /// <summary>
    /// 設定値を検証し、有効な範囲にクランプする
    /// </summary>
    public void ValidateSettings()
    {
        m_meshResolution = Mathf.Clamp(m_meshResolution, 
            CourseDefaults.MeshGeneration.MIN_RESOLUTION, 
            CourseDefaults.MeshGeneration.MAX_RESOLUTION);
        m_segmentsPerCurve = Mathf.Clamp(m_segmentsPerCurve, 
            CourseDefaults.MeshGeneration.MIN_SEGMENTS_PER_CURVE, 
            CourseDefaults.MeshGeneration.MAX_SEGMENTS_PER_CURVE);
        m_minSegmentsPerCurve = Mathf.Clamp(m_minSegmentsPerCurve, 2, 16);
        m_maxSegmentsPerCurve = Mathf.Clamp(m_maxSegmentsPerCurve, 8, 64);
        m_curvatureThreshold = Mathf.Clamp(m_curvatureThreshold, 0.01f, 1f);
        m_roadThickness = Mathf.Clamp(m_roadThickness, 
            CourseDefaults.MeshGeneration.MIN_ROAD_THICKNESS, 
            CourseDefaults.MeshGeneration.MAX_ROAD_THICKNESS);
        
        // 最小・最大セグメント数の整合性チェック
        if (m_minSegmentsPerCurve >= m_maxSegmentsPerCurve)
        {
            m_maxSegmentsPerCurve = m_minSegmentsPerCurve + 4;
        }
    }
}