using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CheckpointGeneratorWindow : EditorWindow
{
    public CheckpointRecording recording;
    public GameObject checkpointPrefab;
    public string parentName = "GeneratedCheckpoints_FromRecording";

    [MenuItem("Tools/Checkpoint Generator (from recording)")]
    public static void ShowWindow() => GetWindow<CheckpointGeneratorWindow>("Checkpoint Generator");

    void OnGUI()
    {
        recording = (CheckpointRecording)EditorGUILayout.ObjectField("Recording", recording, typeof(CheckpointRecording), false);
        checkpointPrefab = (GameObject)EditorGUILayout.ObjectField("Checkpoint Prefab", checkpointPrefab, typeof(GameObject), false);
        parentName = EditorGUILayout.TextField("Parent Name", parentName);

        if (GUILayout.Button("Generate in Scene (Edit mode)"))
        {
            if (recording == null || checkpointPrefab == null)
            {
                EditorUtility.DisplayDialog("Error", "Recording または Prefab が指定されていません。", "OK");
                return;
            }
            Generate();
        }
    }

    void Generate()
    {
        GameObject parent = new GameObject(parentName);
        Undo.RegisterCreatedObjectUndo(parent, "Create Checkpoints Parent");

        for (int i = 0; i < recording.data.Count; i++)
        {
            var d = recording.data[i];
            Vector3 pos = d.position;
            Quaternion rot = d.rotation;

            // ---------- 中央補正 ----------
            Vector3 leftDir = -(rot * Vector3.right);
            Vector3 rightDir = (rot * Vector3.right);

            //コースの幅分のレイ
            float rayLength = 50f;
            //補正判定
            bool adjusted = false;

            // デバッグ用にRayをSceneビューに描く
            Debug.DrawRay(pos, leftDir * rayLength, Color.red, 5f);
            Debug.DrawRay(pos, rightDir * rayLength, Color.blue, 5f);

            // 左右の壁を検出
            if (Physics.Raycast(pos, leftDir, out RaycastHit leftHit, rayLength))
            {
                if (Physics.Raycast(pos, rightDir, out RaycastHit rightHit, rayLength))
                {
                    // 中点を取る
                    Vector3 newPos = (leftHit.point + rightHit.point) * 0.5f;
                    Debug.DrawLine(leftHit.point, rightHit.point, Color.yellow, 5f);

                    // 差があれば補正ログを出す
                    if (Vector3.Distance(pos, newPos) > 0.05f)
                    {
                        Debug.Log($"[CenterAdjust] #{i} pos adjusted by {Vector3.Distance(pos, newPos):F2}m");
                    }

                    pos = newPos;
                    adjusted = true;
                }
            }

            if (!adjusted)
            {
                Debug.LogWarning($"[CenterAdjust] #{i} No both-side hit → not adjusted");
            }

            // ---------- チェックポイント生成 ----------
            GameObject cp = (GameObject)PrefabUtility.InstantiatePrefab(checkpointPrefab);
            cp.transform.position = pos;
            cp.transform.rotation = rot;
            cp.transform.SetParent(parent.transform);

            var checkpoint = cp.GetComponent<Checkpoint>();
            if (checkpoint != null) checkpoint.checkpointID = i;

            Undo.RegisterCreatedObjectUndo(cp, "Create Checkpoint");
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"[Generator] {recording.data.Count} 個のチェックポイントを Editモードで生成しました");
    }
}
