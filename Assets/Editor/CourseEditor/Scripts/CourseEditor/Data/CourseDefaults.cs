using UnityEngine;

/// <summary>
/// レーシングコースエディタのデフォルト設定値
/// プロジェクト全体で使用される標準値を管理
/// </summary>
[System.Serializable]
public static class CourseDefaults
{
    // コース基本設定のデフォルト値
    public static class Course
    {
        public const string DEFAULT_NAME = "新しいコース";  //コース名
        public const string DEFAULT_DESCRIPTION = "";       //説明文
        public const bool DEFAULT_IS_CLOSED_LOOP = true;   // 閉じたコースにするか
    }
    
    // 制御点のデフォルト値
    public static class ControlPoint
    {
        public const float DEFAULT_WIDTH = 30f;              // デフォルト道路幅（メートル）
        public const float MIN_WIDTH = 2f;                   // 最小道路幅
        public const float MAX_WIDTH = 100f;                  // 最大道路幅
        
        public const float DEFAULT_BANKING = 0f;             // デフォルトバンク角（度）
        public const float MIN_BANKING = -45f;               // 最小バンク角
        public const float MAX_BANKING = 45f;                // 最大バンク角
        
        public const float DEFAULT_TANGENT_LENGTH = 5f;      // デフォルト接線長さ
        public const float MIN_TANGENT_LENGTH = 0.5f;        // 最小接線長さ
        public const float MAX_TANGENT_LENGTH = 20f;         // 最大接線長さ
    }
    
    // メッシュ生成のデフォルト値
    public static class MeshGeneration
    {
        public const float DEFAULT_RESOLUTION = 1f;          // デフォルトメッシュ解像度
        public const float MIN_RESOLUTION = 0.1f;            // 最小解像度
        public const float MAX_RESOLUTION = 2f;              // 最大解像度
        
        public const int DEFAULT_SEGMENTS_PER_CURVE = 16;    // デフォルトセグメント数
        public const int MIN_SEGMENTS_PER_CURVE = 4;         // 最小セグメント数
        public const int MAX_SEGMENTS_PER_CURVE = 32;        // 最大セグメント数
        
        public const bool DEFAULT_USE_ADAPTIVE = true;       // 適応的セグメント分割使用
        public const int DEFAULT_MIN_ADAPTIVE = 4;           // 適応的最小セグメント
        public const int DEFAULT_MAX_ADAPTIVE = 32;          // 適応的最大セグメント
        public const float DEFAULT_CURVATURE_THRESHOLD = 0.1f; // 曲率閾値
        
        public const float DEFAULT_ROAD_THICKNESS = 0.5f;    // デフォルト道路厚み
        public const float MIN_ROAD_THICKNESS = 0.1f;        // 最小道路厚み
        public const float MAX_ROAD_THICKNESS = 5f;          // 最大道路厚み
    }
    
    // 物理設定のデフォルト値
    public static class Physics
    {
        public const bool DEFAULT_GENERATE_COLLIDERS = true; // コライダー生成
        public const bool DEFAULT_CAST_SHADOWS = true;       // 影を落とす
        public const bool DEFAULT_RECEIVE_SHADOWS = true;    // 影を受ける
    }
    
    // エディタUIのデフォルト値
    public static class EditorUI
    {
        public const bool DEFAULT_AUTO_UPDATE = false;       // 自動更新
        public const bool DEFAULT_SHOW_PREVIEW = true;       // メッシュ表示
        public const bool DEFAULT_AUTO_GENERATE_ON_PLAY = true; // Play時自動生成
        public const bool DEFAULT_SHOW_SCENE_ELEMENTS = true;   // Scene要素表示
        public const bool DEFAULT_SHOW_SCENE_INFO = true;       // Scene情報表示
        public const bool DEFAULT_SHOW_HELP_TEXT = false;       // ヘルプテキスト表示
    }
    
    // コース品質チェック用の閾値
    public static class QualityCheck
    {
        public const float MIN_CURVE_RADIUS = 5f;            // 最小曲線半径（メートル）
        public const float MAX_GRADIENT = 0.25f;             // 最大勾配（25%）
        public const float MAX_WIDTH_CHANGE_RATE = 0.5f;     // 最大幅変化率（50%）
        public const float MIN_POINT_DISTANCE = 2f;          // 最小制御点間距離
    }
    
    /// <summary>
    /// デフォルト値でCourseSettingsを作成
    /// </summary>
    public static CourseSettings CreateDefaultSettings()
    {
        var settings = new CourseSettings();
        settings.m_courseName = Course.DEFAULT_NAME;
        settings.m_courseDescription = Course.DEFAULT_DESCRIPTION;
        settings.m_isClosedLoop = Course.DEFAULT_IS_CLOSED_LOOP;
        settings.m_meshResolution = MeshGeneration.DEFAULT_RESOLUTION;
        settings.m_segmentsPerCurve = MeshGeneration.DEFAULT_SEGMENTS_PER_CURVE;
        settings.m_useAdaptiveSegmentation = MeshGeneration.DEFAULT_USE_ADAPTIVE;
        settings.m_minSegmentsPerCurve = MeshGeneration.DEFAULT_MIN_ADAPTIVE;
        settings.m_maxSegmentsPerCurve = MeshGeneration.DEFAULT_MAX_ADAPTIVE;
        settings.m_curvatureThreshold = MeshGeneration.DEFAULT_CURVATURE_THRESHOLD;
        settings.m_roadThickness = MeshGeneration.DEFAULT_ROAD_THICKNESS;
        settings.m_generateColliders = Physics.DEFAULT_GENERATE_COLLIDERS;
        settings.m_castShadows = Physics.DEFAULT_CAST_SHADOWS;
        settings.m_receiveShadows = Physics.DEFAULT_RECEIVE_SHADOWS;
        return settings;
    }
    
    /// <summary>
    /// デフォルト値でSplinePointを作成
    /// </summary>
    public static SplinePoint CreateDefaultPoint(Vector3 position)
    {
        var point = new SplinePoint();
        point.position = position;
        point.width = ControlPoint.DEFAULT_WIDTH;
        point.banking = ControlPoint.DEFAULT_BANKING;
        
        // デフォルト接線（前後方向に適当な長さ）
        point.inTangent = Vector3.left * ControlPoint.DEFAULT_TANGENT_LENGTH;
        point.outTangent = Vector3.right * ControlPoint.DEFAULT_TANGENT_LENGTH;
        
        return point;
    }
}