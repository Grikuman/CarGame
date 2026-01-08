using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
