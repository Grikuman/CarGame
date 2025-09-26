using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Checkpoints/Recording")]
public class CheckpointRecording : ScriptableObject
{
    public List<CheckpointData> data = new List<CheckpointData>();
    public float interval = 5f;
}
