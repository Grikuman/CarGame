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
                EditorUtility.DisplayDialog("Error", "Recording �܂��� Prefab ���w�肳��Ă��܂���B", "OK");
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

            // ---------- �����␳ ----------
            Vector3 leftDir = -(rot * Vector3.right);
            Vector3 rightDir = (rot * Vector3.right);

            //�R�[�X�̕����̃��C
            float rayLength = 50f;
            //�␳����
            bool adjusted = false;

            // �f�o�b�O�p��Ray��Scene�r���[�ɕ`��
            Debug.DrawRay(pos, leftDir * rayLength, Color.red, 5f);
            Debug.DrawRay(pos, rightDir * rayLength, Color.blue, 5f);

            // ���E�̕ǂ����o
            if (Physics.Raycast(pos, leftDir, out RaycastHit leftHit, rayLength))
            {
                if (Physics.Raycast(pos, rightDir, out RaycastHit rightHit, rayLength))
                {
                    // ���_�����
                    Vector3 newPos = (leftHit.point + rightHit.point) * 0.5f;
                    Debug.DrawLine(leftHit.point, rightHit.point, Color.yellow, 5f);

                    // ��������Ε␳���O���o��
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
                Debug.LogWarning($"[CenterAdjust] #{i} No both-side hit �� not adjusted");
            }

            // ---------- �`�F�b�N�|�C���g���� ----------
            GameObject cp = (GameObject)PrefabUtility.InstantiatePrefab(checkpointPrefab);
            cp.transform.position = pos;
            cp.transform.rotation = rot;
            cp.transform.SetParent(parent.transform);

            var checkpoint = cp.GetComponent<Checkpoint>();
            if (checkpoint != null) checkpoint.checkpointID = i;

            Undo.RegisterCreatedObjectUndo(cp, "Create Checkpoint");
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"[Generator] {recording.data.Count} �̃`�F�b�N�|�C���g�� Edit���[�h�Ő������܂���");
    }
}
