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
            GameObject cp = (GameObject)PrefabUtility.InstantiatePrefab(checkpointPrefab);
            cp.transform.position = d.position;
            cp.transform.rotation = d.rotation;
            cp.transform.SetParent(parent.transform);

            var checkpoint = cp.GetComponent<Checkpoint>();
            if (checkpoint != null) checkpoint.checkpointID = i;
            Undo.RegisterCreatedObjectUndo(cp, "Create Checkpoint");
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"[Generator] {recording.data.Count} 個のチェックポイントを Editモードで生成しました");
    }
}
