using UnityEngine;
using Cinemachine;

public class CoursePathManager : MonoBehaviour
{
    public static CoursePathManager Instance { get; private set; }

    [SerializeField] private CinemachinePathBase path;

    public CinemachinePathBase Path => path;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
