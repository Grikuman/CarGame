#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheckpointRecorder))]
public class CheckpointRecorderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CheckpointRecorder rec = (CheckpointRecorder)target;

        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("▶ 記録開始")) rec.StartRecording();
            if (GUILayout.Button("⏹ 記録終了")) rec.StopRecording();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("📍 Play中プレビュー生成")) rec.GenerateCheckpointsPlayMode();

            if (GUILayout.Button("💾 記録を.assetとして保存"))
            {
                if (rec.recordedData.Count == 0)
                {
                    Debug.LogWarning("記録データがありません");
                    return;
                }

                string path = EditorUtility.SaveFilePanelInProject(
                    "Save Recording",
                    "CheckpointRecording",
                    "asset",
                    "保存先を選択"
                );
                if (string.IsNullOrEmpty(path)) return;

                var asset = ScriptableObject.CreateInstance<CheckpointRecording>();
                asset.data = new System.Collections.Generic.List<CheckpointData>(rec.recordedData);
                asset.interval = rec.interval;

                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Selection.activeObject = asset;
                Debug.Log($"[Recorder] 記録を保存しました: {path}");
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Playモードで操作してください", MessageType.Info);
        }
    }
}
#endif
