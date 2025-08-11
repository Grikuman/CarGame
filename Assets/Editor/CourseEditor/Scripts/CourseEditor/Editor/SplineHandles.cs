using UnityEngine;
using UnityEditor;

/// <summary>
/// Scene View上でのスプライン制御点操作を提供するハンドルクラス
/// フェーズ2B: Scene操作基盤
/// </summary>
public static class SplineHandles
{
    // ハンドルの表示設定
    private static readonly Color PointColor = Color.white;
    private static readonly Color SelectedPointColor = Color.yellow;
    private static readonly Color InTangentColor = Color.green;
    private static readonly Color OutTangentColor = Color.blue;
    private static readonly Color TangentLineColor = new Color(0.8f, 0.8f, 0.8f, 0.7f);
    private static readonly Color CurveColor = Color.red;
    private static readonly Color WidthGuideColor = new Color(0.5f, 0.5f, 1f, 0.3f);
    
    private static readonly float PointSize = 0.3f;
    private static readonly float TangentHandleSize = 0.4f;
    private static readonly int CurveResolution = 32;
    
    /// <summary>
    /// CourseDataの全スプラインハンドルを描画
    /// </summary>
    /// <param name="courseData">描画対象のコースデータ</param>
    /// <param name="selectedPointIndex">選択中の制御点インデックス（-1なら未選択）</param>
    /// <returns>変更された制御点インデックス（変更なしなら元の値）</returns>
    public static int DrawSplineHandles(CourseData courseData, int selectedPointIndex)
    {
        return DrawSplineHandles(courseData, selectedPointIndex, true, true, true);
    }
    
    /// <summary>
    /// CourseDataの全スプラインハンドルを描画（表示設定可能版）
    /// </summary>
    /// <param name="courseData">描画対象のコースデータ</param>
    /// <param name="selectedPointIndex">選択中の制御点インデックス（-1なら未選択）</param>
    /// <param name="showControlPoints">制御点を表示するかどうか</param>
    /// <param name="showTangentHandles">接線ハンドルを表示するかどうか</param>
    /// <param name="showCurves">曲線を表示するかどうか</param>
    /// <returns>変更された制御点インデックス（変更なしなら元の値）</returns>
    public static int DrawSplineHandles(CourseData courseData, int selectedPointIndex, bool showControlPoints, bool showTangentHandles, bool showCurves)
    {
        if (courseData == null || courseData.PointCount < 2)
            return selectedPointIndex;
        
        int newSelectedIndex = selectedPointIndex;
        
        // 曲線を描画
        if (showCurves)
        {
            DrawSplineCurves(courseData);
        }
        
        // 制御点とハンドルを描画・操作
        if (showControlPoints)
        {
            
            // 幅ガイドを描画
            if (selectedPointIndex >= 0)
            {
                DrawWidthGuides(courseData, selectedPointIndex);
            }
            
            for (int i = 0; i < courseData.PointCount; i++)
            {
                SplinePoint point = courseData.GetPoint(i);
                bool isSelected = (i == selectedPointIndex);
                
                // 制御点ハンドルを描画・操作
                if (DrawPointHandle(courseData, point, i, isSelected))
                {
                    newSelectedIndex = (newSelectedIndex == i) ? -1 : i;
                    GUI.changed = true;
                }
                
                // 選択された制御点の接線ハンドルを描画・操作
                if (isSelected && showTangentHandles)
                {
                    DrawTangentHandles(courseData, point, i);
                }
            }
        }
        
        return newSelectedIndex;
    }
    
    /// <summary>
    /// スプライン曲線を描画
    /// </summary>
    /// <param name="courseData">コースデータ</param>
    private static void DrawSplineCurves(CourseData courseData)
    {
        Handles.color = CurveColor;
        
        for (int i = 0; i < courseData.PointCount - 1; i++)
        {
            SplinePoint point0 = courseData.GetPoint(i);
            SplinePoint point1 = courseData.GetPoint(i + 1);
            
            // ベジェ曲線を分割して描画
            Vector3 previousPos = SplineMath.EvaluateSpline(point0, point1, 0f);
            
            for (int j = 1; j <= CurveResolution; j++)
            {
                float t = (float)j / CurveResolution;
                Vector3 currentPos = SplineMath.EvaluateSpline(point0, point1, t);
                Handles.DrawLine(previousPos, currentPos);
                previousPos = currentPos;
            }
        }
        
        // 閉じた曲線を描画（最後の点から最初の点へ）
        if (courseData.PointCount > 2)
        {
            SplinePoint lastPoint = courseData.GetPoint(courseData.PointCount - 1);
            SplinePoint firstPoint = courseData.GetPoint(0);
            
            Vector3 previousPos = SplineMath.EvaluateSpline(lastPoint, firstPoint, 0f);
            
            for (int j = 1; j <= CurveResolution; j++)
            {
                float t = (float)j / CurveResolution;
                Vector3 currentPos = SplineMath.EvaluateSpline(lastPoint, firstPoint, t);
                Handles.DrawLine(previousPos, currentPos);
                previousPos = currentPos;
            }
        }
    }
    
    /// <summary>
    /// 制御点の幅ガイドを描画
    /// </summary>
    /// <param name="courseData">コースデータ</param>
    /// <param name="pointIndex">制御点インデックス</param>
    private static void DrawWidthGuides(CourseData courseData, int pointIndex)
    {
        if (pointIndex < 0 || pointIndex >= courseData.PointCount)
            return;
        
        SplinePoint point = courseData.GetPoint(pointIndex);
        
        // 前後の制御点との関係から接線方向を取得
        Vector3 tangent = Vector3.forward;
        
        if (pointIndex > 0)
        {
            SplinePoint prevPoint = courseData.GetPoint(pointIndex - 1);
            tangent = SplineMath.EvaluateSplineTangent(prevPoint, point, 1f);
        }
        else if (pointIndex < courseData.PointCount - 1)
        {
            SplinePoint nextPoint = courseData.GetPoint(pointIndex + 1);
            tangent = SplineMath.EvaluateSplineTangent(point, nextPoint, 0f);
        }
        
        // 法線方向を計算
        Vector3 normal = SplineMath.GetBankedNormal(tangent, Vector3.up, point.banking);
        
        // 幅ガイドラインを描画
        Handles.color = WidthGuideColor;
        Vector3 leftPoint = point.position - normal * (point.width * 0.5f);
        Vector3 rightPoint = point.position + normal * (point.width * 0.5f);
        
        Handles.DrawLine(leftPoint, rightPoint);
        Handles.DrawWireCube(leftPoint, Vector3.one * 0.3f);
        Handles.DrawWireCube(rightPoint, Vector3.one * 0.3f);
        
        // 中心線を描画
        Handles.color = new Color(WidthGuideColor.r, WidthGuideColor.g, WidthGuideColor.b, 0.8f);
        Handles.DrawLine(point.position, point.position + tangent * 2f);
    }
    
    /// <summary>
    /// 制御点ハンドルを描画・操作
    /// </summary>
    /// <param name="courseData">コースデータ</param>
    /// <param name="point">制御点データ</param>
    /// <param name="index">制御点インデックス</param>
    /// <param name="isSelected">選択状態</param>
    /// <returns>クリックされたかどうか</returns>
    private static bool DrawPointHandle(CourseData courseData, SplinePoint point, int index, bool isSelected)
    {
        Handles.color = isSelected ? SelectedPointColor : PointColor;
        
        // 制御点の位置ハンドルを表示
        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(point.position, Quaternion.identity);
        bool positionChanged = EditorGUI.EndChangeCheck();
        
        if (positionChanged)
        {
            Undo.RecordObject(courseData, "スプライン制御点を移動");
            point.position = newPosition;
            
            // メッシュ自動更新を通知
            NotifyMeshUpdate();
        }
        
        // 制御点を球として描画
        float handleSize = HandleUtility.GetHandleSize(point.position) * PointSize;
        
        // クリック検出用の非表示ボタン
        bool clicked = Handles.Button(point.position, Quaternion.identity, handleSize, handleSize, Handles.SphereHandleCap);
        
        // 制御点の球を描画
        Handles.SphereHandleCap(0, point.position, Quaternion.identity, handleSize, EventType.Repaint);
        
        // ラベルを表示
        Handles.color = Color.white;
        Handles.Label(point.position + Vector3.up * handleSize, $"P{index}");
        
        return clicked;
    }
    
    /// <summary>
    /// 接線ハンドルを描画・操作
    /// </summary>
    /// <param name="courseData">コースデータ</param>
    /// <param name="point">制御点データ</param>
    /// <param name="index">制御点インデックス</param>
    private static void DrawTangentHandles(CourseData courseData, SplinePoint point, int index)
    {
        // 入力接線ハンドル
        Vector3 inTangentPos = point.position + point.inTangent;
        Vector3 outTangentPos = point.position + point.outTangent;
        
        float handleSize = HandleUtility.GetHandleSize(point.position) * TangentHandleSize;
        
        // 接線を線で表示（薄いグレー）
        Handles.color = TangentLineColor;
        Handles.DrawLine(point.position, inTangentPos);
        Handles.DrawLine(point.position, outTangentPos);
        
        // 入力接線ハンドル操作（緑色）
        Handles.color = InTangentColor;
        EditorGUI.BeginChangeCheck();
        Vector3 newInTangentPos = Handles.PositionHandle(inTangentPos, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(courseData, "入力接線を移動");
            point.inTangent = newInTangentPos - point.position;
            
            // メッシュ自動更新を通知
            NotifyMeshUpdate();
        }
        
        // 出力接線ハンドル操作（青色）
        Handles.color = OutTangentColor;
        EditorGUI.BeginChangeCheck();
        Vector3 newOutTangentPos = Handles.PositionHandle(outTangentPos, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(courseData, "出力接線を移動");
            point.outTangent = newOutTangentPos - point.position;
            
            // メッシュ自動更新を通知
            NotifyMeshUpdate();
        }
        
        // 接線ハンドルを異なる色で描画
        Handles.color = InTangentColor;
        Handles.CubeHandleCap(0, inTangentPos, Quaternion.identity, handleSize * 0.8f, EventType.Repaint);
        
        Handles.color = OutTangentColor;
        Handles.CubeHandleCap(0, outTangentPos, Quaternion.identity, handleSize * 0.8f, EventType.Repaint);
        
        // ラベルを表示
        Handles.color = Color.white;
        Handles.Label(inTangentPos + Vector3.up * handleSize * 0.5f, "入力");
        Handles.Label(outTangentPos + Vector3.up * handleSize * 0.5f, "出力");
    }
    
    /// <summary>
    /// 指定した制御点に Scene View のカメラをフォーカス
    /// </summary>
    /// <param name="point">フォーカス対象の制御点</param>
    public static void FocusOnPoint(SplinePoint point)
    {
        if (point == null) return;
        
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            sceneView.LookAt(point.position);
        }
    }
    
    /// <summary>
    /// Scene View上での簡易情報表示
    /// </summary>
    /// <param name="courseData">コースデータ</param>
    /// <param name="selectedPointIndex">選択中の制御点インデックス</param>
    public static void DrawSceneGUI(CourseData courseData, int selectedPointIndex)
    {
        if (courseData == null) return;
        
        // Scene View の左上に情報を表示
        Handles.BeginGUI();
        
        GUILayout.BeginArea(new Rect(10, 10, 250, 100));
        GUILayout.BeginVertical("box");
        
        GUILayout.Label($"コース: {courseData.Settings.m_courseName}", EditorStyles.boldLabel);
        GUILayout.Label($"制御点数: {courseData.PointCount}");
        
        if (selectedPointIndex >= 0)
        {
            SplinePoint selectedPoint = courseData.GetPoint(selectedPointIndex);
            GUILayout.Label($"選択中: 制御点 {selectedPointIndex}");
            GUILayout.Label($"位置: {selectedPoint.position}");
            GUILayout.Label($"幅: {selectedPoint.width:F1}m");
            GUILayout.Label($"バンク角: {selectedPoint.banking:F1}°");
        }
        else
        {
            GUILayout.Label("制御点が選択されていません");
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
        
        Handles.EndGUI();
    }
    
    /// <summary>
    /// ハンドル操作のヘルプテキストを表示
    /// </summary>
    public static void DrawHelpText()
    {
        Handles.BeginGUI();
        
        GUILayout.BeginArea(new Rect(10, Screen.height - 120, 300, 100));
        GUILayout.BeginVertical("box");
        
        GUILayout.Label("スプラインエディタ操作方法:", EditorStyles.boldLabel);
        GUILayout.Label("• 制御点をクリックで選択/選択解除");
        GUILayout.Label("• 制御点をドラッグで移動");
        GUILayout.Label("• 接線ハンドルをドラッグで曲線調整");
        GUILayout.Label("• 詳細設定はコースエディタウィンドウで");
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
        
        Handles.EndGUI();
    }
    
    /// <summary>
    /// メッシュ更新通知をCourseEditorWindowに送信
    /// </summary>
    private static void NotifyMeshUpdate()
    {
        // CourseEditorWindowのインスタンスを探して更新を通知
        var editorWindows = Resources.FindObjectsOfTypeAll<CourseEditorWindow>();
        if (editorWindows.Length > 0)
        {
            editorWindows[0].ForceUpdateMesh();
        }
    }
}