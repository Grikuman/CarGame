using UnityEngine;

[System.Serializable]
public class CheckpointData
{
    public Vector3 position;
    public Quaternion rotation;

    public CheckpointData(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}
