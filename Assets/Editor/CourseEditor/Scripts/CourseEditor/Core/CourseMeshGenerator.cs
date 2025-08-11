using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// スプライン曲線からレーシングコースのメッシュを生成するクラス
/// フェーズ3A: 基本メッシュ生成機能（固定幅・フラット）
/// </summary>
public static class CourseMeshGenerator
{
    /// <summary>
    /// 生成されたメッシュデータを格納する構造体
    /// </summary>
    public struct MeshData
    {
        public Vector3[] vertices;      // 頂点配列
        public int[] triangles;         // 三角形インデックス配列
        public Vector2[] uvs;           // UV座標配列
        public Vector3[] normals;       // 法線ベクトル配列
        
        public bool IsValid => vertices != null && vertices.Length > 0;
    }
    
    /// <summary>
    /// CourseDataからメッシュデータを生成
    /// </summary>
    /// <param name="courseData">コースデータ</param>
    /// <returns>生成されたメッシュデータ</returns>
    public static MeshData GenerateCourseMesh(CourseData courseData)
    {
        if (courseData == null || !courseData.IsValidCourse)
        {
            return new MeshData();
        }
        
        // 設定を取得
        CourseSettings settings = courseData.Settings;
        int segmentsPerCurve = settings.m_segmentsPerCurve;
        
        // スプライン曲線全体の頂点を生成（適応的セグメント分割対応）
        List<CourseVertex> courseVertices = GenerateCourseVertices(courseData);
        
        
        if (courseVertices.Count < 4) // 最低限の頂点数をチェック
        {
            return new MeshData();
        }
        
        // メッシュデータを構築
        MeshData result = BuildMeshData(courseVertices, settings);
        
        return result;
    }
    
    /// <summary>
    /// コースの頂点情報を格納する構造体
    /// </summary>
    private struct CourseVertex
    {
        public Vector3 m_centerPosition;  // 中心線の位置
        public Vector3 m_tangent;         // 接線方向
        public Vector3 m_normal;          // 法線方向（右向き）
        public float m_width;             // この点での道路幅
        public float m_distanceAlongCourse; // コース開始からの距離
        
        public CourseVertex(Vector3 center, Vector3 tan, Vector3 norm, float w, float dist)
        {
            m_centerPosition = center;
            m_tangent = tan.normalized;
            m_normal = norm.normalized;
            m_width = w;
            m_distanceAlongCourse = dist;
        }
        
        /// <summary>
        /// 左端の頂点位置を取得
        /// </summary>
        public Vector3 GetLeftPosition()
        {
            return m_centerPosition - m_normal * (m_width * 0.5f);
        }
        
        /// <summary>
        /// 右端の頂点位置を取得
        /// </summary>
        public Vector3 GetRightPosition()
        {
            return m_centerPosition + m_normal * (m_width * 0.5f);
        }
    }
    
    /// <summary>
    /// 適応的セグメント分割対応：スプライン曲線に沿ってコース頂点を生成
    /// </summary>
    /// <param name="courseData">コースデータ</param>
    /// <returns>生成されたコース頂点リスト</returns>
    private static List<CourseVertex> GenerateCourseVertices(CourseData courseData)
    {
        List<CourseVertex> vertices = new List<CourseVertex>();
        float totalDistance = 0f;
        
        int pointCount = courseData.PointCount;
        CourseSettings settings = courseData.Settings;
        
        // 各スプライン区間を処理
        // 閉じたループ: 全制御点分の区間（最後から最初への区間含む）
        // 開いたコース: 最後の制御点の1つ前まで（通常の処理）
        int segmentCount = settings.m_isClosedLoop ? pointCount : pointCount - 1;
        
        
        for (int i = 0; i < segmentCount; i++)
        {
            SplinePoint currentPoint = courseData.GetPoint(i);
            SplinePoint nextPoint;
            
            if (settings.m_isClosedLoop)
            {
                // 閉じたループの場合：最後の点から最初の点へ循環
                nextPoint = courseData.GetPoint((i + 1) % pointCount);
            }
            else
            {
                // 開いたコースの場合：次の制御点を取得
                nextPoint = courseData.GetPoint(i + 1);
            }
            
            // 適応的セグメント数を計算
            int segmentsForThisCurve;
            if (settings.m_useAdaptiveSegmentation)
            {
                segmentsForThisCurve = SplineMath.CalculateAdaptiveSegments(
                    currentPoint, nextPoint,
                    settings.m_minSegmentsPerCurve,
                    settings.m_maxSegmentsPerCurve,
                    settings.m_curvatureThreshold
                );
                
            }
            else
            {
                segmentsForThisCurve = settings.m_segmentsPerCurve;
            }
            
            
            // この区間のセグメントを生成
            // 開いたコースの最後の区間の場合、終点も含める
            bool isLastSegmentOfOpenCourse = !settings.m_isClosedLoop && (i == segmentCount - 1);
            int actualSegments = isLastSegmentOfOpenCourse ? segmentsForThisCurve + 1 : segmentsForThisCurve;
            
            for (int seg = 0; seg < actualSegments; seg++)
            {
                float t;
                if (isLastSegmentOfOpenCourse && seg == actualSegments - 1)
                {
                    // 開いたコースの最後の区間の最後のセグメントでは t = 1.0 を確実に設定
                    t = 1.0f;
                }
                else
                {
                    t = (float)seg / segmentsForThisCurve;
                }
                
                // スプライン上の位置と方向を計算
                Vector3 position = SplineMath.EvaluateSpline(currentPoint, nextPoint, t);
                Vector3 tangent;
                
                // 最後の頂点（t = 1.0）の場合、接線を安定的に計算
                if (isLastSegmentOfOpenCourse && seg == actualSegments - 1 && t >= 0.999f)
                {
                    // 通常のスプライン接線を計算し、それが無効な場合のみフォールバック
                    Vector3 splineTangent = SplineMath.EvaluateSplineTangent(currentPoint, nextPoint, t);
                    
                    if (splineTangent.magnitude > 0.001f)
                    {
                        tangent = splineTangent.normalized;
                    }
                    else
                    {
                        // スプライン接線が無効な場合は、少し手前の位置から計算
                        Vector3 nearEndPosition = SplineMath.EvaluateSpline(currentPoint, nextPoint, 0.95f);
                        tangent = (position - nearEndPosition).normalized;
                        
                        // それでも無効な場合は制御点間の方向を使用
                        if (tangent.magnitude < 0.001f)
                        {
                            tangent = (nextPoint.position - currentPoint.position).normalized;
                        }
                    }
                }
                else
                {
                    tangent = SplineMath.EvaluateSplineTangent(currentPoint, nextPoint, t);
                }
                
                // 接線の一貫性をチェック（前の頂点との角度差をチェック）
                if (vertices.Count > 0)
                {
                    Vector3 prevTangent = vertices[vertices.Count - 1].m_tangent;
                    float angleDiff = Vector3.Angle(prevTangent, tangent);
                    
                    // 角度差が90度以上の場合、接線方向が逆になっている可能性
                    if (angleDiff > 90f)
                    {
                        tangent = -tangent;
                    }
                }
                
                // バンク角を補間
                float banking = SplineMath.InterpolateBanking(currentPoint, nextPoint, t);
                
                // バンク角を考慮した法線方向を計算
                Vector3 normal = SplineMath.GetBankedNormal(tangent, Vector3.up, banking);
                
                // 道路幅を補間
                float width = SplineMath.InterpolateWidth(currentPoint, nextPoint, t);
                
                // コース頂点を作成
                CourseVertex courseVertex = new CourseVertex(position, tangent, normal, width, totalDistance);
                vertices.Add(courseVertex);
                
                // 距離を更新（簡易計算）
                if (vertices.Count > 1)
                {
                    totalDistance += Vector3.Distance(vertices[vertices.Count - 2].m_centerPosition, position);
                }
            }
        }
        
        // 閉じたコースの場合、最後の頂点と最初の頂点が重複する可能性があるため除去
        if (settings.m_isClosedLoop && vertices.Count > 0)
        {
            // 最後の頂点と最初の頂点が近すぎる場合は最後の頂点を削除
            CourseVertex lastVertex = vertices[vertices.Count - 1];
            CourseVertex firstVertex = vertices[0];
            float distance = Vector3.Distance(lastVertex.m_centerPosition, firstVertex.m_centerPosition);
            
            // 距離が非常に近い場合（1cm以下）は重複とみなして削除
            if (distance < 0.01f)
            {
                vertices.RemoveAt(vertices.Count - 1);
            }
        }
        
        return vertices;
    }
    
    /// <summary>
    /// コース頂点からメッシュデータを構築
    /// </summary>
    /// <param name="courseVertices">コース頂点リスト</param>
    /// <param name="settings">コース設定</param>
    /// <returns>構築されたメッシュデータ</returns>
    private static MeshData BuildMeshData(List<CourseVertex> courseVertices, CourseSettings settings)
    {
        int vertexCount = courseVertices.Count;
        
        // メッシュ頂点配列を準備（左右2頂点 × セグメント数）
        Vector3[] vertices = new Vector3[vertexCount * 2];
        Vector2[] uvs = new Vector2[vertexCount * 2];
        Vector3[] normals = new Vector3[vertexCount * 2];
        
        // 三角形インデックス配列を準備
        List<int> triangles = new List<int>();
        
        // 頂点とUV座標を生成
        for (int i = 0; i < vertexCount; i++)
        {
            CourseVertex courseVertex = courseVertices[i];
            
            // 左右の頂点位置を計算（道路面を少し下げて見やすくする）
            Vector3 leftPos = courseVertex.GetLeftPosition();
            Vector3 rightPos = courseVertex.GetRightPosition();
            
            // 道路面を0.1m下げる
            leftPos.y -= 0.1f;
            rightPos.y -= 0.1f;
            
            // 頂点配列に追加
            int leftIndex = i * 2;
            int rightIndex = i * 2 + 1;
            
            vertices[leftIndex] = leftPos;
            vertices[rightIndex] = rightPos;
            
            // バンク角を考慮した道路面の法線ベクトルを計算
            Vector3 surfaceNormal = CalculateRoadSurfaceNormal(courseVertex);
            normals[leftIndex] = surfaceNormal;
            normals[rightIndex] = surfaceNormal;
            
            // UV座標を計算
            float u = courseVertex.m_distanceAlongCourse / 10f; // 10mごとにテクスチャを繰り返し
            uvs[leftIndex] = new Vector2(0f, u);
            uvs[rightIndex] = new Vector2(1f, u);
        }
        
        // 三角形を生成（閉じたコース対応）
        int triangleCount = settings.m_isClosedLoop ? vertexCount : vertexCount - 1;
        
        for (int i = 0; i < triangleCount; i++)
        {
            int leftCurrent = i * 2;
            int rightCurrent = i * 2 + 1;
            
            // 次の頂点インデックス（閉じたコースの場合は循環）
            int nextIndex = settings.m_isClosedLoop ? (i + 1) % vertexCount : (i + 1);
            int leftNext = nextIndex * 2;
            int rightNext = nextIndex * 2 + 1;
            
            // 道路面の四角形を2つの三角形で構成
            // 三角形1: 左下 -> 右下 -> 左上
            triangles.Add(leftCurrent);
            triangles.Add(leftNext);
            triangles.Add(rightCurrent);

            // 三角形2: 右下 -> 右上 -> 左上
            triangles.Add(rightCurrent);
            triangles.Add(leftNext);
            triangles.Add(rightNext);
        }
        
        // メッシュデータを返す
        return new MeshData
        {
            vertices = vertices,
            triangles = triangles.ToArray(),
            uvs = uvs,
            normals = normals
        };
    }
    
    /// <summary>
    /// メッシュデータからUnityのMeshオブジェクトを作成
    /// </summary>
    /// <param name="meshData">メッシュデータ</param>
    /// <param name="meshName">メッシュ名</param>
    /// <returns>作成されたMeshオブジェクト</returns>
    public static Mesh CreateUnityMesh(MeshData meshData, string meshName = "CourseMesh")
    {
        if (!meshData.IsValid)
        {
            return null;
        }
        
        Mesh mesh = new Mesh();
        mesh.name = meshName;
        
        // メッシュデータを設定
        mesh.vertices = meshData.vertices;
        mesh.triangles = meshData.triangles;
        mesh.uv = meshData.uvs;
        mesh.normals = meshData.normals;
        
        // バウンディングボックスを再計算
        mesh.RecalculateBounds();
        
        // 法線が設定されていない場合は自動計算
        if (meshData.normals == null || meshData.normals.Length == 0)
        {
            mesh.RecalculateNormals();
        }
        
        return mesh;
    }
    
    /// <summary>
    /// メッシュの統計情報を取得
    /// </summary>
    /// <param name="meshData">メッシュデータ</param>
    /// <returns>統計情報の文字列</returns>
    public static string GetMeshStats(MeshData meshData)
    {
        if (!meshData.IsValid)
        {
            return "無効なメッシュデータ";
        }
        
        int vertexCount = meshData.vertices.Length;
        int triangleCount = meshData.triangles.Length / 3;
        
        return $"頂点数: {vertexCount}, 三角形数: {triangleCount}";
    }
    
    /// <summary>
    /// バンク角を考慮した道路面の法線ベクトルを計算
    /// </summary>
    /// <param name="courseVertex">コース頂点</param>
    /// <returns>道路面の法線ベクトル</returns>
    private static Vector3 CalculateRoadSurfaceNormal(CourseVertex courseVertex)
    {
        // 接線ベクトル（進行方向）
        Vector3 tangent = courseVertex.m_tangent.normalized;
        
        // 右方向ベクトル（バンク角が適用済み）
        Vector3 right = courseVertex.m_normal.normalized;
        
        // 道路面の法線は右方向と接線の外積で求まる（順序重要）
        Vector3 surfaceNormal = Vector3.Cross(right, tangent).normalized;
        
        // 法線が下向きになってしまった場合は反転
        if (Vector3.Dot(surfaceNormal, Vector3.up) < 0)
        {
            surfaceNormal = -surfaceNormal;
        }
        
        // 法線が正しい向きになっているか最終チェック
        // 上向き成分が少なすぎる場合は強制的に上向きに補正
        if (Vector3.Dot(surfaceNormal, Vector3.up) < 0.1f)
        {
            // 接線に垂直で上向きの法線を再計算
            Vector3 horizontalTangent = new Vector3(tangent.x, 0, tangent.z).normalized;
            if (horizontalTangent.magnitude > 0.001f)
            {
                surfaceNormal = Vector3.Cross(horizontalTangent, Vector3.right).normalized;
                if (Vector3.Dot(surfaceNormal, Vector3.up) < 0)
                {
                    surfaceNormal = -surfaceNormal;
                }
            }
            else
            {
                surfaceNormal = Vector3.up;
            }
        }
        
        return surfaceNormal;
    }
    
    /// <summary>
    /// メッシュデータの有効性をチェック
    /// </summary>
    /// <param name="meshData">チェックするメッシュデータ</param>
    /// <returns>エラーメッセージ（問題がなければnull）</returns>
    public static string ValidateMeshData(MeshData meshData)
    {
        if (meshData.vertices == null)
            return "頂点データがnullです";
        
        if (meshData.vertices.Length == 0)
            return "頂点データが空です";
        
        if (meshData.triangles == null)
            return "三角形データがnullです";
        
        if (meshData.triangles.Length % 3 != 0)
            return "三角形インデックスの数が3の倍数ではありません";
        
        if (meshData.uvs != null && meshData.uvs.Length != meshData.vertices.Length)
            return "UV座標の数が頂点数と一致しません";
        
        if (meshData.normals != null && meshData.normals.Length != meshData.vertices.Length)
            return "法線ベクトルの数が頂点数と一致しません";
        
        // 三角形インデックスの範囲チェック
        for (int i = 0; i < meshData.triangles.Length; i++)
        {
            if (meshData.triangles[i] < 0 || meshData.triangles[i] >= meshData.vertices.Length)
            {
                return $"三角形インデックス[{i}]が範囲外です: {meshData.triangles[i]}";
            }
        }
        
        return null; // エラーなし
    }
}