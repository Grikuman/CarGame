using UnityEngine;
using Cinemachine;

public class CourseManager : MonoBehaviour
{
    public static CourseManager Instance { get; private set; }

    [SerializeField] private CinemachineSmoothPath coursePath;
    public CinemachineSmoothPath CoursePath => coursePath;

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
