using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheckpointToCinemachinePath))]
public class CheckpointToCinemachinePathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (CheckpointToCinemachinePath)target;

        GUILayout.Space(10);

        if (GUILayout.Button("チェックポイントから Path を生成"))
        {
            script.ApplyCheckpointsToPath();
        }
    }
}
