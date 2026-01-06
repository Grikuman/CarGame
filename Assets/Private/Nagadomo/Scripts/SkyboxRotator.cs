using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    void Start()
    {
        // Skybox‚ğ•¡»‚µ‚Ä‘¼‚ÌƒV[ƒ“‚É‰e‹¿‚µ‚È‚¢‚æ‚¤‚É
        RenderSettings.skybox = Instantiate(RenderSettings.skybox);
    }

    void Update()
    {
        float rotation = Time.time * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}
