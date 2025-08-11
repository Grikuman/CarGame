using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// レーシングコースエディタのメインウィンドウ
/// フェーズ2A: 最小限のエディタUI実装
/// フェーズ2B: Scene操作基盤追加
/// </summary>
public class CourseEditorWindow : EditorWindow
{
    // 現在編集中のコースデータ
    private CourseData currentCourseData;
    
    // UI状態管理
    private Vector2 m_scrollPosition;
    private Vector2 m_mainScrollPosition; // メイン画面全体のスクロール
    private bool m_showPointList = true;
    private bool m_showSettings = false;
    private int m_selectedPointIndex = -1;
    
    // Scene操作設定
    private bool m_enableSceneHandles = true;
    private bool m_showSceneInfo = true;
    private bool m_showHelpText = false;
    private bool m_showSceneElements = true;  // 制御点・接線・曲線の表示ON/OFF
    
    // メッシュ生成設定
    private bool m_showMeshGeneration = false;
    private GameObject m_previewMeshObject;
    private bool m_autoUpdateMesh = false;
    private bool m_showMeshPreview = true;
    private float m_lastUpdateTime = 0f;
    private const float UPDATE_DELAY = 0.1f; // 更新間隔（秒）
    
    // 設定変更監視用
    private CourseSettings m_lastKnownSettings;
    private int m_lastKnownPointCount;
    
    // ウィンドウ設定
    private const float MIN_WINDOW_WIDTH = 300f;
    private const float MIN_WINDOW_HEIGHT = 400f;
    
    // Play Mode自動生成設定
    private bool m_autoGenerateOnPlay = true;
    
    /// <summary>
    /// メニューからエディタウィンドウを開く
    /// </summary>
    [MenuItem("Racing/コースエディタ")]
    public static void ShowWindow()
    {
        CourseEditorWindow window = GetWindow<CourseEditorWindow>("コースエディタ仮");
        window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
        window.Show();
    }
    
    /// <summary>
    /// Play Mode状態変更時の処理
    /// </summary>
    /// <param name="state">Play Mode状態</param>
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // Play Mode開始直前に自動メッシュ生成
        if (state == PlayModeStateChange.ExitingEditMode && m_autoGenerateOnPlay)
        {
            if (currentCourseData != null)
            {
                GeneratePreviewMesh();
                
                // メッシュが生成された場合、追加の設定を適用
                if (m_previewMeshObject != null)
                {
                    SetupPlayModeComponents();
                }
            }
        }
        
        // Play Mode完全開始後にメッシュが存在するかチェック
        else if (state == PlayModeStateChange.EnteredPlayMode && m_autoGenerateOnPlay)
        {
            // 少し遅延してメッシュの存在確認
            EditorApplication.delayCall += () => {
                CheckAndRecreatePlayModeMesh();
            };
        }
    }
    
    /// <summary>
    /// Play Mode中のメッシュ存在チェックと再生成
    /// </summary>
    private void CheckAndRecreatePlayModeMesh()
    {
        if (currentCourseData == null || !m_autoGenerateOnPlay) return;
        
        // 既存のコースオブジェクトを検索
        string expectedName = $"Course_{currentCourseData.Settings.m_courseName}_PlayMode";
        GameObject existingCourse = GameObject.Find(expectedName);
        
        if (existingCourse == null)
        {
            // メッシュを再生成
            GeneratePreviewMesh();
            
            if (m_previewMeshObject != null)
            {
                SetupPlayModeComponents();
            }
        }
    }
    
    /// <summary>
    /// Play Mode用のコンポーネント設定
    /// </summary>
    private void SetupPlayModeComponents()
    {
        if (m_previewMeshObject == null) return;
        
        // Play Mode中も残るように設定
        DontDestroyOnLoad(m_previewMeshObject);
        
        // MeshFilter/MeshRendererの取得
        MeshFilter meshFilter = m_previewMeshObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = m_previewMeshObject.GetComponent<MeshRenderer>();
        
        // MeshColliderが存在しない場合は追加
        MeshCollider meshCollider = m_previewMeshObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = m_previewMeshObject.AddComponent<MeshCollider>();
            meshCollider.convex = false;
            
            // 物理マテリアルを適用
            if (currentCourseData.Settings.m_roadPhysicMaterial != null)
            {
                meshCollider.material = currentCourseData.Settings.m_roadPhysicMaterial;
            }
        }
        
        // Runtime情報コンポーネントを追加
        var runtimeComponent = m_previewMeshObject.GetComponent<CourseRuntimeObject>();
        if (runtimeComponent == null)
        {
            runtimeComponent = m_previewMeshObject.AddComponent<CourseRuntimeObject>();
        }
        
        // コース情報を設定
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Mesh mesh = meshFilter.sharedMesh;
            runtimeComponent.SetCourseInfo(
                currentCourseData.Settings.m_courseName,
                currentCourseData.PointCount,
                currentCourseData.Settings.m_isClosedLoop,
                mesh.vertexCount,
                mesh.triangles.Length / 3
            );
        }
        
        // オブジェクト名を分かりやすく設定
        m_previewMeshObject.name = $"Course_{currentCourseData.Settings.m_courseName}_PlayMode";
        
        // レイヤーを設定（Ground レイヤーなど）
        m_previewMeshObject.layer = LayerMask.NameToLayer("Default");
        
        // Runtime用にタグを設定
        m_previewMeshObject.tag = "Untagged";
    }
    
    /// <summary>
    /// 新規コースデータ作成メニュー
    /// </summary>
    [MenuItem("Racing/新規コースデータ作成", false, 1)]
    public static void CreateNewCourseFromMenu()
    {
        CourseEditorWindow window = GetWindow<CourseEditorWindow>("コースエディタ");
        window.CreateNewCourseData();
        window.Show();
    }
    
    ///// <summary>
    ///// ヘルプメニュー
    ///// </summary>
    //[MenuItem("Racing/ヘルプ", false, 100)]
    //public static void ShowHelpFromMenu()
    //{
    //    CourseEditorWindow window = GetWindow<CourseEditorWindow>("コースエディタ");
    //    window.ShowHelpDialog();
    //}
    
    /// <summary>
    /// ウィンドウが有効になった時の初期化
    /// </summary>
    void OnEnable()
    {
        // ウィンドウタイトルとアイコンを設定
        titleContent = new GUIContent("コースエディタ 仮", "レーシングコースエディタ - スプライン曲線ベースの3Dコース作成ツール");
        
        // Scene操作のイベントハンドラを登録
        SceneView.duringSceneGui += OnSceneGUI;
        
        // Play Mode状態変更イベントを監視
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        
        // 設定を読み込み
        m_autoGenerateOnPlay = EditorPrefs.GetBool("CourseEditor_AutoGenerateOnPlay", CourseDefaults.EditorUI.DEFAULT_AUTO_GENERATE_ON_PLAY);
        
        // 既存のCourseDataがあれば自動選択を試行
        if (currentCourseData == null)
        {
            TryAutoSelectCourseData();
        }
    }
    
    /// <summary>
    /// ウィンドウが無効になった時のクリーンアップ
    /// </summary>
    void OnDisable()
    {
        // Scene操作のイベントハンドラを解除
        SceneView.duringSceneGui -= OnSceneGUI;
        
        // Play Mode状態変更イベント監視を解除
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        
        // 設定を保存
        EditorPrefs.SetBool("CourseEditor_AutoGenerateOnPlay", m_autoGenerateOnPlay);
        
        // メッシュ更新コールバックを解除
        EditorApplication.update -= CheckForMeshUpdate;
        
        // プレビューメッシュをクリーンアップ
        CleanupPreviewMesh();
    }
    
    /// <summary>
    /// メインのGUI描画
    /// </summary>
    void OnGUI()
    {
        // ヘッダー描画
        DrawHeader();
        
        // ツールバー描画
        DrawToolbar();
        
        EditorGUILayout.Space();
        
        // CourseData選択・管理セクション
        DrawCourseDataSection();
        
        EditorGUILayout.Space();
        
        // メイン画面全体のスクロールビュー開始
        m_mainScrollPosition = EditorGUILayout.BeginScrollView(m_mainScrollPosition);
        
        // 現在のCourseDataがある場合のみコンテンツを表示
        if (currentCourseData != null)
        {
            // フォルダブルセクション
            DrawFoldableSections();
            
            EditorGUILayout.Space();
            
            // フッター情報
            DrawFooter();
        }
        else
        {
            // CourseDataが選択されていない場合の案内
            DrawNoCourseDataMessage();
        }
        
        // メイン画面全体のスクロールビュー終了
        EditorGUILayout.EndScrollView();
        
        // ステータスバー描画
        DrawStatusBar();
        
        // データが変更された場合の保存処理と自動更新
        if (GUI.changed && currentCourseData != null)
        {
            EditorUtility.SetDirty(currentCourseData);
            
            // 自動更新が有効で、プレビューメッシュが存在する場合は更新をスケジュール
            if (m_autoUpdateMesh && m_previewMeshObject != null)
            {
                ScheduleMeshUpdate();
            }
        }
        
        // 設定変更の監視
        if (currentCourseData != null && Event.current.type == EventType.Layout)
        {
            MonitorSettingsChanges();
        }
        
        // 定期的なメッシュ更新処理
        HandleScheduledMeshUpdate();
        
        // キーボードショートカット処理
        HandleKeyboardShortcuts();
    }
    
    /// <summary>
    /// ヘッダー部分の描画
    /// </summary>
    private void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal();
        
        GUILayout.Label("レーシングコースエディタ", EditorStyles.largeLabel);
        GUILayout.FlexibleSpace();
        
        // バージョン情報
        GUILayout.Label("v1.6", EditorStyles.miniLabel);
        
        EditorGUILayout.EndHorizontal();
        
        // 区切り線
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), Color.gray);
    }
    
    /// <summary>
    /// ツールバーの描画
    /// </summary>
    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        
        // ファイル操作
        if (GUILayout.Button("新規作成", EditorStyles.toolbarButton, GUILayout.Width(60)))
        {
            CreateNewCourseData();
        }
        
        if (GUILayout.Button("開く", EditorStyles.toolbarButton, GUILayout.Width(40)))
        {
            OpenCourseData();
        }
        
        GUI.enabled = currentCourseData != null;
        if (GUILayout.Button("保存", EditorStyles.toolbarButton, GUILayout.Width(40)))
        {
            SaveCourseData();
        }
        GUI.enabled = true;
        
        GUILayout.Space(10);
        
        // 制御点操作
        GUI.enabled = currentCourseData != null;
        if (GUILayout.Button("制御点追加", EditorStyles.toolbarButton, GUILayout.Width(70)))
        {
            AddNewControlPoint();
        }
        
        GUI.enabled = currentCourseData != null && m_selectedPointIndex >= 0 && currentCourseData.PointCount > 2;
        if (GUILayout.Button("制御点削除", EditorStyles.toolbarButton, GUILayout.Width(70)))
        {
            DeleteSelectedControlPoint();
        }
        GUI.enabled = true;
        
        GUILayout.Space(10);
        
        // メッシュ操作
        GUI.enabled = currentCourseData != null && currentCourseData.IsValidCourse;
        if (GUILayout.Button("メッシュ生成", EditorStyles.toolbarButton, GUILayout.Width(70)))
        {
            GeneratePreviewMesh();
        }
        
        GUI.enabled = m_previewMeshObject != null;
        if (GUILayout.Button("メッシュクリア", EditorStyles.toolbarButton, GUILayout.Width(80)))
        {
            CleanupPreviewMesh();
        }
        
        GUI.enabled = m_previewMeshObject != null;
        if (GUILayout.Button("プレハブ保存", EditorStyles.toolbarButton, GUILayout.Width(70)))
        {
            SaveMeshAsPrefab();
        }
        GUI.enabled = true;
        GUI.enabled = true;
        
        GUILayout.FlexibleSpace();
        
        // 自動更新トグル
        GUI.enabled = m_previewMeshObject != null;
        bool newAutoUpdate = GUILayout.Toggle(m_autoUpdateMesh, "自動更新", EditorStyles.toolbarButton, GUILayout.Width(60));
        if (newAutoUpdate != m_autoUpdateMesh)
        {
            m_autoUpdateMesh = newAutoUpdate;
            if (m_autoUpdateMesh && currentCourseData != null)
            {
                ResetSettingsMonitoring();
            }
        }
        GUI.enabled = true;
        
        // ヘルプボタン
        if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20)))
        {
            ShowHelpDialog();
        }
        
        EditorGUILayout.EndHorizontal();
    }
    
    /// <summary>
    /// ステータスバーの描画
    /// </summary>
    private void DrawStatusBar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        
        // 現在の状態表示
        if (currentCourseData != null)
        {
            string status = $"制御点: {currentCourseData.PointCount}個";
            
            if (m_selectedPointIndex >= 0)
            {
                status += $" | 選択: {m_selectedPointIndex}";
            }
            
            if (m_previewMeshObject != null)
            {
                MeshFilter meshFilter = m_previewMeshObject.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    status += $" | メッシュ: {meshFilter.sharedMesh.vertexCount}頂点, {meshFilter.sharedMesh.triangles.Length / 3}三角形";
                }
            }
            
            GUILayout.Label(status, EditorStyles.miniLabel);
        }
        else
        {
            GUILayout.Label("コースデータが選択されていません", EditorStyles.miniLabel);
        }
        
        GUILayout.FlexibleSpace();
        
        // 自動更新状態表示
        if (m_autoUpdateMesh)
        {
            GUILayout.Label("自動更新: ON", EditorStyles.miniLabel);
        }
        
        // パフォーマンス情報
        GUILayout.Label($"FPS: {Mathf.RoundToInt(1.0f / Time.unscaledDeltaTime)}", EditorStyles.miniLabel);
        
        EditorGUILayout.EndHorizontal();
    }
    
    /// <summary>
    /// CourseData選択・管理セクションの描画
    /// </summary>
    private void DrawCourseDataSection()
    {
        EditorGUILayout.LabelField("コースデータ", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        // CourseDataのオブジェクトフィールド
        CourseData newCourseData = EditorGUILayout.ObjectField(
            "現在のコース:", 
            currentCourseData, 
            typeof(CourseData), 
            false
        ) as CourseData;
        
        // CourseDataが変更された場合
        if (newCourseData != currentCourseData)
        {
            currentCourseData = newCourseData;
            m_selectedPointIndex = -1; // 選択をリセット
            
            // 設定監視をリセット
            ResetSettingsMonitoring();
        }
        
        // 新規作成ボタン
        if (GUILayout.Button("新規", GUILayout.Width(50)))
        {
            CreateNewCourseData();
        }
        
        EditorGUILayout.EndHorizontal();
        
        // 現在のCourseDataの基本情報を表示
        if (currentCourseData != null)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"コース名: {currentCourseData.Settings.m_courseName}");
            EditorGUILayout.LabelField($"制御点数: {currentCourseData.PointCount}");
            
            EditorGUILayout.EndVertical();
        }
    }
    
    /// <summary>
    /// フォルダブルセクションの描画
    /// </summary>
    private void DrawFoldableSections()
    {
        // Scene操作セクション
        DrawSceneOperationSection();
        
        EditorGUILayout.Space();
        
        // 制御点リストセクション
        m_showPointList = EditorGUILayout.Foldout(m_showPointList, "制御点", true);
        if (m_showPointList)
        {
            DrawPointListSection();
        }
        
        EditorGUILayout.Space();
        
        // 設定セクション
        m_showSettings = EditorGUILayout.Foldout(m_showSettings, "コース設定", true);
        if (m_showSettings)
        {
            DrawSettingsSection();
        }
        
        EditorGUILayout.Space();
        
        // メッシュ生成セクション
        m_showMeshGeneration = EditorGUILayout.Foldout(m_showMeshGeneration, "メッシュ生成", true);
        if (m_showMeshGeneration)
        {
            DrawMeshGenerationSection();
        }
    }
    
    /// <summary>
    /// 制御点リストセクションの描画
    /// </summary>
    private void DrawPointListSection()
    {
        EditorGUILayout.BeginVertical("box");

        // 固定高さの領域を確保してからスクロールビューを作成
        var rect = EditorGUILayout.BeginVertical(GUILayout.Height(250));
        m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition,
            GUILayout.Width(rect.width), GUILayout.Height(250));

        // 制御点のリスト表示
        for (int i = 0; i < currentCourseData.PointCount; i++)
        {
            DrawPointListItem(i);
        }

        // 制御点がない場合のメッセージ
        if (currentCourseData.PointCount == 0)
        {
            EditorGUILayout.LabelField("制御点がありません。デフォルトコースを作成するか、手動で制御点を追加してください。",
                EditorStyles.centeredGreyMiniLabel);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        // 制御点操作ボタン
        DrawPointOperationButtons();

        EditorGUILayout.EndVertical();
    }
    
    /// <summary>
    /// 制御点リストアイテムの描画
    /// </summary>
    /// <param name="index">制御点のインデックス</param>
    private void DrawPointListItem(int index)
    {
        SplinePoint point = currentCourseData.GetPoint(index);
        if (point == null) return;
        
        // 選択状態の背景色
        Color originalColor = GUI.backgroundColor;
        if (m_selectedPointIndex == index)
        {
            GUI.backgroundColor = Color.cyan;
        }
        
        // 制御点の情報を表示（コンパクトに表示）
        string pointInfo = $"制御点 {index}: ({point.position.x:F1}, {point.position.y:F1}, {point.position.z:F1}) 幅:{point.width:F1}m バンク:{point.banking:F1}°";
        
        if (GUILayout.Button(pointInfo, EditorStyles.miniButton, GUILayout.Height(25)))
        {
            m_selectedPointIndex = (m_selectedPointIndex == index) ? -1 : index;
        }
        
        GUI.backgroundColor = originalColor;
        
        // 制御点操作ボタン（選択時のみ表示）
        if (m_selectedPointIndex == index)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15); // インデント縮小
            
            if (GUILayout.Button("前挿入", GUILayout.Height(20), GUILayout.Width(50)))
            {
                InsertControlPointAt(index);
            }
            
            if (GUILayout.Button("後挿入", GUILayout.Height(20), GUILayout.Width(50)))
            {
                InsertControlPointAt(index + 1);
            }
            
            GUI.enabled = currentCourseData.PointCount > 2;
            if (GUILayout.Button("削除", GUILayout.Height(20), GUILayout.Width(40)))
            {
                DeleteSelectedControlPoint();
            }
            GUI.enabled = true;
            
            // Scene Viewにフォーカスボタンを追加
            if (GUILayout.Button("Focus", GUILayout.Height(20), GUILayout.Width(45)))
            {
                SplineHandles.FocusOnPoint(point);
            }
            
            EditorGUILayout.EndHorizontal();
            
            // 選択された制御点の詳細表示（コンパクト版）
            DrawSelectedPointDetailsCompact(index, point);
        }
    }
    
    
    /// <summary>
    /// 選択された制御点の詳細情報表示（コンパクト版）
    /// </summary>
    /// <param name="index">制御点のインデックス</param>
    /// <param name="point">制御点データ</param>
    private void DrawSelectedPointDetailsCompact(int index, SplinePoint point)
    {
        EditorGUILayout.BeginVertical("helpbox");
        
        // 位置設定（1行で表示）
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("位置", GUILayout.Width(30));
        point.position = EditorGUILayout.Vector3Field("", point.position);
        EditorGUILayout.EndHorizontal();
        
        // 道路設定（スライダーをコンパクトに）
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("幅", GUILayout.Width(20));
        point.width = EditorGUILayout.Slider(point.width, 
            CourseDefaults.ControlPoint.MIN_WIDTH, 
            CourseDefaults.ControlPoint.MAX_WIDTH);
        EditorGUILayout.LabelField($"{point.width:F1}m", GUILayout.Width(35));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("バンク", GUILayout.Width(35));
        point.banking = EditorGUILayout.Slider(point.banking, 
            CourseDefaults.ControlPoint.MIN_BANKING, 
            CourseDefaults.ControlPoint.MAX_BANKING);
        EditorGUILayout.LabelField($"{point.banking:F1}°", GUILayout.Width(35));
        EditorGUILayout.EndHorizontal();
        
        // 接線ハンドル設定を追加
        DrawTangentHandleSettings(point);
        
        EditorGUILayout.EndVertical();
    }
    
    /// <summary>
    /// 接線ハンドル設定の描画
    /// </summary>
    /// <param name="point">制御点データ</param>
    private void DrawTangentHandleSettings(SplinePoint point)
    {
        EditorGUILayout.Space(5);
        
        // 接線設定の折りたたみ
        bool showTangents = EditorGUILayout.Foldout(
            EditorPrefs.GetBool("CourseEditor_ShowTangentHandles", false), 
            "接線ハンドル"
        );
        EditorPrefs.SetBool("CourseEditor_ShowTangentHandles", showTangents);
        
        if (showTangents)
        {
            EditorGUI.indentLevel++;
            
            // 入力接線の位置設定
            EditorGUILayout.LabelField("入力接線位置", EditorStyles.miniLabel);
            Vector3 inTangentWorldPos = point.position + point.inTangent;
            Vector3 newInTangentWorldPos = EditorGUILayout.Vector3Field("", inTangentWorldPos);
            if (newInTangentWorldPos != inTangentWorldPos)
            {
                point.inTangent = newInTangentWorldPos - point.position;
            }
            
            EditorGUILayout.Space(3);
            
            // 出力接線の位置設定
            EditorGUILayout.LabelField("出力接線位置", EditorStyles.miniLabel);
            Vector3 outTangentWorldPos = point.position + point.outTangent;
            Vector3 newOutTangentWorldPos = EditorGUILayout.Vector3Field("", outTangentWorldPos);
            if (newOutTangentWorldPos != outTangentWorldPos)
            {
                point.outTangent = newOutTangentWorldPos - point.position;
            }
            
            EditorGUILayout.Space(3);
            
            // 接線の長さ情報表示
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"入力接線長: {point.inTangent.magnitude:F2}", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"出力接線長: {point.outTangent.magnitude:F2}", EditorStyles.miniLabel);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(3);
            
            // 接線リセットボタン
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("接線を自動計算", GUILayout.Height(20)))
            {
                ResetTangentsForPoint(point);
            }
            if (GUILayout.Button("接線を対称化", GUILayout.Height(20)))
            {
                SymmetrizeTangents(point);
            }
            EditorGUILayout.EndHorizontal();
            
            // スマート接線調整ボタン
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("連続接線調整", GUILayout.Height(20)))
            {
                SmartTangentAdjustment(point);
            }
            if (GUILayout.Button("前後も含めて調整", GUILayout.Height(20)))
            {
                SmartTangentWithAdjacent(point);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel--;
        }
    }
    
    /// <summary>
    /// 制御点の接線を自動計算でリセット
    /// </summary>
    /// <param name="point">対象の制御点</param>
    private void ResetTangentsForPoint(SplinePoint point)
    {
        if (currentCourseData == null) return;
        
        Undo.RecordObject(currentCourseData, "接線を自動計算");
        
        // 制御点のインデックスを取得
        int pointIndex = -1;
        for (int i = 0; i < currentCourseData.PointCount; i++)
        {
            if (currentCourseData.GetPoint(i) == point)
            {
                pointIndex = i;
                break;
            }
        }
        
        if (pointIndex >= 0)
        {
            currentCourseData.CalculateAutoTangents(point, pointIndex, point.position);
            
            // メッシュを自動更新
            if (m_autoUpdateMesh && m_previewMeshObject != null)
            {
                ScheduleMeshUpdate();
            }
        }
    }
    
    /// <summary>
    /// 接線を対称化（出力接線を入力接線の反対向きに設定）
    /// </summary>
    /// <param name="point">対象の制御点</param>
    private void SymmetrizeTangents(SplinePoint point)
    {
        if (currentCourseData == null) return;
        
        Undo.RecordObject(currentCourseData, "接線を対称化");
        
        // 入力接線の反対方向を出力接線に設定
        float inLength = point.inTangent.magnitude;
        float outLength = point.outTangent.magnitude;
        float avgLength = (inLength + outLength) * 0.5f;
        
        Vector3 direction = point.inTangent.normalized;
        point.inTangent = direction * avgLength;
        point.outTangent = -direction * avgLength;
        
        // メッシュを自動更新
        if (m_autoUpdateMesh && m_previewMeshObject != null)
        {
            ScheduleMeshUpdate();
        }
    }
    
    /// <summary>
    /// スマート接線調整（選択した制御点のみ）
    /// </summary>
    /// <param name="point">対象の制御点</param>
    private void SmartTangentAdjustment(SplinePoint point)
    {
        if (currentCourseData == null) return;
        
        Undo.RecordObject(currentCourseData, "スマート接線調整");
        
        // 制御点のインデックスを取得
        int pointIndex = -1;
        for (int i = 0; i < currentCourseData.PointCount; i++)
        {
            if (currentCourseData.GetPoint(i) == point)
            {
                pointIndex = i;
                break;
            }
        }
        
        if (pointIndex >= 0)
        {
            currentCourseData.CalculateSmartTangents(point, pointIndex, point.position);
            
            // メッシュを自動更新
            if (m_autoUpdateMesh && m_previewMeshObject != null)
            {
                ScheduleMeshUpdate();
            }
        }
    }
    
    /// <summary>
    /// スマート接線調整（前後の制御点も含めて）
    /// </summary>
    /// <param name="point">対象の制御点</param>
    private void SmartTangentWithAdjacent(SplinePoint point)
    {
        if (currentCourseData == null) return;
        
        Undo.RecordObject(currentCourseData, "前後含むスマート接線調整");
        
        // 制御点のインデックスを取得
        int pointIndex = -1;
        for (int i = 0; i < currentCourseData.PointCount; i++)
        {
            if (currentCourseData.GetPoint(i) == point)
            {
                pointIndex = i;
                break;
            }
        }
        
        if (pointIndex >= 0)
        {
            currentCourseData.RecalculateAdjacentTangents(pointIndex);
            
            // メッシュを自動更新
            if (m_autoUpdateMesh && m_previewMeshObject != null)
            {
                ScheduleMeshUpdate();
            }
        }
    }
    
    /// <summary>
    /// 制御点操作ボタンの描画
    /// </summary>
    private void DrawPointOperationButtons()
    {
        // 制御点追加ボタン
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("制御点を末尾に追加", GUILayout.Height(30)))
        {
            AddNewControlPoint();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // コース操作ボタン行
        EditorGUILayout.BeginHorizontal();
        
        // デフォルトコース作成
        if (GUILayout.Button("デフォルトコース作成", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("デフォルトコース作成", 
                "現在のコースを削除してデフォルトコースを作成しますか？", 
                "はい", "キャンセル"))
            {
                Undo.RecordObject(currentCourseData, "デフォルトコース作成");
                currentCourseData.CreateDefaultStraightCourse();
                m_selectedPointIndex = -1;
                
                // メッシュを自動更新
                if (m_autoUpdateMesh && m_previewMeshObject != null)
                {
                    ScheduleMeshUpdate();
                }
            }
        }
        
        // 制御点をクリア
        if (GUILayout.Button("全制御点をクリア", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("全制御点をクリア", 
                "すべての制御点を削除してもよろしいですか？", 
                "はい", "キャンセル"))
            {
                Undo.RecordObject(currentCourseData, "全制御点をクリア");
                currentCourseData.ClearPoints();
                m_selectedPointIndex = -1;
                
                // メッシュをクリア
                CleanupPreviewMesh();
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // 全体の接線調整ボタン行
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("全接線をスマート調整", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("全接線調整", 
                "すべての制御点の接線を前後の位置から自動調整しますか？", 
                "はい", "キャンセル"))
            {
                Undo.RecordObject(currentCourseData, "全接線スマート調整");
                currentCourseData.RecalculateAllTangents();
                
                // メッシュを自動更新
                if (m_autoUpdateMesh && m_previewMeshObject != null)
                {
                    ScheduleMeshUpdate();
                }
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
    }
    
    /// <summary>
    /// 設定セクションの描画
    /// </summary>
    private void DrawSettingsSection()
    {
        EditorGUILayout.BeginVertical("box");
        
        CourseSettings settings = currentCourseData.Settings;
        
        // 基本設定
        EditorGUILayout.LabelField("基本設定", EditorStyles.boldLabel);
        settings.m_courseName = EditorGUILayout.TextField("コース名", settings.m_courseName);
        settings.m_courseDescription = EditorGUILayout.TextField("説明", settings.m_courseDescription);
        
        bool newIsClosedLoop = EditorGUILayout.Toggle("閉じたループコース", settings.m_isClosedLoop);
        if (newIsClosedLoop != settings.m_isClosedLoop)
        {
            settings.m_isClosedLoop = newIsClosedLoop;
            // ループ設定変更時に接線を再計算
            currentCourseData.RecalculateAllTangents();
            EditorUtility.SetDirty(currentCourseData);
        }
        
        EditorGUILayout.Space();
        
        // メッシュ設定
        EditorGUILayout.LabelField("メッシュ設定", EditorStyles.boldLabel);
        settings.m_meshResolution = EditorGUILayout.Slider("解像度", settings.m_meshResolution, 0.1f, 2f);
        
        // 適応的セグメント分割設定
        EditorGUILayout.Space();
        settings.m_useAdaptiveSegmentation = EditorGUILayout.Toggle("適応的セグメント分割", settings.m_useAdaptiveSegmentation);
        
        if (settings.m_useAdaptiveSegmentation)
        {
            EditorGUI.indentLevel++;
            settings.m_minSegmentsPerCurve = EditorGUILayout.IntSlider("最小セグメント数 (直線)", settings.m_minSegmentsPerCurve, 2, 16);
            settings.m_maxSegmentsPerCurve = EditorGUILayout.IntSlider("最大セグメント数 (急カーブ)", settings.m_maxSegmentsPerCurve, 8, 64);
            settings.m_curvatureThreshold = EditorGUILayout.Slider("曲率閾値", settings.m_curvatureThreshold, 0.01f, 1f);
            
            EditorGUI.indentLevel--;
        }
        else
        {
            settings.m_segmentsPerCurve = EditorGUILayout.IntSlider("曲線あたりのセグメント数", settings.m_segmentsPerCurve, 4, 32);
        }
        
        EditorGUILayout.Space();
        
        // 物理設定
        EditorGUILayout.LabelField("物理設定", EditorStyles.boldLabel);
        settings.m_generateColliders = EditorGUILayout.Toggle("メッシュコライダー生成", settings.m_generateColliders);
        
        if (settings.m_generateColliders)
        {
            EditorGUI.indentLevel++;
            settings.m_roadPhysicMaterial = EditorGUILayout.ObjectField("物理マテリアル", settings.m_roadPhysicMaterial, typeof(PhysicsMaterial), false) as PhysicsMaterial;
            EditorGUI.indentLevel--;
            
        }
        
        EditorGUILayout.EndVertical();
    }
    
    /// <summary>
    /// フッター情報の描画
    /// </summary>
    private void DrawFooter()
    {
        EditorGUILayout.BeginHorizontal();
        
        GUILayout.FlexibleSpace();
        
        if (currentCourseData != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(currentCourseData);
            if (!string.IsNullOrEmpty(assetPath))
            {
                GUILayout.Label($"アセット: {assetPath}", EditorStyles.miniLabel);
            }
            else
            {
                GUILayout.Label("未保存のコースデータ", EditorStyles.miniLabel);
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }
    
    /// <summary>
    /// CourseDataが未選択時のメッセージ表示
    /// </summary>
    private void DrawNoCourseDataMessage()
    {
        EditorGUILayout.BeginVertical("box");
        
        EditorGUILayout.LabelField("コースデータが選択されていません", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("既存のコースデータアセットを選択するか、新しいものを作成してください。", EditorStyles.wordWrappedMiniLabel);
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("新しいコースデータを作成"))
        {
            CreateNewCourseData();
        }
        
        EditorGUILayout.EndVertical();
    }
    
    /// <summary>
    /// 新しいCourseDataを作成
    /// </summary>
    private void CreateNewCourseData()
    {
        // 新しいCourseDataインスタンスを作成
        CourseData newCourseData = ScriptableObject.CreateInstance<CourseData>();
        newCourseData.CreateDefaultStraightCourse();
        
        // アセットとして保存
        string path = EditorUtility.SaveFilePanelInProject(
            "新しいコースデータを保存",
            "NewCourse",
            "asset",
            "新しいコースデータの保存場所を選択してください"
        );
        
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(newCourseData, path);
            AssetDatabase.SaveAssets();
            
            currentCourseData = newCourseData;
            m_selectedPointIndex = -1;
            
        }
        else
        {
            // キャンセルされた場合は一時的なオブジェクトとして使用
            currentCourseData = newCourseData;
            m_selectedPointIndex = -1;
        }
    }
    
    /// <summary>
    /// 既存のCourseDataの自動選択を試行
    /// </summary>
    private void TryAutoSelectCourseData()
    {
        // プロジェクト内の最初のCourseDataアセットを検索
        string[] guids = AssetDatabase.FindAssets("t:CourseData");
        
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            CourseData foundCourseData = AssetDatabase.LoadAssetAtPath<CourseData>(path);
            
            if (foundCourseData != null)
            {
                currentCourseData = foundCourseData;
            }
        }
    }
    
    /// <summary>
    /// Scene操作セクションの描画
    /// </summary>
    private void DrawSceneOperationSection()
    {
        EditorGUILayout.LabelField("Scene操作", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginVertical("box");
        
        // Scene操作の有効/無効切り替え
        m_enableSceneHandles = EditorGUILayout.Toggle("Sceneハンドル有効", m_enableSceneHandles);
        m_showSceneInfo = EditorGUILayout.Toggle("Scene情報表示", m_showSceneInfo);
        m_showHelpText = EditorGUILayout.Toggle("ヘルプテキスト表示", m_showHelpText);
        m_showSceneElements = EditorGUILayout.Toggle("制御点・曲線表示", m_showSceneElements);
        
        EditorGUILayout.Space();
        
        // 選択された制御点の情報表示と操作
        if (m_selectedPointIndex >= 0 && m_selectedPointIndex < currentCourseData.PointCount)
        {
            SplinePoint selectedPoint = currentCourseData.GetPoint(m_selectedPointIndex);
            EditorGUILayout.LabelField($"選択中の制御点: {m_selectedPointIndex}", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Sceneでフォーカス"))
            {
                SplineHandles.FocusOnPoint(selectedPoint);
            }
            if (GUILayout.Button("選択解除"))
            {
                m_selectedPointIndex = -1;
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.LabelField("制御点が選択されていません");
            EditorGUILayout.LabelField("Scene Viewで制御点をクリックして選択してください", EditorStyles.miniLabel);
        }
        
        
        EditorGUILayout.EndVertical();
    }
    
    /// <summary>
    /// Scene View上でのGUI描画処理
    /// </summary>
    /// <param name="sceneView">Scene Viewインスタンス</param>
    private void OnSceneGUI(SceneView sceneView)
    {
        if (currentCourseData == null || !m_enableSceneHandles)
            return;
        
        // スプラインハンドルを描画・操作（表示設定に応じて）
        int newSelectedIndex = SplineHandles.DrawSplineHandles(
            currentCourseData, 
            m_selectedPointIndex, 
            m_showSceneElements,  // 制御点表示
            m_showSceneElements,  // 接線ハンドル表示
            m_showSceneElements   // 曲線表示
        );
        
        // 選択状態の変更を直接チェック
        bool selectionChanged = (newSelectedIndex != m_selectedPointIndex);
        
        if (selectionChanged)
        {
            m_selectedPointIndex = newSelectedIndex;
            
            // 制御点が選択された場合、制御点リストを自動で開く
            if (m_selectedPointIndex >= 0)
            {
                m_showPointList = true;
                
                // 接線ハンドルパネルも自動で開く
                EditorPrefs.SetBool("CourseEditor_ShowTangentHandles", true);
                
                // 制御点リストの該当項目までスクロール
                // 制御点1つあたりの高さを25px、詳細パネルを含めると約150pxと仮定
                float itemHeight = 25f;
                float detailPanelHeight = 150f;
                float targetScrollY = m_selectedPointIndex * itemHeight;
                
                // 詳細パネルが見えるように少し上にオフセット
                targetScrollY = Mathf.Max(0, targetScrollY - detailPanelHeight * 0.5f);
                m_scrollPosition.y = targetScrollY;
                
                // メイン画面も制御点セクションが見えるように調整
                // 制御点セクションは通常上部にあるので、少し上にスクロール
                m_mainScrollPosition.y = Mathf.Max(0, m_mainScrollPosition.y - 100f);
            }
            
            Repaint(); // ウィンドウを再描画
        }
        
        // Scene上の情報表示
        if (m_showSceneInfo)
        {
            SplineHandles.DrawSceneGUI(currentCourseData, m_selectedPointIndex);
        }
        
        // ヘルプテキスト表示
        if (m_showHelpText)
        {
            SplineHandles.DrawHelpText();
        }
        
        // データが変更された場合は保存をマーク
        if (GUI.changed)
        {
            EditorUtility.SetDirty(currentCourseData);
            SceneView.RepaintAll();
        }
    }
    
    /// <summary>
    /// メッシュ生成セクションの描画
    /// </summary>
    private void DrawMeshGenerationSection()
    {
        EditorGUILayout.BeginVertical("box");
        
        EditorGUILayout.LabelField("メッシュプレビュー", EditorStyles.boldLabel);
        
        // メッシュ生成ボタン
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("メッシュ生成"))
        {
            GeneratePreviewMesh();
        }
        
        if (GUILayout.Button("プレビュークリア"))
        {
            CleanupPreviewMesh();
        }
        
        GUI.enabled = m_previewMeshObject != null;
        if (GUILayout.Button("プレハブとして保存"))
        {
            SaveMeshAsPrefab();
        }
        GUI.enabled = true;
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // 自動更新設定
        EditorGUILayout.BeginHorizontal();
        bool newAutoUpdate = EditorGUILayout.Toggle("自動更新", m_autoUpdateMesh);
        bool newShowPreview = EditorGUILayout.Toggle("メッシュ表示", m_showMeshPreview);
        EditorGUILayout.EndHorizontal();
        
        // Play Mode自動生成設定
        bool newAutoGenerateOnPlay = EditorGUILayout.Toggle("Play時自動生成", m_autoGenerateOnPlay);
        if (newAutoGenerateOnPlay != m_autoGenerateOnPlay)
        {
            m_autoGenerateOnPlay = newAutoGenerateOnPlay;
            EditorPrefs.SetBool("CourseEditor_AutoGenerateOnPlay", m_autoGenerateOnPlay);
        }
        
        // ヘルプテキスト
        GUIStyle helpStyle = new GUIStyle(EditorStyles.miniLabel);
        helpStyle.normal.textColor = Color.gray;
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Playボタン押下時に自動でメッシュ生成", helpStyle);
        EditorGUI.indentLevel--;
        
        // 自動更新設定が変更された場合の処理
        if (newAutoUpdate != m_autoUpdateMesh)
        {
            m_autoUpdateMesh = newAutoUpdate;
            if (m_autoUpdateMesh && m_previewMeshObject != null && currentCourseData != null)
            {
                // 自動更新を有効にした時、設定監視を初期化
                ResetSettingsMonitoring();
            }
        }
        
        // メッシュ表示設定が変更された場合の処理
        if (newShowPreview != m_showMeshPreview)
        {
            m_showMeshPreview = newShowPreview;
        }
        
        // メッシュ表示の切り替え
        if (m_previewMeshObject != null)
        {
            MeshRenderer meshRenderer = m_previewMeshObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null && meshRenderer.enabled != m_showMeshPreview)
            {
                meshRenderer.enabled = m_showMeshPreview;
                SceneView.RepaintAll(); // Scene Viewを即座に更新
            }
        }
        
        EditorGUILayout.Space();
        
        // メッシュ情報表示
        if (m_previewMeshObject != null)
        {
            MeshFilter meshFilter = m_previewMeshObject.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                Mesh mesh = meshFilter.sharedMesh;
                
                EditorGUILayout.BeginVertical("helpbox");
                EditorGUILayout.LabelField("生成されたメッシュ情報:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"頂点数: {mesh.vertexCount}");
                EditorGUILayout.LabelField($"三角形数: {mesh.triangles.Length / 3}");
                
                // コライダー情報も表示
                MeshCollider meshCollider = m_previewMeshObject.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    EditorGUILayout.LabelField($"メッシュコライダー: あり");
                    if (meshCollider.material != null)
                    {
                        EditorGUILayout.LabelField($"物理マテリアル: {meshCollider.material.name}");
                    }
                }
                else
                {
                    EditorGUILayout.LabelField($"メッシュコライダー: なし");
                }
                EditorGUILayout.LabelField($"バウンズ: {mesh.bounds.size}");
                EditorGUILayout.EndVertical();
            }
        }
        else
        {
            EditorGUILayout.LabelField("プレビューメッシュなし", EditorStyles.centeredGreyMiniLabel);
        }
        
        
        EditorGUILayout.EndVertical();
    }
    
    /// <summary>
    /// プレビューメッシュを生成
    /// </summary>
    private void GeneratePreviewMesh()
    {
        if (currentCourseData == null || !currentCourseData.IsValidCourse)
        {
            EditorUtility.DisplayDialog("エラー", "有効なコースデータが選択されていません。\n最低2つ以上の制御点が必要です。", "OK");
            return;
        }
        
        try
        {
            // 既存のプレビューメッシュをクリーンアップ
            CleanupPreviewMesh();
            
            // メッシュデータを生成
            CourseMeshGenerator.MeshData meshData = CourseMeshGenerator.GenerateCourseMesh(currentCourseData);
            
            // メッシュデータの有効性をチェック
            string validationError = CourseMeshGenerator.ValidateMeshData(meshData);
            if (!string.IsNullOrEmpty(validationError))
            {
                EditorUtility.DisplayDialog("メッシュ生成エラー", $"メッシュデータに問題があります:\n{validationError}", "OK");
                return;
            }
            
            // UnityのMeshオブジェクトを作成
            Mesh unityMesh = CourseMeshGenerator.CreateUnityMesh(meshData, currentCourseData.Settings.m_courseName + "_Preview");
            
            if (unityMesh == null)
            {
                EditorUtility.DisplayDialog("エラー", "メッシュの作成に失敗しました。", "OK");
                return;
            }
            
            // プレビュー用GameObjectを作成
            m_previewMeshObject = new GameObject("Course Preview Mesh");
            m_previewMeshObject.hideFlags = HideFlags.DontSave; // シーンに保存されないようにする
            
            // MeshFilterとMeshRendererを追加
            MeshFilter meshFilter = m_previewMeshObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = m_previewMeshObject.AddComponent<MeshRenderer>();
            
            meshFilter.mesh = unityMesh;
            
            // コライダー生成が有効な場合のみMeshColliderを追加
            if (currentCourseData.Settings.m_generateColliders)
            {
                MeshCollider meshCollider = m_previewMeshObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = unityMesh;
                meshCollider.convex = false; // 複雑なメッシュの場合はfalse
                
                // 物理マテリアルを適用
                if (currentCourseData.Settings.m_roadPhysicMaterial != null)
                {
                    meshCollider.material = currentCourseData.Settings.m_roadPhysicMaterial;
                }
            }
            
            // デフォルトマテリアルを設定
            Material defaultMaterial = currentCourseData.Settings.m_roadMaterial;
            if (defaultMaterial == null)
            {
                // Standard シェーダーでデフォルトマテリアルを作成
                defaultMaterial = new Material(Shader.Find("Standard"));
                defaultMaterial.color = new Color(0.2f, 0.2f, 0.2f, 1f); // 暗いグレー（コントラストを高める）
                defaultMaterial.SetFloat("_Metallic", 0f);
                defaultMaterial.SetFloat("_Glossiness", 0.1f);
                defaultMaterial.name = "Default Road Material";
            }
            
            meshRenderer.material = defaultMaterial;
            
            // Scene ViewとHierarchyを更新
            SceneView.RepaintAll();
            EditorApplication.DirtyHierarchyWindowSorting();
            
        }
        catch (System.Exception ex)
        {
            EditorUtility.DisplayDialog("メッシュ生成エラー", $"メッシュ生成中にエラーが発生しました:\n{ex.Message}", "OK");
            // エラーはダイアログで表示するためコンソールログは省略
        }
    }
    
    /// <summary>
    /// プレビューメッシュをクリーンアップ
    /// </summary>
    private void CleanupPreviewMesh()
    {
        if (m_previewMeshObject != null)
        {
            // メッシュリソースをクリーンアップ
            MeshFilter meshFilter = m_previewMeshObject.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                DestroyImmediate(meshFilter.sharedMesh);
            }
            
            // GameObjectを削除
            DestroyImmediate(m_previewMeshObject);
            m_previewMeshObject = null;
            
            // Scene Viewを更新
            SceneView.RepaintAll();
            EditorApplication.DirtyHierarchyWindowSorting();
        }
    }
    
    /// <summary>
    /// メッシュ更新をスケジュール
    /// </summary>
    private void ScheduleMeshUpdate()
    {
        // 既にスケジュールされている場合は時間のみ更新
        m_lastUpdateTime = (float)EditorApplication.timeSinceStartup + UPDATE_DELAY;
        
        // 重複してコールバックを登録しないようにチェック
        EditorApplication.update -= CheckForMeshUpdate;
        EditorApplication.update += CheckForMeshUpdate;
    }
    
    /// <summary>
    /// スケジュールされたメッシュ更新処理
    /// </summary>
    private void HandleScheduledMeshUpdate()
    {
        // この処理は空にして、CheckForMeshUpdateで実際の更新を行う
    }
    
    /// <summary>
    /// メッシュ更新のチェック（EditorApplicationのupdateコールバック）
    /// </summary>
    private void CheckForMeshUpdate()
    {
        if (EditorApplication.timeSinceStartup >= m_lastUpdateTime)
        {
            EditorApplication.update -= CheckForMeshUpdate;
            
            // メッシュが存在し、自動更新が有効な場合のみ更新
            if (m_previewMeshObject != null && m_autoUpdateMesh && currentCourseData != null)
            {
                UpdatePreviewMeshSilent();
            }
        }
    }
    
    /// <summary>
    /// プレビューメッシュをサイレント更新（エラーダイアログなし）
    /// </summary>
    private void UpdatePreviewMeshSilent()
    {
        if (currentCourseData == null || !currentCourseData.IsValidCourse)
        {
            return;
        }

        try
        {
            // メッシュデータを生成
            CourseMeshGenerator.MeshData meshData = CourseMeshGenerator.GenerateCourseMesh(currentCourseData);
            
            if (!meshData.IsValid)
            {
                return;
            }
            
            // 既存のメッシュを更新
            if (m_previewMeshObject != null)
            {
                MeshFilter meshFilter = m_previewMeshObject.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    // 既存のメッシュを削除
                    if (meshFilter.sharedMesh != null)
                    {
                        DestroyImmediate(meshFilter.sharedMesh);
                    }
                    
                    // 新しいメッシュを作成・設定
                    Mesh newMesh = CourseMeshGenerator.CreateUnityMesh(meshData, currentCourseData.Settings.m_courseName + "_Preview");
                    meshFilter.mesh = newMesh;
                    
                    // Scene Viewを更新
                    SceneView.RepaintAll();
                }
            }
        }
        catch (System.Exception)
        {
           
        }
    }
    
    /// <summary>
    /// 手動でメッシュ更新を実行
    /// </summary>
    public void ForceUpdateMesh()
    {
        if (m_previewMeshObject != null && currentCourseData != null)
        {
            UpdatePreviewMeshSilent();
        }
    }
    
    /// <summary>
    /// 設定変更を監視してメッシュを自動更新
    /// </summary>
    private void MonitorSettingsChanges()
    {
        if (currentCourseData == null || !m_autoUpdateMesh || m_previewMeshObject == null)
            return;
        
        bool settingsChanged = false;
        
        // 制御点数の変更をチェック
        if (m_lastKnownPointCount != currentCourseData.PointCount)
        {
            m_lastKnownPointCount = currentCourseData.PointCount;
            settingsChanged = true;
        }
        
        // 設定の変更をチェック
        if (m_lastKnownSettings != null)
        {
            CourseSettings current = currentCourseData.Settings;
            
            if (m_lastKnownSettings.m_meshResolution != current.m_meshResolution ||
                m_lastKnownSettings.m_segmentsPerCurve != current.m_segmentsPerCurve ||
                m_lastKnownSettings.m_isClosedLoop != current.m_isClosedLoop ||
                m_lastKnownSettings.m_useAdaptiveSegmentation != current.m_useAdaptiveSegmentation ||
                m_lastKnownSettings.m_minSegmentsPerCurve != current.m_minSegmentsPerCurve ||
                m_lastKnownSettings.m_maxSegmentsPerCurve != current.m_maxSegmentsPerCurve ||
                m_lastKnownSettings.m_curvatureThreshold != current.m_curvatureThreshold ||
                m_lastKnownSettings.m_generateColliders != current.m_generateColliders)
            {
                settingsChanged = true;
            }
        }
        
        if (settingsChanged)
        {
            UpdateSettingsSnapshot();
            ScheduleMeshUpdate();
        }
    }
    
    /// <summary>
    /// 設定のスナップショットを更新
    /// </summary>
    private void UpdateSettingsSnapshot()
    {
        if (currentCourseData == null) return;
        
        if (m_lastKnownSettings == null)
        {
            m_lastKnownSettings = new CourseSettings();
        }
        
        CourseSettings current = currentCourseData.Settings;
        m_lastKnownSettings.m_meshResolution = current.m_meshResolution;
        m_lastKnownSettings.m_segmentsPerCurve = current.m_segmentsPerCurve;
        m_lastKnownSettings.m_isClosedLoop = current.m_isClosedLoop;
        m_lastKnownSettings.m_useAdaptiveSegmentation = current.m_useAdaptiveSegmentation;
        m_lastKnownSettings.m_minSegmentsPerCurve = current.m_minSegmentsPerCurve;
        m_lastKnownSettings.m_maxSegmentsPerCurve = current.m_maxSegmentsPerCurve;
        m_lastKnownSettings.m_curvatureThreshold = current.m_curvatureThreshold;
        m_lastKnownSettings.m_generateColliders = current.m_generateColliders;
        
        m_lastKnownPointCount = currentCourseData.PointCount;
    }
    
    /// <summary>
    /// 設定監視をリセット
    /// </summary>
    private void ResetSettingsMonitoring()
    {
        m_lastKnownSettings = null;
        m_lastKnownPointCount = 0;
        
        if (currentCourseData != null)
        {
            UpdateSettingsSnapshot();
        }
    }
    
    /// <summary>
    /// 新しい制御点を末尾に追加
    /// </summary>
    private void AddNewControlPoint()
    {
        if (currentCourseData == null) return;
        
        Undo.RecordObject(currentCourseData, "制御点を追加");
        
        int newIndex = currentCourseData.AddPointAt(currentCourseData.PointCount);
        // 追加後は自動選択しない（接線ハンドルを表示させない）
        m_selectedPointIndex = -1;
        
        // メッシュを自動更新
        if (m_autoUpdateMesh && m_previewMeshObject != null)
        {
            ScheduleMeshUpdate();
        }
        
    }
    
    /// <summary>
    /// 指定位置に制御点を挿入
    /// </summary>
    /// <param name="insertIndex">挿入位置</param>
    private void InsertControlPointAt(int insertIndex)
    {
        if (currentCourseData == null || insertIndex < 0) return;
        
        Undo.RecordObject(currentCourseData, "制御点を挿入");
        
        int newIndex = currentCourseData.AddPointAt(insertIndex);
        // 挿入後は選択を解除（接線ハンドルを表示させない）
        m_selectedPointIndex = -1;
        
        // メッシュを自動更新
        if (m_autoUpdateMesh && m_previewMeshObject != null)
        {
            ScheduleMeshUpdate();
        }
        
    }
    
    /// <summary>
    /// 選択された制御点を削除
    /// </summary>
    private void DeleteSelectedControlPoint()
    {
        if (currentCourseData == null || m_selectedPointIndex < 0) return;
        
        // 削除前の安全確認
        if (currentCourseData.PointCount <= 2)
        {
            EditorUtility.DisplayDialog("削除エラー", 
                "最低2つの制御点が必要です。削除できません。", "OK");
            return;
        }
        
        string pointInfo = $"制御点 {m_selectedPointIndex}";
        SplinePoint pointToDelete = currentCourseData.GetPoint(m_selectedPointIndex);
        if (pointToDelete != null)
        {
            pointInfo = $"制御点 {m_selectedPointIndex} (位置: {pointToDelete.position})";
        }
        
        if (EditorUtility.DisplayDialog("制御点を削除", 
            $"{pointInfo}を削除してもよろしいですか？", 
            "はい", "キャンセル"))
        {
            Undo.RecordObject(currentCourseData, "制御点を削除");
            
            bool success = currentCourseData.RemovePointSafely(m_selectedPointIndex);
            if (success)
            {
                // 選択インデックスを調整
                if (m_selectedPointIndex >= currentCourseData.PointCount)
                {
                    m_selectedPointIndex = currentCourseData.PointCount - 1;
                }
                
                // メッシュを自動更新
                if (m_autoUpdateMesh && m_previewMeshObject != null)
                {
                    ScheduleMeshUpdate();
                }
                
            }
            else
            {
                EditorUtility.DisplayDialog("削除エラー", 
                    "制御点の削除に失敗しました。", "OK");
            }
        }
    }
    
    /// <summary>
    /// CourseDataを開く
    /// </summary>
    private void OpenCourseData()
    {
        string path = EditorUtility.OpenFilePanel("コースデータを開く", "Assets", "asset");
        if (!string.IsNullOrEmpty(path))
        {
            // Assetsフォルダからの相対パスに変換
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            CourseData loadedData = AssetDatabase.LoadAssetAtPath<CourseData>(relativePath);
            
            if (loadedData != null)
            {
                currentCourseData = loadedData;
                m_selectedPointIndex = -1;
                ResetSettingsMonitoring();
            }
            else
            {
                EditorUtility.DisplayDialog("エラー", "選択されたファイルはCourseDataではありません。", "OK");
            }
        }
    }
    
    /// <summary>
    /// 現在のCourseDataを保存
    /// </summary>
    private void SaveCourseData()
    {
        if (currentCourseData == null) return;
        
        string path = AssetDatabase.GetAssetPath(currentCourseData);
        if (string.IsNullOrEmpty(path))
        {
            // 新規ファイルの場合は名前を付けて保存
            path = EditorUtility.SaveFilePanelInProject(
                "コースデータを保存",
                currentCourseData.Settings.m_courseName,
                "asset",
                "コースデータの保存場所を選択してください"
            );
            
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(currentCourseData, path);
            }
        }
        
        if (!string.IsNullOrEmpty(path))
        {
            EditorUtility.SetDirty(currentCourseData);
            AssetDatabase.SaveAssets();
        }
    }
    
    /// <summary>
    /// ヘルプダイアログを表示
    /// </summary>
    private void ShowHelpDialog()
    {
        string helpText = @"レーシングコースエディタ v1.3 ヘルプ

■ 基本操作
• 新規作成/開く/保存

■ 制御点操作
• Scene Viewでドラッグ移動
• 追加/削除/挿入

■ メッシュ操作
• 自動生成と更新
• 適応的セグメント分割
• 物理コライダー生成

■ キーボードショートカット
• Ctrl+N/O/S: ファイル操作
• Delete: 制御点削除
• G: メッシュ生成";

        EditorUtility.DisplayDialog("ヘルプ", helpText, "OK");
    }
    
    /// <summary>
    /// キーボードショートカットの処理
    /// </summary>
    private void HandleKeyboardShortcuts()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            switch (e.keyCode)
            {
                case KeyCode.N when e.control:
                    CreateNewCourseData();
                    e.Use();
                    break;
                    
                case KeyCode.O when e.control:
                    OpenCourseData();
                    e.Use();
                    break;
                    
                case KeyCode.S when e.control:
                    if (currentCourseData != null)
                    {
                        SaveCourseData();
                    }
                    e.Use();
                    break;
                    
                case KeyCode.Delete:
                    if (currentCourseData != null && m_selectedPointIndex >= 0 && currentCourseData.PointCount > 2)
                    {
                        DeleteSelectedControlPoint();
                    }
                    e.Use();
                    break;
                    
                case KeyCode.G:
                    if (currentCourseData != null && currentCourseData.IsValidCourse)
                    {
                        GeneratePreviewMesh();
                    }
                    e.Use();
                    break;
                    
                case KeyCode.H:
                    ShowHelpDialog();
                    e.Use();
                    break;
                    
                case KeyCode.A when e.control:
                    if (currentCourseData != null)
                    {
                        AddNewControlPoint();
                    }
                    e.Use();
                    break;
            }
        }
    }
    
    /// <summary>
    /// プレビューメッシュをプレハブとして保存
    /// </summary>
    private void SaveMeshAsPrefab()
    {
        if (m_previewMeshObject == null)
        {
            EditorUtility.DisplayDialog("エラー", "保存するプレビューメッシュがありません。", "OK");
            return;
        }
        
        if (currentCourseData == null)
        {
            EditorUtility.DisplayDialog("エラー", "コースデータが選択されていません。", "OK");
            return;
        }
        
        try
        {
            // 保存パスを取得
            string defaultName = currentCourseData.Settings.m_courseName + "_Mesh";
            string savePath = EditorUtility.SaveFilePanelInProject(
                "コースメッシュをプレハブとして保存",
                defaultName,
                "prefab",
                "プレハブの保存場所を選択してください",
                "Assets/Prefabs"
            );
            
            if (string.IsNullOrEmpty(savePath))
            {
                return; // キャンセルされた
            }
            
            // 一時的なGameObjectを作成してプレハブ用に準備
            GameObject prefabObject = new GameObject(defaultName);
            
            // メッシュデータをコピー
            MeshFilter originalMeshFilter = m_previewMeshObject.GetComponent<MeshFilter>();
            MeshRenderer originalMeshRenderer = m_previewMeshObject.GetComponent<MeshRenderer>();
            MeshCollider originalMeshCollider = m_previewMeshObject.GetComponent<MeshCollider>();
            
            if (originalMeshFilter != null && originalMeshFilter.sharedMesh != null)
            {
                // MeshFilterとMeshRendererを追加
                MeshFilter newMeshFilter = prefabObject.AddComponent<MeshFilter>();
                MeshRenderer newMeshRenderer = prefabObject.AddComponent<MeshRenderer>();
                
                // メッシュアセットを作成して保存
                string meshAssetPath = savePath.Replace(".prefab", "_Mesh.asset");
                Mesh meshCopy = Object.Instantiate(originalMeshFilter.sharedMesh);
                meshCopy.name = defaultName + "_Mesh";
                AssetDatabase.CreateAsset(meshCopy, meshAssetPath);
                
                // メッシュを設定
                newMeshFilter.sharedMesh = meshCopy;
                
                // マテリアルをコピー
                if (originalMeshRenderer != null)
                {
                    newMeshRenderer.materials = originalMeshRenderer.sharedMaterials;
                    newMeshRenderer.shadowCastingMode = originalMeshRenderer.shadowCastingMode;
                    newMeshRenderer.receiveShadows = originalMeshRenderer.receiveShadows;
                }
                
                // MeshColliderもコピー
                if (originalMeshCollider != null)
                {
                    MeshCollider newMeshCollider = prefabObject.AddComponent<MeshCollider>();
                    newMeshCollider.sharedMesh = meshCopy;
                    newMeshCollider.convex = originalMeshCollider.convex;
                    if (originalMeshCollider.material != null)
                    {
                        newMeshCollider.material = originalMeshCollider.material;
                    }
                }
                
                // プレハブとして保存
                GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(prefabObject, savePath);
                
                // 一時的なGameObjectを削除
                DestroyImmediate(prefabObject);
                
                // アセットデータベースを更新
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                // 保存されたプレハブを選択状態にする
                Selection.activeObject = prefabAsset;
                EditorGUIUtility.PingObject(prefabAsset);
                
                EditorUtility.DisplayDialog("プレハブ保存完了", 
                    $"コースメッシュをプレハブとして保存しました。\n\nプレハブ: {savePath}\nメッシュアセット: {meshAssetPath}", 
                    "OK");
            }
            else
            {
                DestroyImmediate(prefabObject);
                EditorUtility.DisplayDialog("エラー", "プレビューメッシュにメッシュデータがありません。", "OK");
            }
        }
        catch (System.Exception ex)
        {
            EditorUtility.DisplayDialog("プレハブ保存エラー", 
                $"プレハブの保存中にエラーが発生しました:\n{ex.Message}", 
                "OK");
        }
    }
    
    /// <summary>
    /// 適応的セグメント分割での総セグメント数を推定
    /// </summary>
    private int EstimateTotalSegments()
    {
        if (currentCourseData == null || currentCourseData.PointCount < 2)
            return 0;
            
        int totalSegments = 0;
        CourseSettings settings = currentCourseData.Settings;
        int segmentCount = settings.m_isClosedLoop ? currentCourseData.PointCount : currentCourseData.PointCount - 1;
        
        for (int i = 0; i < segmentCount; i++)
        {
            SplinePoint currentPoint = currentCourseData.GetPoint(i);
            SplinePoint nextPoint;
            
            if (settings.m_isClosedLoop)
            {
                nextPoint = currentCourseData.GetPoint((i + 1) % currentCourseData.PointCount);
            }
            else
            {
                nextPoint = currentCourseData.GetPoint(i + 1);
            }
            
            if (settings.m_useAdaptiveSegmentation)
            {
                totalSegments += SplineMath.CalculateAdaptiveSegments(
                    currentPoint, nextPoint,
                    settings.m_minSegmentsPerCurve,
                    settings.m_maxSegmentsPerCurve,
                    settings.m_curvatureThreshold
                );
            }
            else
            {
                totalSegments += settings.m_segmentsPerCurve;
            }
        }
        
        return totalSegments;
    }
    
    /// <summary>
    /// 固定セグメント分割での総セグメント数を推定
    /// </summary>
    private int EstimateFixedSegments()
    {
        if (currentCourseData == null || currentCourseData.PointCount < 2)
            return 0;
            
        CourseSettings settings = currentCourseData.Settings;
        int segmentCount = settings.m_isClosedLoop ? currentCourseData.PointCount : currentCourseData.PointCount - 1;
        
        return segmentCount * settings.m_segmentsPerCurve;
    }
   
}