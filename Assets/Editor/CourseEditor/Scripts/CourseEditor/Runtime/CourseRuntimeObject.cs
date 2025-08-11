using UnityEngine;

/// <summary>
/// Play Mode中にコースメッシュを保持するためのRuntimeコンポーネント
/// </summary>
public class CourseRuntimeObject : MonoBehaviour
{
    [Header("コース情報")]
    [SerializeField] private string courseName = "";
    [SerializeField] private int controlPointCount = 0;
    [SerializeField] private bool isClosedLoop = false;
    
    [Header("メッシュ情報")]
    [SerializeField] private int vertexCount = 0;
    [SerializeField] private int triangleCount = 0;
    
    void Start()
    {
        // Play Mode開始時の初期化処理
    }
    
    void OnDestroy()
    {
        // クリーンアップ処理
    }
    
    /// <summary>
    /// コース情報を設定
    /// </summary>
    public void SetCourseInfo(string name, int pointCount, bool closed, int vertices, int triangles)
    {
        courseName = name;
        controlPointCount = pointCount;
        isClosedLoop = closed;
        vertexCount = vertices;
        triangleCount = triangles;
    }
    
    /// <summary>
    /// コース情報を取得
    /// </summary>
    public string GetCourseInfo()
    {
        return $"Course: {courseName} | Points: {controlPointCount} | Vertices: {vertexCount} | Triangles: {triangleCount} | Loop: {isClosedLoop}";
    }
}